using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private Action<bool> m_DetectorAction;
    private string m_DetectTag;
    // Start is called before the first frame update
    public void AddListener(string _detectTag, Action<bool> _detectorAction)
    {
        m_DetectorAction = _detectorAction;
        m_DetectTag = _detectTag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == m_DetectTag)
        {
            m_DetectorAction?.Invoke(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == m_DetectTag)
        {
            m_DetectorAction?.Invoke(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == m_DetectTag)
        {
            m_DetectorAction?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == m_DetectTag)
        {
            m_DetectorAction?.Invoke(false);
        }
    }
}
