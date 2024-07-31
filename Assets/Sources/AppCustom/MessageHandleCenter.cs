using Plusbe.Message;
using Plusbe.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AppCustom
{
    /// <summary>
    /// 根据不同的项目进行定制化
    /// </summary>
    public class MessageHandleCenter
    {
        private static string paramAct = "act";
        private static string paramObject = "object";
        private static string paramStatus = "states";

        /// <summary>
        /// 避免无注册从非主线程进行逻辑处理
        /// </summary>
        public static void Init()
        {
            NotificationCenter.Instance.Init();
        }

        /// <summary>
        /// 数据转发
        /// 有返回值的要在当前位置处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HandleHttpMessage(string data)
        {
			
            //Debug.Log("HTTP:"+data+","+WWW.UnEscapeURL(data,Encoding.UTF8));
            data = UnityWebRequest.UnEscapeURL(data, Encoding.UTF8);  // 转码
            Debug.Log("HTTP:" + data);

            //NotificationCenter.DefaultCenter().PostNotification(null, "TestHttp", data);

            //ScreenManager.UpdateGameTime();
            string result = "true";
            NameValueCollection datas = MessageSerializerHelper.ParseHttpUrl(data);
            if (datas[paramAct] == "hello")
            {
                if (datas[paramStatus] == "1")
                {
                    result = "hello world ," + datas[paramObject];
                    return result;
                }
                else
                {
                    NotificationCenter.Instance.PostNotification(null, "TestHttpHello", data);
                    return result;
                }
            }

            #region 需要数据返回的请及时处理
            if (datas[paramAct] == "welcome")
            {
                if (datas["t"] == "getxml" || datas["t"] == "getjson")
                {
                    //欢迎词列表返回
                    return Plusbe.Welcome.WelcomeXml.Instance.GetJson();
                }
                NotificationCenter.Instance.PostNotification(null, "UnityWelcome", data);
            }


            if (datas[paramAct] == "unity" && datas["states"] == "14")
            {
                //中控配置列表返回
                StandardReturnJsonInformation.Instance.VerifyBackMessage(datas[paramAct], datas[paramObject], datas[paramStatus], out result);
                return result;
            }

            #endregion

            NotificationCenter.Instance.PostNotification(null, "UnityCommand", data);

            return result;
        }

        public static string HandleUdpMessage(string data)
        {
            Debug.Log("UDP:" + data);

            NotificationCenter.Instance.PostNotification(null, "UnityTCPCommand", data);
            return "true";
        }

        public static string HandlerSocketTCP(string data)
        {
            Debug.Log("TCP:" + data);

            NotificationCenter.Instance.PostNotification(null, "UnityTCPCommand", data);
            return "true";
        }
    }
}
