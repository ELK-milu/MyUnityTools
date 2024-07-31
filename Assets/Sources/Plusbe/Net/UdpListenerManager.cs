using AppCustom;
using Plusbe.Config;
using Plusbe.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Plusbe.Net
{
    public class UdpListenerManager:MonoBehaviour
    {
        public static UdpListenerManager instance;

        private Thread receiveThread;
        private UdpClient client;
        private static int port = 8888; // DEFAULT UDP PORT !!!!! THE QUAKE ONE ;)  
        public string strReceiveUDP = "";
        public string LocalIP = String.Empty;
        private string hostname;

        public static void Init()
        {
            Init(AppConfig.Instance.GetValueByKeyInt("Port"));
        }

        public static void Init(int mport)
        {
            port = mport;

            instance = ApplicationManager.Instance.gameObject.AddComponent<UdpListenerManager>();

            ApplicationManager.s_OnApplicationQuit += instance.OnAppQuit;
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
            try
            {
                if (client != null)
                {
                    this.receiveThread.Abort();
                    this.client.Close();
                    ((IDisposable)this.client).Dispose();
                    this.client = null;
                }
            }
            catch { }
            
        }

        public void OnCreate()
        {
            OnClose();

            receiveThread = new Thread(new ThreadStart(receiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            hostname = Dns.GetHostName();
            IPAddress[] ips = Dns.GetHostAddresses(hostname);
            if (ips.Length > 0)
            {
                LocalIP = ips[0].ToString();
            }
        }

        private void receiveData()
        {
            try
            {
                client = new UdpClient(port);
            }
            catch (Exception ex)
            {
                Debug.Log("udp 端口监听失败1:" + port + ex.ToString());
                return;
            }

            while (true)
            {
                try
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, port);
                    byte[] data = new byte[1024];
                    data = client.Receive(ref anyIP);
                    strReceiveUDP = System.Text.Encoding.UTF8.GetString(data);
                    if (!string.IsNullOrEmpty(strReceiveUDP))
                    {
                        MessageHandleCenter.HandleUdpMessage(strReceiveUDP);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("udp 端口监听处理失败2:" + ex.ToString());
                }
            }
        }
    }
}
