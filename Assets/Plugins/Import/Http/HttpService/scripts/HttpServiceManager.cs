using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PlusbeQuickPlugin.HttpService
{
    public class HttpServiceManager
    {
        private static GameObject httpObj;
        public static GameObject HttpObject
        {
            get
            {
                if (httpObj == null)
                {
                    httpObj = new GameObject("HttpService");
                    UnityEngine.Object.DontDestroyOnLoad(HttpObject);
                }
                return httpObj;
            }
        }
        public static StandardHttpService StandardService;

        static Dictionary<string, IHttpService> dict = new Dictionary<string, IHttpService>();

        /// <summary>
        /// 创建默认http服务
        /// </summary>
        /// <param name="port"></param>
        public static void CreateStandardHttpService(int port)
        {
            var listener=HttpObject.AddComponent<HttpListenerBehaviour>();
            if (StandardService == null) StandardService = new StandardHttpService(port);
            listener.StartService(StandardService);
        }

        public static void CreateWebDebugHttpService(int port)
        {
            CreateHttpService(new ZKDebugHttpService(port));
        }

        /// <summary>
        /// 创建自定义http服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="port"></param>
        /// <param name="service"></param>
        public static void CreateHttpService(IHttpService service)
        {
            string serviceName = service.ServiceName;
            if (dict.TryGetValue(serviceName,out IHttpService ser)==false)
            {
                dict[serviceName] = service;
                var listener = HttpObject.AddComponent<HttpListenerBehaviour>();
                listener.StartService(service);
            }
            else
            {
                Debug.LogError("已经有同名服务正在运行");
            }
        }
        /// <summary>
        /// 获取http服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static IHttpService GetHttpService(string serviceName)
        {
            dict.TryGetValue(serviceName, out IHttpService service);
            return service;
        }
        
    }

}


