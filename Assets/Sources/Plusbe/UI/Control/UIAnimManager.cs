using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimManager : MonoBehaviour {

    public void StartEnterAnim(UIWindowBase uiBase, UICallBack callBack, params object[] objs)
    {
        UISystemEvent.Dispatch(uiBase, UIEvent.OnStartEnterAnim);

        StartCoroutine(uiBase.EnterAnim(EndEnterAnim, callBack, objs)); //内部回调，进场动画结束播放
    }

    public void EndEnterAnim(UIWindowBase uiBase, UICallBack callBack, params object[] objs)
    {
        UISystemEvent.Dispatch(uiBase, UIEvent.OnCompleteEnterAnim);
        uiBase.OnCompleteEnterAnim();
        uiBase.windowStatus = UIWindowBase.WindowStatus.Open;
        try
        {
            if (callBack != null)
            {
                callBack(uiBase, objs);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    public void StartExitAnim(UIWindowBase uiBase, UICallBack callBack, params object[] objs)
    {
        UISystemEvent.Dispatch(uiBase, UIEvent.OnStartExitAnim);
        StartCoroutine(uiBase.ExitAnim(EndExitAnim, callBack, objs));
    }

    public void EndExitAnim(UIWindowBase uiBase, UICallBack callBack, params object[] objs)
    {
        UISystemEvent.Dispatch(uiBase, UIEvent.OnCompleteExitAnim);
        uiBase.OnCompleteExitAnim();
        uiBase.windowStatus = UIWindowBase.WindowStatus.Close;
        try
        {
            if (callBack != null)
            {
                callBack(uiBase, objs);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    
}
