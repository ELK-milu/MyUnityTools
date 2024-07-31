/*
*┌────────────────────────────────────────────────┐
*│　描    述：FileUploadHelper                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0                                              
*│　创建时间：2019/8/19 14:12:10                        
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace TestUploadFile
{
    public class FileUploadHelper
    {
        private static string serverIP = "http://127.0.0.1:101/";
        public static string productName = "plusbe";
        private static string urlExist = "PhotoUploaderNew.aspx?type=exist&product={0}&filename={1}&r={2}";
        private static string urlUpload = "PhotoUploaderNew.aspx?type=insert&product={0}&filename={1}&r={2}";

        private const int byteCount = 128 * 1024;

        public static void Init(string ip, string product)
        {
            serverIP = ip;
            productName = product;
        }

        #region 上传接口
        public static void UploadFile(string path)
        {
            UploadFile(path,null);
        }

        public static void UploadFile(string path, Action<string, int, long, long> action)
        {
            UploadFile(productName, path, action);
        }

        public static void UploadFile(string product,string path,Action<string, int, long, long> action)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(HttpUploadFiles), new UploadData() { product = product, path = path, action = action });

            

        }
        #endregion

        class UploadData
        {
            public string product;
            public string path;
            public Action<string, int, long, long> action;
        }

        private static void HttpUploadFiles(object obj)
        {
            UploadData data = obj as UploadData;

            string product = data.product;
            string path = data.path;
            Action<string, int, long, long> action = data.action;

            int len = GetFileLenth(product, Path.GetFileName(path));
            action?.Invoke(path, len, 0, 0);

            if (len != -1)
                HttpUploadFiles(product, path, len, GetUploadUrl(product, Path.GetFileName(path)), action);
        }

        private static void HttpUploadFiles(string product, string path,int startPoint,string url,Action<string, int, long, long> action)
        {
            WebClient WebClientObj = new WebClient();
            FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            BinaryReader bReader = new BinaryReader(fStream);
            long length = fStream.Length;
            string sMsg = "上传成功";
            string fileName = Path.GetFileName(path);
            try
            {

                #region 续传处理
                byte[] data;
                if (startPoint > 0)
                {
                    fStream.Seek(startPoint, SeekOrigin.Current);
                }
                #endregion

                #region 分割文件上传
                for (; startPoint <= length; startPoint = startPoint + byteCount)
                {
                    if (startPoint + byteCount > length)
                    {
                        data = new byte[Convert.ToInt64((length - startPoint))];
                        bReader.Read(data, 0, Convert.ToInt32((length - startPoint)));
                    }
                    else
                    {
                        data = new byte[byteCount];
                        bReader.Read(data, 0, byteCount);
                    }

                    try
                    {
                        //***                        bytes 21010-47021/47022
                        WebClientObj.Headers.Remove(HttpRequestHeader.ContentRange);
                        WebClientObj.Headers.Add(HttpRequestHeader.ContentRange, "bytes " + startPoint + "-" + (startPoint + byteCount) + "/" + fStream.Length);

                        byte[] byRemoteInfo = WebClientObj.UploadData(url, "POST", data);
                        string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);

                        action?.Invoke(path, -2, startPoint, length);

                        //ShowProgress(Convert.ToInt32(startPoint * 100 / length));

                        //  获取返回信息
                        if (sRemoteInfo.Trim() != "")
                        {
                            sMsg = sRemoteInfo;
                            break;

                        }
                    }
                    catch (Exception ex)
                    {
                        sMsg = ex.ToString();
                        break;
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                sMsg = sMsg + ex.ToString();
            }
            try
            {
                bReader.Close();
                fStream.Close();
            }
            catch (Exception exMsg)
            {
                sMsg = exMsg.ToString();
            }

            GC.Collect();
        }



        private static void HttpUploadFiles(string path, string product, byte[] bytes,Action<string,string,long,long> action)
        {

        }

        private static int GetFileLenth(string product, string fileName)
        {
            WebClient WebClientObj = new WebClient();
            var url = GetExistUrl(product, fileName);
            byte[] byRemoteInfo = WebClientObj.DownloadData(url);
            string result = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            return Convert.ToInt32(result);
        }

        public static string GetExistUrl(string product, string fileName)
        {
            return serverIP + string.Format(urlExist, product, fileName, new Random().Next());
        }

        public static string GetUploadUrl(string product, string fileName)
        {
            return serverIP + string.Format(urlUpload, product, fileName, new Random().Next());
        }

        #region 获取文件存储名称
        public static string GetFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100, 999)+".png";
        }

        public static string[] GetFileName(int count)
        {
            string[] names = new string[count];
            string now = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100, 999);
            for (int i = 0; i < count; i++)
            {
                names[i] = now + CheckNum(i + 1)+".png";
            }

            return names;
        }

        private static string CheckNum(int num)
        {
            if (num < 10) return "00" + num;
            if (num < 100) return "0" + num;
            return "" + num;
        }
        #endregion
    }
}
