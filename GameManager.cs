using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    [SerializeField] public Controller m_Controller;

    private void Awake()
    {
        Instance = this;
    }

}
