using Plusbe.AppManager;
using Plusbe.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWindow : UIWindowBase {

    private int count = 0;

    private bool isLongAnim;

    public override void OnOpen()
    {
        //base.OnOpen();
        AddOnClickListener("btn_start", OnClickSetting);

        AddOnClickListener("btn_add_hide",OnClickAddHide);

        

        //Debug.Log("当前状态：" + windowStatus);
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        count++;
        isLongAnim = true;

        AnimationHelper.DoScaleIn(this.m_uiRoot.transform);

        //for (int i = 0; i < 1; i++)
        //{
        //    Debug.Log(lastCount  + ">> is playing < " + i + " >");
        //    yield return new WaitForSeconds(1f);
        //}

        animComplete(this, callBack, objs);

        yield break;
    }

    public override void OnCompleteEnterAnim()
    {
        isLongAnim = false;
        //Debug.Log("Enter Anim Com");
    }

    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        if (isLongAnim)
        {
            //StopCoroutine(EnterAnim);
        }

        

        //Debug.Log("当前状态：" + windowStatus);
        if (windowStatus == WindowStatus.CloseAnim || windowStatus == WindowStatus.Close || windowStatus == WindowStatus.Hide)
        {
            Debug.Log("处于非打开状态");
        }
        else
        {
            //yield return new WaitForSeconds(0.5f);
            AnimationHelper.DoScaleBigOut(this.m_uiRoot.transform, (() =>
            {
                Debug.Log("callback exit");
                StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
            }));
        }
        

        Debug.Log("end exit");
        yield return new WaitForEndOfFrame();
    }

    public void OnClickSetting(InputUIOnClickEvent e)
    {
        //Debug.Log("OnClickSetting");
        ApplicationStatusManager.EnterStatus<SecondStatus>();
    }

    public void OnClickAddHide(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow(this);
        //UIManager.CreateUIWindowOne<AVVideoPlayerWindow>();
        //UIManager.OpenUIWindow<AVVideoPlayerWindow>();
    }
}
