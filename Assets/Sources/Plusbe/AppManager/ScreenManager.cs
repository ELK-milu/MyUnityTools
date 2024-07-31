using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plusbe.AppManager
{

    public class ScreenManager
    {

        //private static ScreenManager instance;
        private static float lastGameTime;
        private static int maxTime = 3 * 3;

        private static Action screenBack;

        public static void Init(Action back)
        {
            screenBack = back;
            Init();
        }

        public static void Init()
        {
            //instance = ApplicationManager.Instance.gameObject.AddComponent<ScreenManager>();
            ApplicationManager.s_OnApplicationUpdate += OnUpdate;
        }

        public static void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateGameTime();
            }

            if (Time.time - lastGameTime > maxTime)
            {
                //返回屏保
                lastGameTime = int.MaxValue;
                if (screenBack != null) screenBack();
                Debug.Log("Auto Back Screen");
            }
        }

        /// <summary>
        /// 不可非主线程调用。。
        /// 1.按键事件触发
        /// 2.外部命令调用触发
        /// </summary>
        public static void UpdateGameTime()
        {
            lastGameTime = Time.time;
        }
    }
}