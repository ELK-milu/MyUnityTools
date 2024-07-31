using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class IInputDispatcher
{
    public InputEventCallBack m_OnAllEventDispatch;

    public abstract void AddListener(string eventKey, InputEventHandle<IInputEventBase> callBack);

    public abstract void RemoveListener(string eventKey, InputEventHandle<IInputEventBase> callBack);

    public abstract void Dispatch(IInputEventBase inputEvent);

    protected void AllEventDispatch(string eventName, IInputEventBase inputEvent)
    {
        if (m_OnAllEventDispatch != null)
        {
            try
            {
                m_OnAllEventDispatch(eventName, inputEvent);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}

public delegate void InputEventHandle<T>(T inputEvent);

