using AppCustom;
using Plusbe.Config;
using Plusbe.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Plusbe.Net
{
    //https://blog.csdn.net/NippyLi/article/details/79123521
    public class SocketListerManager : MonoBehaviour
    {
        //以下默认都是私有的成员
        Socket serverSocket; //服务器端socket
        Socket clientSocket; //客户端socket
        IPEndPoint ipEnd; //侦听端口
        string recvStr; //接收的字符串
        string sendStr; //发送的字符串
        byte[] recvData = new byte[1024]; //接收的数据，必须为字节
        byte[] sendData = new byte[1024]; //发送的数据，必须为字节
        int recvLen; //接收的数据长度
        Thread connectThread; //连接线程


        private static SocketListerManager instance;

        private static int port = 8030;

        public static SocketListerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ApplicationManager.Instance.gameObject.AddComponent<SocketListerManager>();
                }

                return instance;
            }
        }

        public static void Init()
        {
            Init(AppConfig.Instance.GetValueByKeyInt("Port"));
        }

        public static void Init(int mport)
        {
            port = mport;
            instance = ApplicationManager.Instance.gameObject.AddComponent<SocketListerManager>();

            ApplicationManager.s_OnApplicationQuit += instance.OnAppQuit;
        }

        public void OnCreate()
        {
            InitSocket(port);
        }

        public void InitSocket(int port)
        {

            //定义侦听端口,侦听任何IP
            ipEnd = new IPEndPoint(IPAddress.Any, port);
            //定义套接字类型,在主线程中定义
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接
            serverSocket.Bind(ipEnd);
            //开始侦听,最大10个连接
            serverSocket.Listen(10);

            //开启一个线程连接，必须的，否则主线程卡死
            connectThread = new Thread(new ThreadStart(SocketReceive));
            connectThread.Start();
        }

        //连接
        void SocketConnet()
        {
            if (clientSocket != null)
                clientSocket.Close();
            //控制台输出侦听状态
            print("Waiting for a client");
            //一旦接受连接，创建一个客户端
            clientSocket = serverSocket.Accept();
            //获取客户端的IP和端口
            IPEndPoint ipEndClient = (IPEndPoint)clientSocket.RemoteEndPoint;
            //输出客户端的IP和端口
            print("Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString());
            //连接成功则发送数据
            sendStr = "Welcome to my server";
            SocketSend(sendStr);
        }

        void SocketSend(string sendStr)
        {
            //清空发送缓存
            sendData = new byte[1024];
            //数据类型转换
            sendData = Encoding.UTF8.GetBytes(sendStr);
            //发送
            clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
        }

        //服务器接收
        void SocketReceive()
        {
            //连接
            SocketConnet();
            //进入接收循环
            while (true)
            {
                //对data清零
                recvData = new byte[1024];
                //获取收到的数据的长度
                recvLen = clientSocket.Receive(recvData);
                //如果收到的数据长度为0，则重连并进入下一个循环
                if (recvLen == 0)
                {
                    SocketConnet();
                    continue;
                }
                //输出接收到的数据
                recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                print(recvStr);

                //NotificationCenter.Instance.PostNotification(null, "UnityTCPCommand", recvStr);

                MessageHandleCenter.HandlerSocketTCP(recvStr);

                //将接收到的数据经过处理再发送出去
                sendStr = "From Server: " + recvStr;
                SocketSend(sendStr);
            }
        }

        //连接关闭
        void SocketQuit()
        {
            //先关闭客户端
            if (clientSocket != null)
                clientSocket.Close();
            //再关闭线程
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }
            //最后关闭服务器
            serverSocket.Close();
            print("diconnect");
        }


        void OnAppQuit()
        {
            SocketQuit();
        }
    }
}