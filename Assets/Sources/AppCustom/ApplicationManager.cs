using Plusbe.AppManager;
using Plusbe.Config;
using Plusbe.Core;
using Plusbe.Develop;
using Plusbe.Encrypt;
using Plusbe.Message;
using Plusbe.Net;
using Plusbe.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : ApplicationManagerBase {

    private static ApplicationManager instance;


    public static ApplicationManager Instance
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ApplicationManager>();
            }
            return ApplicationManager.instance;
        }
        set
        {
            ApplicationManager.instance = value;
        }
    }

    /// <summary>
    /// 程序启动
    /// </summary>
    public void Awake()
    {
        instance = this;
        AppLaunch();
    }

    protected override void AppLaunch()
    {
        base.AppLaunch();

        InitUserData();

        Debug.Log("框架初始化完成 ~_~ ");

        InitLogic();

        InitScreenBack();
    }


    /// <summary>
    /// 业务界面逻辑初始化
    /// </summary>
    protected void InitLogic()
    {
        if(AppRunName == AppName.TestApp)
        {
            //UIManager.OpenUIWindow<PicPlayerWindow>();

            UIManager.OpenUIWindow<HomepageWindow>();

            //UIManager.OpenUIWindow<NewVideoPlayerWindow>();

           // UIManager.OpenUIWindow<TestEncryptionWindow>();

            return;
        }
    }


    #region 屏保操作
    /// <summary>
    /// 屏保操作
    /// 1.如果启用了status，进入status
    /// 2.如果只是openui，HideOtherUI
    /// </summary>
    protected void ScreenCallBack()
    {
        Debug.Log("ScreenCallBack");

        //屏保操作 未测试！！！
        //1. 状态操作 首页屏保切换 ApplicationStatusManager.EnterStatus<PlusbeTestStatus>();
        //2. 窗体操作 销毁所有窗体初始化 UIManager.DestroyAll();UIManager.OpenUIWindow<PicPlayerWindow>();


        if (AppRunName == AppName.TestApp)
        {
            UIManager.DestroyAll();

            UIManager.OpenUIWindow<TestFirstWindow>();

            //UIManager.OpenUIWindow<PicPlayerWindow>();
        }

        //ApplicationStatusManager.EnterStatus<PlusbeTestStatus>();

        //if (EnabledJsonConfig) JsonDataManager.Init();  //json数据初始化
        //if (EnabledFileConfig) FileConfig.Init();
    }

    private void InitScreenBack()
    {
        if (EnabledScreen)
        {
            ScreenManager.Init(ScreenCallBack);
        }
    }

    #endregion

    /// <summary>
    /// 部分用户信息初始化
    /// </summary>
    private void InitUserData()
    {
        Application.targetFrameRate = AppConfig.Instance.GetValueByKeyInt("TargetFrame");

        //GlobalSetting.testA = AppConfig.Instance.GetValueByKey("TargetFrame");

    }

    public static AppName AppRunName
    {
        get { return Instance.appRunName; }
    }

    public static AppMode AppRunMode
    {
        get { return Instance.appRunMode; }
    }
}

public enum AppName
{
    None,
    TestApp,
    UIFrame,
}
