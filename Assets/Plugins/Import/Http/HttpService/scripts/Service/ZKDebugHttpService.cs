using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{

    public class ZKDebugHttpService : IHttpService
    {
        public int ServicePort { get; set; }
        public string ServiceName { get { return "标准中控命令网页调试服务"; } }

        string templateHtml;
        
        public ZKDebugHttpService(int port)
        {
            ServicePort = port;
            templateHtml = Resources.Load<TextAsset>("template").text;
        }

        public string HandleHttpCommand(string rawUrl)
        {
            string address = GetLocalIP() + ":" + HttpServiceManager.StandardService.ServicePort;
            if(templateHtml.Contains("++ADDRESSHERE++"))
            {
                templateHtml = templateHtml.Replace("++ADDRESSHERE++", address);
            }
            return templateHtml;
        }

        string GetLocalIP()
        {
            return GetLocalIpAddress("InterNetwork")[0];
        }

        /// <summary>
        /// 获取本机所有ip地址
        /// </summary>
        /// <param name="netType">"InterNetwork":ipv4地址，"InterNetworkV6":ipv6地址</param>
        /// <returns>ip地址集合</returns>
        List<string> GetLocalIpAddress(string netType)
        {
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName()); //解析主机IP地址 

            List<string> IPList = new List<string>();
            if (netType == string.Empty)
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    IPList.Add(addresses[i].ToString());
                }
            }
            else
            {
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                for (int i = 0; i < addresses.Length; i++)
                {
                    if (addresses[i].AddressFamily.ToString() == netType)
                    {
                        IPList.Add(addresses[i].ToString());
                    }
                }
            }
            return IPList;
        }
    }
}

