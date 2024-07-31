/****************************************************
    文件：OpenOrCloseLightByIP.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2020/10/14 18:6:48
	功能：发送UDP 16进制数据
    调用案例： 
        data = new byte[] { 0x14, 0x01, 0x11, 0x0a, 0x00, 0x00, 0x00, 0x30, 0x0d, 0x0a };
        UDPSendHelper.SendTest(data);
*****************************************************/
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Plusbe.Net
{
    public class UDPSendHelper
    {

        public static string serverip = "127.0.0.1";
        public static int serverport = 6001;


        public static void SendTest()
        {
            try
            {
                UdpClient udpclient = new UdpClient();
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8030);  //修改此处为目标主机IP及端口
                byte[] data = new byte[] { 0x45, 0x4A, 0x4B, 0x9A };

                udpclient.Send(data, data.Length, ipendpoint);
                udpclient.Close();

                //Console.WriteLine("{0:HH:mm:ss}->发送数据(to {1})：{2}", DateTime.Now, serverip, serverport);
            }
            catch (Exception)
            {
                //Console.WriteLine("{0:HH:mm:ss}->{1}", DateTime.Now, ex.Message);
            }
        }

        public static void SendMsg(string msg)
        {
            SendBytes(Encoding.UTF8.GetBytes(msg));
        }

        public static void SendByte(byte sendByte)
        {
            try
            {
                UdpClient udpclient = new UdpClient();
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(serverip), serverport);
                byte[] data = new byte[1];
                data[0] = sendByte;
                udpclient.Send(data, data.Length, ipendpoint);
                udpclient.Close();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("send byte error :" + ex.ToString());
            }
        }

        public static void SendBytes(byte[] sendBytes)
        {
            try
            {
                UdpClient udpclient = new UdpClient();
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(serverip), serverport);
                udpclient.Send(sendBytes, sendBytes.Length, ipendpoint);
                udpclient.Close();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("send byte error :" + ex.ToString());
            }
        }
    }
}
