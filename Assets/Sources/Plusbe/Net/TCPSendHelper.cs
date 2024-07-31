using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plusbe.Net
{
    public class TCPSendHelper
    {

        public static string serverip = "127.0.0.1";
        public static int serverport = 6002;

        private static void SendToPad(int index)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ToSendData), index);
        }

        public static void SendData(byte[] bytes)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();

                tcpClient.Connect(IPAddress.Parse(serverip), serverport);

                NetworkStream ntwStream = tcpClient.GetStream();

                //bytes = Encoding.UTF8.GetBytes("asdasd");

                ntwStream.Write(bytes, 0, bytes.Length);

                //Thread.Sleep(200);

                ntwStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Debug2.Log("TCPSendHelper error :" + ex.ToString());
            }
        }
    }
}
