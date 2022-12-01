using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeData : MonoBehaviour
{
    static public AttributeData Instance;
    public float m_PlayerWalkSpeed;
    public float m_PlayerRunIndex;
    public float m_PlayerHorizontalSensitivity = 0.01f;
    public float m_PlayerShutRate = 0.3f;
    public float m_PlayerJumpVel = 5f;
    public float m_PlayerFlyOnceVel = 3f;


    public float m_PlayerJumpTime = 0.5f;
    public float m_PlayerWalkToRunInternalTime = 0.2f;
    public float m_PlayerFlyOnceTime = 0.3f;

    private void Awake()
    {
        Instance = this;
    }
}
