/*
*┌────────────────────────────────────────────────┐
*│　描    述：NewsXinhuaHelper                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0                                              
*│　创建时间：2020/3/24 16:25:26                        
*└────────────────────────────────────────────────┘
*/

using Plusbe.Helper;
using Plusbe.Http;
using PlusbeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Mono
{

    //http://www.xinhuanet.com/politics/news_politics.xml|http://www.xinhuanet.com/fortune/news_fortune.xml|http://www.xinhuanet.com/tech/news_tech.xml|http://www.xinhuanet.com/world/news_world.xml
    public class NewsXinhuaHelper: MonoBehaviour
    {
        public const string URL_TECH = "http://www.xinhuanet.com/tech/news_tech.xml";
        public const string URL_POLITICS = "http://www.xinhuanet.com/politics/news_politics.xml";
        public const string URL_FORTUNE = "http://www.xinhuanet.com/fortune/news_fortune.xml";

        private string title;
        private string url;
        private string tempPath;
        private int updateTime;

        private List<NewsXinhuaItem> itemList = new List<NewsXinhuaItem>();

        private static NewsXinhuaHelper instance;

        public static NewsXinhuaHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<NewsXinhuaHelper>();
                }

                return instance;
            }
        }

        public List<NewsXinhuaItem> GetNews(int top)
        {
            List<NewsXinhuaItem> temps = new List<NewsXinhuaItem>();

            int maxLen = top < itemList.Count ? top : itemList.Count;
            for (int i = 0; i < maxLen; i++)
            {
                temps.Add(itemList[i]);
            }

            return temps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">标准名称</param>
        /// <param name="url">链接地址</param>
        /// <param name="tempPath">本地缓存临时文件</param>
        /// <param name="updateTime">更新时间,单位秒,未启用</param>
        public void Init(string title, string url, string tempPath, int updateTime)
        {
            this.title = title;
            this.url = url;
            this.tempPath = tempPath;
            this.updateTime = updateTime;

            //读取本地文件信息
            UpdateLocalData();

            //更新网络信息
            CheckVersion();
        }


        public void CheckVersion()
        {
            new Thread(DoCheckVersion).Start();
        }

        private void DoCheckVersion()
        {
            string content = HttpHelper.HtmlCode(url);
            UpdateNewsData(content);
        }

        /// <summary>
        /// 更新缓存本地文件
        /// </summary>
        private void UpdateLocalData()
        {
            string content = FileHelper.ReadText(tempPath);
            UpdateNewsData(content);
        }

        private void UpdateNewsData(string content)
        {
            if (content == "") return;

            try
            {
                
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);
                XmlNodeList list = xmlDoc.DocumentElement.GetElementsByTagName("item");
                int count = list.Count;

                itemList.Clear();

                string pattern = "<a(?:\\s+.+?)*?\\s+href=(.*?).*?>";
                Regex regex = new Regex(pattern);
                Regex regex2 = new Regex("<font(?:\\s+.+?)*?\\s+color=(.*?).*?>");

                //获取有效新闻
                for (int i = 0; i < count; i++)
                {
                    //XmlNode xmlNode = list[i];
                    NewsXinhuaItem item = new NewsXinhuaItem();
                    item.index = i;
                    string title = list[i].SelectSingleNode("title").InnerText;
                    item.title = regex.Replace(title, "").Replace("</a>", "").Replace("\r\n", "");

                    item.link = list[i].SelectSingleNode("link").InnerText.Replace("\"", "");
                    item.link = item.link.Replace("http://ent.news.cn/", "http://www.xinhuanet.com/ent/");
                    item.hLink = GetDirectoryNameByHtml(item.link);
                    //item.repeatCount = 0;
                    item.image = "";
                    item.video = "";
                    string des = list[i].SelectSingleNode("description").InnerText;
                    if (!string.IsNullOrEmpty(des))
                    {
                        des = regex2.Replace(regex.Replace(des, "").Replace("</a>", ""), "").Replace("</font>", "");
                    }

                    XmlNode node = new XmlDocument().CreateNode(XmlNodeType.CDATA, "", "");
                    node.InnerText = des;
                    item.description = node;
                    item.descriptionContent = des;

                    //crawlerLink.Add(item.link);

                    if(!string.IsNullOrEmpty(item.descriptionContent))
                        itemList.Add(item);

                }

                if (itemList.Count > 5)
                {
                    //保证正确解析超过5条，缓存改文件
                    FileHelper.WriteText(tempPath, content);
                }

                //if (m_debug == 1)
                //{
                //    itemList[0].link = "http://news.xinhuanet.com/politics/2016-09/08/c_1119535078.htm";
                //}
            }
            catch (Exception err)
            {
                Debug.Log("xinhuanews error:" + err.ToString()) ;

                //addTextUrl("出错拉" + err.ToString());
            }
        }

        /// <summary>
        /// 截取路径名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetDirectoryNameByHtml(string url)
        {
            return url.Substring(0, url.LastIndexOf("/"));
        }
    }

    public class NewsXinhuaItem
    {
        public int index { get; set; }
        public int repeatCount { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string hLink { get; set; }
        public XmlNode description { get; set; }

        public string descriptionContent;

        public string image { get; set; }
        public string image2 { get; set; }
        public string video { get; set; }
        public string video2 { get; set; }
    }
}
