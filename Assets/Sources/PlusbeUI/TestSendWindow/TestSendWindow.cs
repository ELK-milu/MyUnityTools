using UnityEngine;
using System.Collections;
using Plusbe.Helper;
using Plusbe.Message;

public class TestSendWindow : UIWindowBase 
{

    public override void OnInit()
    {
        NotificationCenter.Instance.AddObserver(this, "TestHttp");
    }

    //UI打开的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("btn_back", OnClick1, "我是参数");
    }

    public void TestHttp(Notification notification)
    {
        Debug.Log("TestHttp：" + notification.data.ToString());
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        AnimationHelper.DoScaleIn(this.m_uiRoot.transform);

        return base.EnterAnim(animComplete, callBack, objs);
    }

    private void OnClick1(InputUIOnClickEvent e)
    {
        UIManager.OpenUIWindow<TestFirstWindow>(this);

        //ProcessHelper.KillMySelf();

        //ProcessHelper.KillProcess("PlusbeTestDemo");

        //if (!Application.isEditor)
        //{
        //    Debug.Log("即将杀掉本进程");
        //    ProcessHelper.KillProcess("PlusbeTestDemo");
        //}
    }
}