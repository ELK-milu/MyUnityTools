using AppCustom;
using Assets.Scripts.Plusbe;
using Plusbe.Config;
using Plusbe.Message;
using Plusbe.Serialization;
using Plusbe.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Plusbe.Net
{
    public class HttpListenerManager : MonoBehaviour
    {
        public static HttpListenerManager instance;

        private HttpListener httpListener;
        private static int port = 8020;

        public static void Init()
        {
            Init(AppConfig.Instance.GetValueByKeyInt("Port"));
        }

        public static void Init(int mport)
        {
            port = mport;
            instance = ApplicationManager.Instance.gameObject.AddComponent<HttpListenerManager>();

            ApplicationManager.s_OnApplicationQuit += instance.OnAppQuit;

            //onCreate();
        }

        private void OnAppQuit()
        {
            if (Application.isEditor)
            {
                OnClose();
            }
        }

        public void OnClose()
        {
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
            }
        }

        public void OnCreate()
        {
            OnClose();
            try
            {
                httpListener = new HttpListener();
                httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                httpListener.Prefixes.Add("http://+:" + port + "/");
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), httpListener);
            }
            catch (HttpListenerException e)
            {
                Debug.LogWarning("监听失败,检查端口是否被占用！" + e.ToString());
            }
            catch (Exception e)
            {
                Debug.LogWarning("监听失败,未知错误！" + e.ToString());
            }
        }

        private void GetContextCallBack(IAsyncResult ar)
        {
            HttpListener listener = ar.AsyncState as HttpListener;
            try
            {
                string result = "true";
                try
                {
                    if (!listener.IsListening)
                    {
                        return;
                    }
                    HttpListenerContext ctx = listener.EndGetContext(ar);
                    ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                    result = MessageHandleCenter.HandleHttpMessage(ctx.Request.RawUrl);
                    using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, Encoding.GetEncoding("utf-8")))//转码
                    {
                        writer.WriteLine(result.ToString());
                    }
                }
                catch (ObjectDisposedException nullEx)
                {
                    Debug.Log("ObjectDisposedException:" + nullEx.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("http 1-监听处理失败" + ex.ToString());
                }
                finally
                {
                    if (listener.IsListening)
                    {
                        listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
                    }
                }
            }
            catch (ObjectDisposedException nullEx)
            {
                Debug.Log("ObjectDisposedException:" + nullEx.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogWarning("http 2-监听处理失败" + ex.ToString());
            }
        }
    }
}
