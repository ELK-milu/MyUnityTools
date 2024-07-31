using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;


public class InputUIEventProxy:IInputProxyBase
{
    public static InputButtonClickRegisterInfo GetOnClickListener(Button button, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnClickEvent> callback)
    {
        InputButtonClickRegisterInfo info = HeapObjectPool<InputButtonClickRegisterInfo>.GetObject();

        info.eventKey = InputUIOnClickEvent.GetEventKey(UIName, ComponentName, parm);
        info.callBack = callback;
        info.m_button = button;
        info.m_OnClick = () =>
        {
            DispatchOnClickEvent(UIName, ComponentName, parm);
        };

        return info;
    }

    public static void DispatchOnClickEvent(string UIName, string ComponentName, string parm)
    {
        //只有允许输入时才派发事件
        if (IsActive)
        {
            InputUIOnClickEvent e = GetUIEvent<InputUIOnClickEvent>(UIName, ComponentName, parm);
            InputManager.Dispatch("InputUIOnClickEvent", e);
        }
    }

    static T GetUIEvent<T>(string UIName, string ComponentName, string parm) where T : InputUIEventBase, new()
    {
        T msg = HeapObjectPool<T>.GetObject();
        msg.Reset();
        msg.m_name = UIName;
        msg.m_compName = ComponentName;
        msg.m_param = parm;

        return msg;
    }
}


public class InputButtonClickRegisterInfo : InputEventRegisterInfo<InputUIOnClickEvent>
{
    public Button m_button;
    public UnityAction m_OnClick;

    public override void RemoveListener(bool isSole)
    {
        base.RemoveListener(isSole);
        if (isSole)
        {
            m_button.onClick.RemoveListener(m_OnClick);
        }
    }

    public override void AddListener(bool isSole)
    {
        base.AddListener(isSole);

        if (isSole)
        {
            m_button.onClick.AddListener(m_OnClick);
        }
    }
}
