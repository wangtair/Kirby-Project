using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClassMode : MonoBehaviour
{
    int m_Index = 0;
    // Update is called once per frame
    private int m_CareerType = 0;
    
    protected virtual void Update()
    {
        if (m_CareerType != 0) { 
            if (Input.GetKeyDown("c"))
            {
                ChangeTheClass(0);
            }

            if (Input.GetKeyDown("a"))
            {
                ChangeTheClass(1);
            }

            if (Input.GetKeyDown("b"))
            {
                ChangeTheClass(2);
            }

            else return;
        }

        if (Input.GetKeyDown("1"))
        {
            m_CareerType = 1;
            AddTheClass();
        }

        if (Input.GetKeyDown("2"))
        {
            m_CareerType = 2;
            AddTheClass();
        }

        if (Input.GetKeyDown("p")) Debug.Log("A");

}

    void AddTheClass()
    {
        switch (m_CareerType)
        {
            case (1):
                {
                    gameObject.AddComponent<ChangeClassModeB>();
                    break;
                }
            case (2):
                {
                    gameObject.AddComponent<ChangeClassModeC>();
                    break;
                }
        }
    }

    void ChangeTheClass(int _newCareerIndex)
    {
        switch (m_CareerType)
        {
            case (1):
                {
                    ChangeClassModeB component = GetComponent<ChangeClassModeB>();
                    Destroy(component);
                    break;
                }
            case (2):
                {
                    ChangeClassModeC component = GetComponent<ChangeClassModeC>();
                    Destroy(component);
                    break;
                }
        }
        m_CareerType = _newCareerIndex;
        if (m_CareerType != 0) AddTheClass();
    }
}
