using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Plusbe.Http
{
    public class HttpUploadFileHelper
    {

        //string parentdate = "?userName=test&cardType=0&compName=向正&jobs=1&userNum=001&fingerCode=code1&fingerCode1=code2&fingerCode2=code3&userType=0&type=1&icCard=&idCard=4128251990";
        //string str = HttpUploadFile("http://jsgw.weixin.plusbe.com/FlashXml/ClientUser.aspx" + parentdate, @"C:\1.png");

        public static string serverIP = "http://facedemo.cloud.plusbe.com/";

        public static string HttpUploadFile(string path)
        {
            string url = serverIP + "PhotoUploader.aspx?type=insert&r=" + new Random().Next();
            string result = HttpUploadFile(url, path);
            return result;
        }

        public static string HttpUploadFace(string path, string dir, string id)
        {
            string url = serverIP + "PhotoUploader.aspx?type=insert&dir=" + dir + "&pid=" + id + "&r=" + new Random().Next();
            string result = HttpUploadFile(url, path);
            return result;
        }

        public static string HttpUploadFace(byte[] bytes, string dir, string id)
        {
            string url = serverIP + "PhotoUploader.aspx?type=insert&dir=" + dir + "&pid=" + id + "&r=" + new Random().Next();
            string result = HttpUploadFile(url, bytes);
            return result;
        }

        public static string HttpUploadFile(string url, byte[] bytes)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            //int pos = path.LastIndexOf("\\");
            //string fileName = path.Substring(pos + 1);
            //请求头部信息 
            //StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", DateTime.Now.ToString("yyyyMMddHHmmss") + ".png"));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
            //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //byte[] bArr = new byte[fs.Length];
            //fs.Read(bArr, 0, bArr.Length);
            //fs.Close();
            byte[] bArr = bytes;
            Stream postStream;
            try
            {
                postStream = request.GetRequestStream();
            }
            catch
            {
                string err = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UserConfig><Msg>远程服务器没有响应，请检查网络状态或联系管理员!</Msg></UserConfig>";
                return err;
            }

            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

        /// <summary>
        /// Http上传文件
        /// </summary>
        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);
            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();
            Stream postStream;
            try
            {
                postStream = request.GetRequestStream();
            }
            catch 
                //(WebException webErr)
            {
                string err = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UserConfig><Msg>远程服务器没有响应，请检查网络状态或联系管理员!</Msg></UserConfig>";
                return err;
            }

            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
    }
}
