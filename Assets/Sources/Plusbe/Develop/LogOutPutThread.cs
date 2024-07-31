using Plusbe.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Develop
{
    public class LogOutPutThread
    {
        private string logPath;
        private StreamWriter logWriter = null;

        public void Init()
        {
            try
            {
                ApplicationManager.s_OnApplicationQuit += OnAppQuit;

                logPath = GlobalSetting.DataPath + "Log/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToString("HH-mm-ss") + ".txt";
                if (File.Exists(logPath))
                    File.Delete(logPath);

                string logDir = Path.GetDirectoryName(logPath);
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                logWriter = new StreamWriter(logPath);
                logWriter.AutoFlush = true;
            }catch(Exception ex)
            {
                Debug.LogError("LogOutPutThread Init Exception:" + ex.ToString());
            }
            
        }

        public void Log(string log, string track, LogType type)
        {
            this.logWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log);
        }

        public void OnAppQuit()
        {
            LogManager.OnAppQuit();
            logWriter.Close();
        }
    }
}
