using AppCustom;
using Plusbe.Config;
using Plusbe.Core;
using Plusbe.Develop;
using Plusbe.Encrypt;
using Plusbe.Helper;
using Plusbe.Message;
using Plusbe.Net;
using Plusbe.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plusbe.AppManager
{
    public class ApplicationManagerBase : MonoBehaviour
    {
        [HideInInspector]
        public string currentStatus;

        public bool EnabledWindowMode = true;
        public bool EnabledJsonConfig;
        public bool EnabledFileConfig;
        public bool EnabledLog = true;
        

        public bool EnabledScreen;

        public bool EnabledServer;
        public ServerType MServerType = ServerType.Http;

        public AppName appRunName;
        public AppMode appRunMode = AppMode.Release;

        private bool EnabledKillProcess;
        private bool EnabledUpdateServer;
        private bool EnabledHideTaskbar;

        protected virtual void AppLaunch()
        {
            DontDestroyOnLoad(gameObject);

            GlobalSetting.Init();               //全局路径初始化

            AppConfig.Instance.Init();       //配置文件初始化

            InitAppData();                     //初始化用户配置信息

            KeyHelper.Active();                //激活

            MessageHandleCenter.Init();         //消息处理中心初始化

            ResourceConfigManager.Init();       //预制体配置初始化

            if (EnabledLog) LogManager.Init();  //日志初始化

            //win窗体定位初始化
            if (EnabledWindowMode)
            {
                ResolutionData.Instance.Init();
                WindowMode.Instance.SetWindowMode();

                //if (EnaledUpdate) WindowMode.Instance.SetWindowModeLoop();
                if (EnabledHideTaskbar) WindowMode.Instance.HideTaskBar();
            }

            //网络监听初始化
            if (EnabledServer)
            {
                //NotificationCenter.DefaultCenter().AddObserver(this, "UnityServer");
                if (MServerType == ServerType.Http)
                {
                    HttpListenerManager.Init();
                    HttpListenerManager.instance.OnCreate();
                }
                else if (MServerType == ServerType.Udp)
                {
                    UdpListenerManager.Init();
                    UdpListenerManager.instance.OnCreate();
                }
                else if (MServerType == ServerType.Scoket)
                {
                    SocketListerManager.Init();
                    SocketListerManager.Instance.OnCreate();
                }
            }

            PlusbeCommandCenter.Init();

            if (EnabledJsonConfig) JsonDataManager.Init();  //json数据初始化
            if (EnabledFileConfig) FileConfig.Init();

            //UIManager.Init();                   //UIManager启动

            AppTimeChecker.Instance.CheckKey();

           //WindownKey.Init();                  //按键初始化
        }

        protected void InitAppData()
        {
            //int quality = AppConfig.Instance.GetValueByKeyInt("QualityLevel");
            //quality = quality >= 0 && quality <= 5 ? quality : 2;
            //QualitySettings.SetQualityLevel(quality, true);
            if (Application.isEditor || AppConfig.Instance.GetValueByKeyInt("Debug") == 1)
                appRunMode = AppMode.Developing;

            if (appRunMode == AppMode.Developing)
            {
                GUIConsoleManager.Init();
            }

            EnabledKillProcess = AppConfig.Instance.GetValueByKeyBool("KillMySelf");

            EnabledHideTaskbar = AppConfig.Instance.GetValueByKeyBool("HideTaskbar");

        }

        

        #region 程序生命周期事件派发

        public static ApplicationVoidCallback s_OnApplicationQuit = null;
        public static ApplicationBoolCallback s_OnApplicationPause = null;
        public static ApplicationBoolCallback s_OnApplicationFocus = null;
        public static ApplicationVoidCallback s_OnApplicationUpdate = null;
        public static ApplicationVoidCallback s_OnApplicationFixedUpdate = null;
        public static ApplicationVoidCallback s_OnApplicationOnGUI = null;
        public static ApplicationVoidCallback s_OnApplicationOnDrawGizmos = null;
        public static ApplicationVoidCallback s_OnApplicationLateUpdate = null;

        void OnApplicationQuit()
        {
            if (EnabledHideTaskbar) WindowMode.Instance.ShowTaskbar();

            OnKillProcess();

            if (s_OnApplicationQuit != null)
            {
                try
                {
                    s_OnApplicationQuit();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        /*
         * 强制暂停时，先 OnApplicationPause，后 OnApplicationFocus
         * 重新“启动”游戏时，先OnApplicationFocus，后 OnApplicationPause
         */
        void OnApplicationPause(bool pauseStatus)
        {
            if (s_OnApplicationPause != null)
            {
                try
                {
                    s_OnApplicationPause(pauseStatus);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        void OnApplicationFocus(bool focusStatus)
        {
            if (s_OnApplicationFocus != null)
            {
                try
                {
                    s_OnApplicationFocus(focusStatus);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        void Update()
        {
            if (s_OnApplicationUpdate != null)
                s_OnApplicationUpdate();

            OnUpdateResourceGC();
        }

        private void LateUpdate()
        {
            if (s_OnApplicationLateUpdate != null)
            {
                s_OnApplicationLateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (s_OnApplicationFixedUpdate != null)
                s_OnApplicationFixedUpdate();
        }

        void OnGUI()
        {
            if (s_OnApplicationOnGUI != null)
                s_OnApplicationOnGUI();
        }

        private void OnDrawGizmos()
        {
            if (s_OnApplicationOnDrawGizmos != null)
                s_OnApplicationOnDrawGizmos();
        }

        #endregion

        #region 其他方法

        //强制杀掉本进程
        void OnKillProcess()
        {
            if (!Application.isEditor && EnabledKillProcess)
            {
                Debug.Log("即将杀掉本进程");
                Helper.ProcessHelper.KillMySelf();
            }
        }

        private float lastGCTime;

        //自动垃圾回收
        void OnUpdateResourceGC()
        {
            if (Time.time - lastGCTime > 60)
            {
                lastGCTime = Time.time;
                ResourceGCHelper.GC();
            }
        }

        #endregion
    }

}

public enum AppMode
{
    Developing,
    QA,
    Release
}

public enum ServerType
{
    None,
    Http,
    Udp,
    Scoket
}

public delegate void ApplicationBoolCallback(bool status);
public delegate void ApplicationVoidCallback();