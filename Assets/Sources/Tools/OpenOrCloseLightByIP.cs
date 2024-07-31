/****************************************************
    文件：OpenOrCloseLightByIP.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2020/10/14 18:6:48
	功能：通过继电器中控服务器IP和端口号控制继电器开关灯光
    调用方法： LightHelper.OpenLight(6); 打开继电器第6路
*****************************************************/

using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
public class OpenOrCloseLightByIP
{
    private static string ip = "10.20.68.81";
    //private static string ip = "192.168.40.56";
    private static string port = "6300";
    private static int deviceID = 1;
    //public static void Init(string ip, string port)
    //{
    //    LightHelper.ip = ip;
    //    LightHelper.port = port;
    //}
    //http://127.0.0.1:8020/?act=unity&object=0-5&states=30&r=123123
    //http://127.0.0.1:8020/?act=unity&object=-1&states=31
    private static TcpClient tcp;
    public static void Init()
    {
        if (tcp == null)
        {
            tcp = new TcpClient(ip, int.Parse(port));
            tcp.SendTimeout = 2;
            tcp.ReceiveTimeout = 2;
        }
    }
    public static void OpenLight(int lightID)
    {
        //Init();
        OpenLight(ip, Convert.ToInt32(port), lightID, deviceID);
        Debug.Log("开灯：" + lightID);
    }
    public static void CloseLight(int lightID)
    {
        //Init();
        CloseLight(ip, Convert.ToInt32(port), lightID, deviceID);
        Debug.Log("关灯：" + lightID);
    }
    private static void OpenLight(string ip, int port, int lightID, int deviceID)
    {
        try
        {
            if (lightID != -1)
            {
                byte[] data = new byte[]
                {
                        85,
                        1,
                        18,
                        0,
                        0,
                        0,
                        1,
                        105
                };
                data[1] = Convert.ToByte(deviceID);
                data[6] = Convert.ToByte((int)data[6] + lightID);
                data[7] = Convert.ToByte((int)data[7] + lightID + deviceID - 1);
                LightSend(data, ip, port);
            }
            else
            {
                byte[] data = new byte[]
                {
                        85,
                        1,
                        19,
                        0,
                        0,
                        255,
                        255,
                        103
                };
                data[1] = Convert.ToByte(deviceID);
                data[7] = Convert.ToByte((int)data[7] + deviceID - 1);
                LightSend(data, ip, port);
            }
        }
        catch
        {
        }
    }
    private static void CloseLight(string ip, int port, int lightID, int deviceID)
    {
        if (lightID != -1)
        {
            byte[] data = new byte[]
            {
                    85,
                    1,
                    17,
                    0,
                    0,
                    0,
                    1,
                    104
            };
            data[1] = Convert.ToByte(deviceID);
            data[6] = Convert.ToByte((int)data[6] + lightID);
            data[7] = Convert.ToByte((int)data[7] + lightID + deviceID - 1);
            LightSend(data, ip, port);
        }
        else
        {
            byte[] data = new byte[]
            {
                    85,
                    1,
                    19,
                    0,
                    0,
                    0,
                    0,
                    105
            };
            data[1] = Convert.ToByte(deviceID);
            data[7] = Convert.ToByte((int)data[7] + deviceID - 1);
            LightSend(data, ip, port);
        }
    }
    private static void LightSend(byte[] data, string ip, int port)
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback(Send), new LightData1() { bytes = data, ip = ip, port = port });
        //Send(data, ip, port);
    }

    public static void Send(object obj)
    {
        LightData1 data = obj as LightData1;
        Send(data.bytes, data.ip, data.port);
    }

    public static void Send(byte[] btSend, string host, int port)
    {
        try
        {
            TcpClient tcp = new TcpClient(host, port);
            tcp.SendTimeout = 2;
            tcp.ReceiveTimeout = 2;
            NetworkStream ns = tcp.GetStream();
            ns.Write(btSend, 0, btSend.Length);
            tcp.Close();
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);
            //socket.Connect(host, port);
            //socket.Send(btSend);
            //socket.Close();
        }
        catch
        {
        }
    }
}
class LightData1
{
    public byte[] bytes;
    public string ip;
    public int port;
}