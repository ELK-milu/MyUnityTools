using Plusbe.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowBase : UIBase {

    public UIType m_UIType;
    [HideInInspector]
    public string uiWindowPath;
    [NonSerialized]
    public WindowStatus windowStatus;

    public GameObject m_bgMask;
    public GameObject m_uiRoot;

    #region 重载方法

    public virtual void OnOpen() { }

    public virtual void OnClose() { }

    public virtual void OnShow() { }

    public virtual void OnHide() { }

    public virtual void OnRefresh() { }

    public virtual IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        animComplete(this, callBack, objs);
        yield break;
    }

    public virtual void OnCompleteEnterAnim() { }

    public virtual IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        animComplete(this, callBack, objs);
        yield break;
    }

    public virtual void OnCompleteExitAnim() { }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region 继承方法

    //刷新是主动调用
    public void Refresh(params object[] args)
    {
        UISystemEvent.Dispatch(this, UIEvent.OnRefresh);
        OnRefresh();
    }

    public void AddEventListener(Enum event_)
    {
        if (!m_EventNames.Contains(event_))
        {
            m_EventNames.Add(event_);
            GlobalEvent.AddEvent(event_, Refresh);
        }
    }

    public void RemoveAllEventListener()
    {
        base.RemoveAllListener();

        for (int i = 0; i < m_EventNames.Count; i++)
        {
            GlobalEvent.RemoveEvent(m_EventNames[i], Refresh);
        }

        m_EventNames.Clear();
    }

    #endregion

    public enum WindowStatus
    {
        Create,
        Open,
        Close,
        OpenAnim,
        CloseAnim,
        Hide,
    }
}
