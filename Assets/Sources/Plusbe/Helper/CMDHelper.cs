using Plusbe.Config;
using Plusbe.Core;
using Plusbe.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Plusbe.Helper
{

    public class CMDHelper
    {
        public static void Init()
        {
            GlobalSetting.ToIP = AppConfig.Instance.GetValueByKey("ToIP");
            GlobalSetting.ToPort = AppConfig.Instance.GetValueByKey("ToPort");

            GlobalSetting.ServerIP = AppConfig.Instance.GetValueByKey("ServerIP");
        }

        public static void GoChannel(string id)
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", id, "27");
        }

        public static void GoTag(string id)
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", id, "28");
        }

        public static void GoFile(string id)
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", id, "13");
        }


        public static void GoPlay()
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", "", "1");
        }

        public static void GoPause()
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", "", "2");
        }

        public static void GoReplay()
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", "", "3");
        }

        public static void GoPrev()
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", "", "9");
        }

        public static void GoNext()
        {
            OpenUrl(GlobalSetting.ToIP, GlobalSetting.ToPort, "movie", "", "10");
        }

        public static string GetListUrl()
        {
            return "http://" + GlobalSetting.ToIP + ":" + GlobalSetting.ToPort + "/?act=" + "movie" + "&object=" + "" + "&states=" + 14 + "&r=" + new Random().Next(1000, 9999);
        }

        public static void SetVolumn(int num)
        {
            OpenUrl(GlobalSetting.ToIP, "8088", "setvolume", "", "" + num);
        }

        public static void SetLight(int num)
        {
            OpenUrl("127.0.0.1", "8088", "com", "", "" + num);
        }

        public static void OpenUrl(string ip, string port, string act, string obj, string sta)
        {
            //ApplicationManager.UpdateScreenTime();

            string url = "http://" + ip + ":" + port + "/?act=" + act + "&object=" + obj + "&states=" + sta + "&r=" + new Random().Next(1000, 9999);
            HtmlCodeThread(url);
        }

        public static void OpenUrl(string ipport, string act, string obj, string sta)
        {
            //ApplicationManager.UpdateScreenTime();

            string url = "http://" + ipport + "/?act=" + act + "&object=" + obj + "&states=" + sta + "&r=" + new Random().Next(1000, 9999);
            HtmlCodeThread(url);
        }

        /// <summary>
        /// 发送数据 不处理返回值
        /// </summary>
        /// <param name="url"></param>
        public static void HtmlCodeThread(string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(runWork), url);
        }

        private static void runWork(object url)
        {
            HttpHelper.HtmlCode(url.ToString());
        }
    }
}


