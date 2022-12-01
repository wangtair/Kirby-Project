using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] private UIButton m_ButtonUp;
    [SerializeField] private UIButton m_ButtonDown;
    [SerializeField] private UIButton m_ButtonLeft;
    [SerializeField] private UIButton m_ButtonRight;
    [SerializeField] private UIButton m_ButtonA;
    [SerializeField] private UIButton m_ButtonB;
    [SerializeField] private UIButton m_ButtonX;
    [SerializeField] private UIButton m_ButtonY;

    public bool m_ButtonUpDown;
    public bool m_ButtonDownDown;
    public bool m_ButtonLeftDown;
    public bool m_ButtonRightDown;
    public bool m_ButtonADown;
    public bool m_ButtonBDown;
    public bool m_ButtonXDown;
    public bool m_ButtonYDown;



    void Awake()
    {
        InitTheKeys();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitTheKeys() 
    {
        m_ButtonUp.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "up");
        m_ButtonDown.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "down");
        m_ButtonLeft.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "left");
        m_ButtonRight.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "right");
        m_ButtonA.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "A");
        m_ButtonB.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "B");
        m_ButtonX.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "X");
        m_ButtonY.RegisterTheDownAndUpFunction(GetInputDown, GetInputUp, "Y");
    }

    void InitFunction(string _string) 
    {
        Debug.Log(_string);
    }

    void GetInputDown(string _string)
    {
        switch (_string)
        {
            case ("up"): {
                    m_ButtonUpDown = true;
                    break;
                }
            case ("down"):
                {
                    m_ButtonDownDown = true;
                    break;
                }
            case ("left"):
                {
                    m_ButtonLeftDown = true;
                    break;
                }
            case ("right"):
                {
                    m_ButtonRightDown = true;
                    break;
                }
            case ("A"):
                {
                    m_ButtonADown = true;
                    break;
                }
            case ("B"):
                {
                    m_ButtonBDown = true;
                    break;
                }
            case ("X"):
                {
                    m_ButtonXDown = true;
                    break;
                }
            case ("Y"):
                {
                    m_ButtonYDown = true;
                    break;
                }
        }
    }

    void GetInputUp(string _string)
    {
        switch (_string)
        {
            case ("up"):
                {
                    m_ButtonUpDown = false;
                    break;
                }
            case ("down"):
                {
                    m_ButtonDownDown = false;
                    break;
                }
            case ("left"):
                {
                    m_ButtonLeftDown = false;
                    break;
                }
            case ("right"):
                {
                    m_ButtonRightDown = false;
                    break;
                }
            case ("A"):
                {
                    m_ButtonADown = false;
                    break;
                }
            case ("B"):
                {
                    m_ButtonBDown = false;
                    break;
                }
            case ("X"):
                {
                    m_ButtonXDown = false;
                    break;
                }
            case ("Y"):
                {
                    m_ButtonYDown = false;
                    break;
                }
        }
    }
}
