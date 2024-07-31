using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Plusbe.Http
{
    public class HttpHelper
    {
        public static string HtmlCode(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            try
            {
                //创建一个请求
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.KeepAlive = false;
                webReq.Method = "GET";
                webReq.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
                webReq.ServicePoint.Expect100Continue = false;
                webReq.Timeout = 5000;
                webReq.AllowAutoRedirect = true;//是否允许302
                ServicePointManager.DefaultConnectionLimit = 20;
                //获取响应
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                ////使用GB2312的编码方式 获取响应的文本流             
                //StreamReader sReader = new StreamReader(webRes.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                string content = string.Empty;
                using (System.IO.Stream stream = webRes.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                webReq.Abort();
                return content;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void OpenUrl(string ip, string port, string act, string obj, string sta)
        { 
            string url = "http://"+ip+":"+port+"/?act="+act+"&object="+obj+"&states="+sta+"&r="+new Random().Next(1000,9999);
            HtmlCodeThread(url);
        }

        /// <summary>
        /// 发送数据 不处理返回值
        /// </summary>
        /// <param name="url"></param>
        public static void HtmlCodeThread(string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(runWork),url);
        }

        //public static void HtmlCodeThread(string url)
        //{
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(runWork), url);
        //}

        private static void runWork(object url)
        {
            HtmlCode(url.ToString());
        }

        ///// <summary>
        ///// 中文转码 防止乱码
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static string UTF(string str, string name)
        //{
        //    string filename = Path.GetFileName(str);
        //    string xmlcontent = HttpUtility.ParseQueryString(filename).Get(name);
        //    return xmlcontent;
        //}
    }
}
