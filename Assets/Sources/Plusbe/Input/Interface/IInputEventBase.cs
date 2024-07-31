using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class IInputEventBase
{
    public float m_t = 0;
    protected string m_eventKey;

    public IInputEventBase()
    {
        Reset();
    }

    public virtual void Reset()
    {
        m_eventKey = null;
        //m_t = DevelopReplayManager.CurrentTime; ???
    }

    public string EventKey
    {
        get
        {
            if (m_eventKey == null)
            {
                m_eventKey = GetEventKey();
            }

            return m_eventKey;
        }
        set
        {
            m_eventKey = value;
        }
    }

    protected virtual string GetEventKey()
    {
        if (m_eventKey == null)
        {
            m_eventKey = ToString();
        }

        return m_eventKey;
    }

    ///// <summary>
    ///// 序列化，使一个输入事件变成可保存的文本
    ///// </summary>
    ///// <returns></returns>
    //public virtual string Serialize()
    //{
    //    return Serializer.Serialize(this);
    //}

    ///// <summary>
    ///// 解析，将一个文本变成输入事件的数据
    ///// </summary>
    ///// <param name="data"></param>
    ///// <returns></returns>
    //public IInputEventBase Analysis(string data)
    //{
    //    return JsonUtility.FromJson<IInputEventBase>(data);
    //}
}

