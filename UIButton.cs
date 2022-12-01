using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private Button button;
    private string m_keyName;

    private Action<string> m_OnPressDownFunction;
    private Action<string> m_OnPressUpFunction;

    public void RegisterTheDownAndUpFunction(Action<string> _actionDown, Action<string> _actionUp, string _keyName)
    {
        m_keyName = _keyName;
        m_OnPressDownFunction = _actionDown;
        m_OnPressUpFunction = _actionUp;
    }



    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_OnPressDownFunction?.Invoke(m_keyName);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_OnPressUpFunction?.Invoke(m_keyName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
