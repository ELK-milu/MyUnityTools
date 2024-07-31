using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Develop
{
    public class LogManager
    {
        private static LogOutPutThread logOutPutThread = new LogOutPutThread();

        public static void Init(bool openLog = true)
        {
            if (openLog)
            {
                logOutPutThread.Init();
                Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
                //Application.logMessageReceived += Application_logMessageReceived;
            }
        }

        //static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        //{
        //    throw new NotImplementedException();
        //}

        static void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            logOutPutThread.Log(condition, stackTrace, type);
        }

        public static void OnAppQuit()
        {
            Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
        }
    }
}
