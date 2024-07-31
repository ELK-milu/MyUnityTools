using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{
    public class HttpListenerBehaviour : MonoBehaviour
    {
        HttpListener httpListener;
        IHttpService service;

        public void StartService(IHttpService handler)
        {
            Loom.Initialize();
            this.service = handler;
            try
            {
                httpListener = new HttpListener();
                httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                httpListener.Prefixes.Add("http://+:" + handler.ServicePort + "/");
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), this);
                print("Http监听初始化完成"+handler.ServiceName+" port: "+handler.ServicePort);
            }
            catch (Exception e)
            {
                Debug.Log("监听建立失败" + e.Message);
            }
        }

        private void OnApplicationQuit()
        {
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
            }
        }

        private void GetContextCallBack(IAsyncResult ar)
        {
            HttpListenerBehaviour listenerBehaviour = ar.AsyncState as HttpListenerBehaviour;
            HttpListener listener = listenerBehaviour.httpListener;
            IHttpService service = listenerBehaviour.service;
            string result = "true";
            try
            {
                if (!listener.IsListening)
                {
                    return;
                }
                HttpListenerContext context = listener.EndGetContext(ar);
                context.Response.StatusCode = 200;//设置状态返回码
                context.Response.ContentType = "text/html; Charset=UTF-8";//设置返回类型
                context.Response.AppendHeader("Access-Control-Allow-Origin", "*");//设置跨域访问

                result = service.HandleHttpCommand(context.Request.RawUrl);

                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.GetEncoding("utf-8")))//转码
                {
                    writer.WriteLine(result.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Log("http监听处理失败" + ex.Message);
            }
            finally
            {
                if (listener.IsListening)
                {
                    listener.BeginGetContext(new AsyncCallback(GetContextCallBack), this);
                }
            }
        }



    }

}


