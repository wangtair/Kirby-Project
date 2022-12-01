using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalKirby : MonoBehaviour
{
    [SerializeField] private Controller m_Controller;
    [SerializeField] private Detector m_FloorDetector;
    [SerializeField] private Detector m_GroundDetector;
    [SerializeField] private RectTransform m_CollidersRect;


    [SerializeField] private RectTransform m_Rect;
    [SerializeField] private Rigidbody2D m_Rigid;
    [SerializeField] private Animator m_anim;

    [SerializeField] private PlayerStates m_PlayerStates = PlayerStates.idle;

    private bool m_Up { get { return m_Controller.m_ButtonUpDown; } }
    private bool m_Down { get { return m_Controller.m_ButtonDownDown; } }
    private bool m_Left { get { return m_Controller.m_ButtonLeftDown; } }
    private bool m_Right { get { return m_Controller.m_ButtonRightDown; } }
    private bool m_A { get { return m_Controller.m_ButtonADown; } }
    private bool m_B { get { return m_Controller.m_ButtonBDown; } }
    private bool m_X { get { return m_Controller.m_ButtonXDown; } }
    private bool m_Y { get { return m_Controller.m_ButtonYDown; } }

    [SerializeField] private bool m_OnTheFloor;
    private bool m_Jumping;

    private float m_PlayerSpeed { 
        get { return AttributeData.Instance.m_PlayerWalkSpeed; }
    }

    private float m_PlayerRunIndex
    {
        get { return AttributeData.Instance.m_PlayerRunIndex; }
    }

    private float m_PlayerHorizontalSensitivity
    {
        get { return AttributeData.Instance.m_PlayerHorizontalSensitivity; }
    }

    private float m_PlayerShutRate
    {
        get { return AttributeData.Instance.m_PlayerShutRate; }
    }

    private float m_PlayerJumpVel 
    {
        get { return AttributeData.Instance.m_PlayerJumpVel; }
    }

    private float m_PlayerJumpTime
    {
        get { return AttributeData.Instance.m_PlayerJumpTime; }
    }

    private float m_PlayerWalkToRunInternalTime
    {
        get { return AttributeData.Instance.m_PlayerWalkToRunInternalTime; }
    }

    private float m_PlayerFlyOnceVel {
        get { return AttributeData.Instance.m_PlayerFlyOnceVel; }
    }

    private float m_PlayerFlyOnceTime
    {
        get { return AttributeData.Instance.m_PlayerFlyOnceTime; }
    }
   


    private Vector3 m_DefaultSca;
    private Vector3 m_DefaultCollidersSca;

    [SerializeField] private float m_HorizontalAxisRate = 0;

    private float m_StateVelInterval = 0.01f;

    //Animation Calculation Time
    private float m_JumpTime = Mathf.NegativeInfinity;
    private float m_WalkTime = Mathf.NegativeInfinity;
    private float m_FlyTime = Mathf.NegativeInfinity;

    //State Stage variable
    private int m_JumpStages;

    //Animation Transition Mode
    private bool m_TransitionAnimation = false;

    //Coroutine
    private bool m_IsCoroutine;

    //Transformation Index
    private int m_TransformationIndex = 0;

    private bool m_IsFlying;

    private Vector3 m_Vel {
        get { return m_Rigid.velocity; }
        set { m_Rigid.velocity = value; }
    }

    private Vector3 m_Sca
    {
        get { return m_Rect.localScale; }
        set { m_Rect.localScale = value; }
    }

    private Vector3 m_CollidersScale {
        get { return m_CollidersRect.localScale; }
        set { m_CollidersRect.localScale = value; }
    }

    private void Start()
    {
        m_DefaultSca = m_Sca;
        m_GroundDetector.AddListener("Floor", DetectFloor);
        m_DefaultCollidersSca = m_CollidersScale;
    }

    private void Update()
    {

        if (Input.GetKeyDown("p"))
        {
            PDebug();
        }

        //float horizontalAxis = CalculateTheHorizontalAxis();
        StateMachine();

        
    }

    private void FixedUpdate()
    {
        MovementChange();
        AnimationChange();
    }

    void StateMachine()
    {
        switch (m_PlayerStates) {
            case (PlayerStates.idle):
                {
                    //关于Transition转回来的情况
                    if (m_TransitionAnimation)
                    {
                        if (!m_IsCoroutine)
                        {
                            this.DelayToInvoke(() => {
                                SwitchTransitionAnimationMode(false);
                                m_IsCoroutine = false;
                            }, 0.05f);
                            m_IsCoroutine = true;
                        }
                    }

                    if (m_Left || m_Right)
                    {
                        //关于变为跑的功能
                        if (m_Left && m_Right) return;

                        //=>
                        if (Time.time - m_WalkTime < m_PlayerWalkToRunInternalTime)//每次Update都判定一遍，这不要命么。。。
                        {
                            Debug.Log("State => Run");
                            StateChange(PlayerStates.run);
                            return;
                        }

                        m_WalkTime = Time.time;
                        StateChange(PlayerStates.walk);
                    }
                    DetectJump();
                    DetectSneak(false);
                    break;
                }
            case (PlayerStates.walk):
                {
                    //if(Time.time - m_WalkTime < m_PlayerWalkToRunInternalTime)
                    //{
                    //    //(m_Vel.x > 0 && )
                    //}

                    //=>
                    if (!m_Left && !m_Right && (m_Vel.x > -m_StateVelInterval || m_Vel.x < m_StateVelInterval))
                    {
                        StateChange(PlayerStates.idle);
                    }
                    DetectJump();

                    break;
                }
            case (PlayerStates.jump):
                {
                    if (m_Jumping)
                    {
                        if (!m_A)
                        {
                            m_Jumping = false;
                            SwitchTransitionAnimationMode(true);
                        }
                            if (Time.time - m_JumpTime > m_PlayerJumpTime)
                        {
                            m_Jumping = false;
                            SwitchTransitionAnimationMode(true);
                        }
                    }
                    else
                    {
                        if (m_TransitionAnimation)
                        {
                            if (!m_IsCoroutine)
                            {
                                this.DelayToInvoke(() => {
                                    SwitchTransitionAnimationMode(false);
                                    m_IsCoroutine = false;
                                }, 0.4f);
                                m_IsCoroutine = true;
                            }
                        }

                        if (m_A)
                        {
                            //=>上来就飞。必须得先撒开，然后再按一次的时候
                            m_IsFlying = true;
                            //m_TransformationIndex = 0;
                            m_FlyTime = Time.time;
                            StateChange(PlayerStates.fly);
                        };
                    }

                    
                    break;
                }
            case (PlayerStates.run):
                {

                    if (!m_Left && !m_Right && (m_Vel.x > -m_StateVelInterval || m_Vel.x < m_StateVelInterval))
                    {
                        StateChange(PlayerStates.idle);
                    }
                    DetectJump();
                    break;
                }
            case (PlayerStates.sneak):
                {
                    DetectSneak(true);
                    break;
                }
            case (PlayerStates.fly):
                {
                    if (m_IsFlying)
                    {
                        //float flyingTime = Time.time;
                        //if (flyingTime - m_FlyTime > m_PlayerFlyOnceTime) m_IsFlying = false;
                        m_IsFlying = false;
                    }
                    else
                    {
                        //不飞的情况-这一方法根本没用，貌似还得搞点击参数去 
                        if (m_A)
                        {
                            m_IsFlying = true;
                            //m_FlyTime = Time.time;
                        }
                    }

                    if (m_B)
                    {
                        //吐口气还原回去的状态
                    }


                    break;
                }
        }
    }

    void StateChange(PlayerStates _playerState)
    {
        switch (_playerState)
        {
            case (PlayerStates.idle):
                {
                    m_PlayerStates = PlayerStates.idle;
                    break;
                }
            case (PlayerStates.walk):
                {
                    m_PlayerStates = PlayerStates.walk;
                    break;
                }
            case (PlayerStates.jump):
                {
                    m_PlayerStates = PlayerStates.jump;
                    break;
                }
            case (PlayerStates.run):
                {
                    m_PlayerStates = PlayerStates.run;
                    break;
                }
            case (PlayerStates.sneak):
                {
                    m_PlayerStates = PlayerStates.sneak;
                    break;
                }
            case (PlayerStates.fly):
                {
                    m_PlayerStates = PlayerStates.fly;
                    break;
                }
        }
    }

    void MovementChange()
    {
        Vector3 vel = m_Vel;
        Vector3 sca = m_Sca;
        bool running = m_PlayerStates == PlayerStates.run ? true : false;
        AdjustHorizontalAxis(running);
        switch (m_PlayerStates)
        {
            case (PlayerStates.idle):
                {
                    //vel.x = 0;
                    break;
                }
            case (PlayerStates.walk):
                {
                    vel.x = m_HorizontalAxisRate * m_PlayerSpeed;

                    //=>
                    sca.x = m_Vel.x < 0 ? -m_DefaultSca.x : m_DefaultSca.x;
                    break;
                }
            case (PlayerStates.jump):
                {
                    if (m_Jumping)
                    {
                        vel.y = m_PlayerJumpVel;
                    }
                    

                    vel.x = m_HorizontalAxisRate * m_PlayerSpeed;
                    //=>
                    sca.x = m_Vel.x < 0 ? -m_DefaultSca.x : m_DefaultSca.x;
                    break;
                }
            case (PlayerStates.run):
                {
                    vel.x = m_HorizontalAxisRate * m_PlayerSpeed * m_PlayerRunIndex;
                    //=>
                    sca.x = m_Vel.x < 0 ? -m_DefaultSca.x : m_DefaultSca.x;
                    break;
                }
            case (PlayerStates.fly):
                {
                    if (m_IsFlying)
                    {
                        vel.y += m_PlayerFlyOnceVel;
                    }
                    break;
                }
        }
        m_Vel = vel;
        m_Sca = sca;
    }

    void AnimationChange()
    {
        switch (m_PlayerStates)
        {
            case (PlayerStates.idle):
                {
                    if (m_TransitionAnimation) m_anim.CrossFade("Sneak", 0);
                    else m_anim.CrossFade("Idle", 0);
                    break;
                }
            case (PlayerStates.walk):
                {
                    if((m_Vel.x > 0 && m_Left)|| (m_Vel.x < 0 && m_Right)) m_anim.CrossFade("Shut", 0);
                    else m_anim.CrossFade("Walk", 0);
                    break;
                }
            case (PlayerStates.jump):
                {
                    if(m_Jumping) m_anim.CrossFade("Jump", 0);
                    else
                    {
                        if (m_TransitionAnimation) m_anim.CrossFade("JumpToDrop", 0);
                        else m_anim.CrossFade("Drop", 0);
                    }
                    
                    break;
                }
            case (PlayerStates.run):
                {
                    if ((m_Vel.x > 0 && m_Left) || (m_Vel.x < 0 && m_Right)) m_anim.CrossFade("Shut", 0);
                    else m_anim.CrossFade("Run", 0);
                    break;
                }
            case (PlayerStates.sneak):
                {
                    m_anim.CrossFade("Sneak", 0);
                    break;
                }
            case (PlayerStates.fly):
                {
                    if (m_IsFlying)
                    {
                        m_anim.CrossFade("Fly", 0);
                    } else
                    {
                        m_anim.CrossFade("FlyDrop", 0);
                    }
                    break;
                }
        }
    }

    void AdjustHorizontalAxis(bool _run)
    {
        float horizontalAxisRate = m_HorizontalAxisRate;
        float runAcceleration = _run? m_PlayerRunIndex : 1;
        if (m_Left)
        {
            if(horizontalAxisRate > -1)  horizontalAxisRate -= m_PlayerHorizontalSensitivity;
        }
        else
        {
            if (horizontalAxisRate < 0) horizontalAxisRate += m_PlayerHorizontalSensitivity * m_PlayerShutRate;
        }

        if (m_Right)
        {
            if (horizontalAxisRate < 1) horizontalAxisRate += m_PlayerHorizontalSensitivity;
        }
        else
        {
            if (horizontalAxisRate > 0) horizontalAxisRate -= m_PlayerHorizontalSensitivity * m_PlayerShutRate;
        }
        m_HorizontalAxisRate = horizontalAxisRate;
    }

    void DetectJump()
    {
        m_Jumping = true;
        m_JumpTime = Time.time;
        if(m_A) StateChange(PlayerStates.jump);
    }

    void DetectSneak(bool _sneaking)
    {
        if (_sneaking)
        {
            if (!m_Down) StateChange(PlayerStates.idle);
        } else
        {
            if (m_Down) StateChange(PlayerStates.sneak);
        }
        ScaleChange(m_PlayerStates);
    }

    void DetectFloor(bool _onTheFloor)
    {
        m_OnTheFloor = _onTheFloor;
        PlayerStates playerState = m_OnTheFloor ? PlayerStates.idle : PlayerStates.jump;
        StateChange(playerState);
        if (_onTheFloor) SwitchTransitionAnimationMode(true);
    }

    void SwitchTransitionAnimationMode(bool _transitionAnimation)
    {
        Debug.Log(_transitionAnimation);
        m_TransitionAnimation = _transitionAnimation;
    }

    void ScaleChange(PlayerStates _playerStates)
    {
        switch (_playerStates)
        {
            case (PlayerStates.idle):
                {
                    m_CollidersScale = m_DefaultCollidersSca;
                    break;
                }
            case (PlayerStates.sneak):
                {
                    Vector3 newScale = m_DefaultCollidersSca;
                    newScale.y *= 0.3f;
                    m_CollidersScale = newScale;
                    break;
                }
        }
    }

    void PDebug()
    {
        Debug.Log(m_PlayerStates);
    }

    private enum PlayerStates{
        idle, walk, run, jump, sneak, fly
    }
}
