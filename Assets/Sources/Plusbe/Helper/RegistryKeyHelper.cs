using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlusbeUnitySetting.Plusbe
{
    /// <summary>
    /// 启动项注册表操作
    /// </summary>
    public class RegistryKeyHelper
    {
        public static bool ExistKey(string key)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey run = registry.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (run.GetValue(key) != null)
            {
                registry.Close();
                return true;
            }

            registry.Close();
            return false;
        }

        public static bool ExistKey(string key, string value,out string nowValue)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey run = registry.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            nowValue = "";

            if (run.GetValue(key) != null)
            {
                nowValue = run.GetValue(key).ToString();
                if (nowValue == value)
                {
                    registry.Close();
                    return true;
                }

                return false;
            }

            registry.Close();
            return false;
        }

        public static string CheckKey(string key)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey run = registry.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            return run.GetValue(key) == null ? "": run.GetValue(key).ToString();
        }

        public static void DeleteKey(string key)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey run = registry.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",true);

            try
            {
                run.DeleteValue(key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                registry.Close();
            }

            registry.Close();
        }


        //计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run

        public static void SetKey(string key, string path)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey run = registry.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            run.SetValue(key, path);
            registry.Close();
        }
    }
}
