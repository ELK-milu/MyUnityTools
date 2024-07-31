using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class InputUIEventBase:IInputEventBase
{
    public string m_name;
    public string m_compName;
    public InputUIEventType m_type;
    public string m_param;

    public InputUIEventBase()
        : base()
    {
        m_name = "";
        m_compName = "";
    }

    public InputUIEventBase(string uiName, string componentName, InputUIEventType type, string param = null)
    {
        m_name = uiName;
        m_compName = componentName;
        m_type = type;
        m_param = param;
    }

    protected override string GetEventKey()
    {
        return GetEventKey(m_name, m_compName, m_type, m_param);
    }

    public static string GetEventKey(string UIName, string ComponentName, InputUIEventType type, string pram = null)
    {
        return UIName + "." + ComponentName + "." + pram + "." + type.ToString();
    }
}


public enum InputUIEventType
{ 
    Click,

    Scroll,

    PressDown,
    PressUp,

    LongPress,

    BeginDrag,
    Drag,
    EndDrag
}
