/*
*┌────────────────────────────────────────────────┐
*│　描    述：LightHeloer                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0                                              
*│　创建时间：2019/11/11 11:18:54                        
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Plusbe.Helper
{
    public class LightHelper
    {

        #region 自定义
        private const int SPACE_TIME = 120; // 组播每个之间间隔毫秒

        #endregion

        #region 开灯

        public static void OpenLight(LightData lightData)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                if (lightData.sendBytes != null)
                {
                    LightSend(lightData.sendBytes, lightData.ip, lightData.port, lightData.com);
                }
                else
                {
                    OpenLightAction(lightData.ip, lightData.port, lightData.lightID, lightData.deviceID,lightData.com);
                }
            }));
        }

        public static void OpenLight(string ip, int port, int lightID,int deviceID=1,string com="")
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                OpenLightAction(ip, port, lightID, deviceID, com);
            }));
        }

        public static void OpenLightMore(LightData[] lightDatas)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                for (int i = 0; i < lightDatas.Length; i++)
                {
                    if (lightDatas[i].sendBytes != null)
                    {
                        LightSend(lightDatas[i].sendBytes, lightDatas[i].ip, lightDatas[i].port, lightDatas[i].com);
                    }
                    else
                    {
                        OpenLightAction(lightDatas[i].ip, lightDatas[i].port, lightDatas[i].lightID, lightDatas[i].deviceID, lightDatas[i].com);
                    }

                    Thread.Sleep(SPACE_TIME);
                }
            }));
        }

        #endregion

        #region 关灯

        public static void CloseLight(LightData lightData)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                if (lightData.sendBytes != null)
                {
                    LightSend(lightData.sendBytes, lightData.ip, lightData.port,lightData.com);
                }
                else
                {
                    CloseLightAction(lightData.ip, lightData.port, lightData.lightID, lightData.deviceID,lightData.com);
                }
            }));
        }

        public static void CloseLight(string ip, int port, int lightID, int deviceID = 1,string com="")
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                CloseLightAction(ip, port, lightID, deviceID,com);
            }));
        }

        public static void CloseLightMore(LightData[] lightDatas)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                for (int i = 0; i < lightDatas.Length; i++)
                {
                    if (lightDatas[i].sendBytes != null)
                    {
                        LightSend(lightDatas[i].sendBytes, lightDatas[i].ip, lightDatas[i].port,lightDatas[i].com);
                    }
                    else
                    {
                        CloseLightAction(lightDatas[i].ip, lightDatas[i].port, lightDatas[i].lightID, lightDatas[i].deviceID, lightDatas[i].com);
                    }

                    Thread.Sleep(SPACE_TIME);
                }
            }));
        }

        #endregion

        #region 指令转化与发送


        public static void ThreadSendLight(string ip, int port, byte[] bytes, string com = "")
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                LightSend(bytes, ip, port, com);
            }));
        }

        private static void OpenLightAction(string ip, int port, int lightID, int deviceID,string com="")
        {
            try
            {
                //Debug.Log("开灯：" + lightID);
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
                    LightSend(data, ip, port, com);
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
                    LightSend(data, ip, port, com);
                }
            }
            catch
            {
            }
        }

        private static void CloseLightAction(string ip, int port, int lightID, int deviceID,string com = "")
        {
            try
            {
                // Debug.Log("关灯：" + lightID);
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

                    LightSend(data, ip, port, com);
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

                    LightSend(data, ip, port, com);
                }
            }
            catch { }
        }

        private static void LightSend(byte[] bytes, string host, int port, string com)
        {
            if (string.IsNullOrEmpty(com))
            {
                LightSend(bytes, com);
            }
            else
            {
                LightSend(bytes, host, port);
            }
        }

        private static void LightSend(byte[] bytes, string host, int port)
        {
            try
            {
                TcpClient tcp = new TcpClient(host, port);
                tcp.SendTimeout = 2;
                tcp.ReceiveTimeout = 2;
                NetworkStream ns = tcp.GetStream();
                ns.Write(bytes, 0, bytes.Length);
                tcp.Close();
            }
            catch
            {
            }
        }

        private static void LightSend(byte[] bytes, string comPort)
        {
            try
            {
                SerialPort serialPort1 = new SerialPort(comPort, 9600);
                serialPort1.DtrEnable = true;
                serialPort1.Open();
                serialPort1.Write(bytes, 0, bytes.Length);
                serialPort1.Close();
            }
            catch
            {
            }
        }

        #endregion
    }

    public class LightData
    {
        public int sendType;

        public string id;

        public string ip;
        public int port;
        public int lightID;
        public int deviceID;

        public string com;

        public byte[] sendBytes = null;
    }
}
