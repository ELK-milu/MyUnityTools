using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plusbe.Helper
{
    public class ProcessHelper
    {
        public static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcesses();
            try
            {
                foreach (Process item in processes)
                {
                    if (item.ProcessName == processName)
                    {
                        UnityEngine.Debug.Log("kill process :" + processName);
                        item.Kill();
                    }
                }
            }
            catch { }
        }

        public static void KillMySelf()
        {
            if(!UnityEngine.Application.isEditor)
                Process.GetCurrentProcess().Kill();

        }
    }
}
