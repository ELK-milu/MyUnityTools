using Plusbe.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace AppCustom
{
    /// <summary>
    /// 参考样例,所有涉及到第三方接口请求等
    /// </summary>
    public class WebAPI
    {
        public static string ServerIP = "http://127.0.0.1:8003/";

        private static string UrlBand = "CardNum.aspx?act=bing&uid={0}&icnum={1}&r={2}"; //绑定用户id与ic卡

        private static string UrlRegister = "CardNum.aspx?act=regist&name={0}&fullname={1}&phone={2}&sex={3}&age={4}&hy={5}&pic={6}&uid={7}&r={8}"; //用户注册

        #region 用户信息绑定 get
        public static string UserBand(string uid, string icNum)
        {
            string url = string.Format(ServerIP + UrlBand, uid, icNum, RandNum);
            return HttpHelper.HtmlCode(url);
        }

        public static string UserBand(string uid, string icNum, out string url)
        {
            url = string.Format(ServerIP + UrlBand, uid, icNum, RandNum);
            return HttpHelper.HtmlCode(url);
        }
        #endregion

        #region 用户信息注册 post

        //public static string UserRigister(string name, string fullName, string phone, int sex, int age, int area, string base64Pic, string uid = "")
        //{
        //    string url = string.Format(ServerIP + UrlRegister, name, fullName, phone, sex, age, area, base64Pic, uid, RandNum);
        //    return HttpHelper.HtmlCode(url);
        //}

        public static string UserRigister(string name, string fullName, string phone, int sex, int age, int area, string base64Pic, string guid, string uid, out string url)
        {
            url = string.Format(ServerIP + UrlRegister, name, fullName, phone, sex, age, area, "base64", uid, RandNum);
            string url2 = "act=regist&name={0}&fullname={1}&phone={2}&sex={3}&age={4}&hy={5}&pic={6}&uid={7}&r={8}&token={9}";
            string data = string.Format(url2, name, fullName, phone, sex, age, area, base64Pic, uid, RandNum, guid);
            string msg = PostWebRequest(ServerIP + "CardNum.aspx", data, Encoding.UTF8);
            return msg;
        }

        #endregion

        #region common
        private static string RandNum
        {
            get
            {
                return "" + new Random().Next(99999);
            }
        }

        public static string GetString(HttpWebResponse response)
        {
            if (response != null)
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return "{\"time_used\": 10, \"error_message\": \"ERROR_NETWORK\", \"request_id\": \"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"}";
        }

        static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        #endregion
    }
}


