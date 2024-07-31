using Newtonsoft.Json.Linq;
using Plusbe.Core;
using PlusbeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Plusbe.Config
{

    public class JsonDataManager
    {
        public static string jsonPath = GlobalSetting.JsonConfigPath;

        public static JObject jobject = null;
        public static JArray jChannel;
        public static JArray jList;
        public static int rootID;
        public static string json;

        //public 

        //public static ZhouData lastData;

        /// <summary>
        /// channelandfile 配置json
        /// </summary>
        public static void Init()
        {
            json = PathHelper.GetData(jsonPath);

            try
            {
                jobject = JObject.Parse(json);
                jChannel = jobject["Channellist"] as JArray;
                jList = jobject["list"] as JArray;

                if (jChannel.Count > 0) rootID = Convert.ToInt32(jChannel[0]["ID"]);

                Debug.Log("json>>jChannel:" + jChannel.Count + ">>jList:" + jList.Count);
            }
            catch
            { }
        }

        public static string GetChannelFileIndex(int index)
        {
            if (jChannel != null)
            {
                if (index >= 0 && index < jChannel.Count)
                {
                    JArray array = jChannel[index]["News"] as JArray;
                    if (array != null && array.Count > 0) return array[0]["Files"].ToString();
                }
                //for (int i = 0; i < jChannel.Count; i++)
                //{
                //    if (Convert.ToInt32(jChannel[i]["ID"]) == id)
                //    {
                //        JArray array = jChannel[i]["News"] as JArray;
                //        if (array != null && array.Count > 0) return array[0]["Files"].ToString();
                //    }
                //}
            }

            return "";
        }

        public static string GetChannelFile(int id)
        {
            if (jChannel != null)
            {
                for (int i = 0; i < jChannel.Count; i++)
                {
                    if (Convert.ToInt32(jChannel[i]["ID"]) == id)
                    {
                        JArray array = jChannel[i]["News"] as JArray;
                        if (array != null && array.Count > 0) return array[0]["Files"].ToString();
                    }
                }
            }

            return "";
        }

        public static JToken GetChannel(int index)
        {

            if (jChannel != null && index < jChannel.Count)
            {
                return jChannel[index];
                //for (int i = 0; i < jChannel.Count; i++)
                //{
                //    if (Convert.ToInt32(jChannel[i]["ID"]) == id) return jChannel[i]["SinglePic"].ToString();
                //}
            }

            return null;
        }

        //public static string GetChannelFile(int id)
        //{

        //    if (jChannel != null)
        //    {
        //        for (int i = 0; i < jChannel.Count; i++)
        //        {
        //            if (Convert.ToInt32(jChannel[i]["ID"]) == id) return jChannel[i]["SinglePic"].ToString();
        //        }
        //    }

        //    return "";
        //}

        public static string GetListFile(int id)
        {
            if (jList != null)
            {
                for (int i = 0; i < jList.Count; i++)
                {
                    if (Convert.ToInt32(jList[i]["ID"]) == id) return jList[i]["File"].ToString();
                }
            }

            return "";
        }

        public static string GetListFileByIndex(int index)
        {
            if (jList != null && index < jList.Count)
            {
                return jList[index]["ID"].ToString();
            }

            return "";
        }

        public static string GetListFileByIndex(int index, int tag)
        {
            int count = 0;

            if (jList != null)
            {
                for (int i = 0; i < jList.Count; i++)
                {
                    if (Convert.ToInt32(jList[i]["ChannlID"]) == tag)
                    {
                        if (index == count) return jList[i]["ID"].ToString();

                        count++;
                    }
                }
            }

            return "";
        }

        public static JToken GetListJToken(int index)
        {
            if (jList != null && index < jList.Count)
            {
                return jList[index];
            }

            return null;
        }

        public static string GetIndexFile(int tag)
        {
            if (jList != null)
            {
                for (int i = 0; i < jList.Count; i++)
                {
                    if (Convert.ToInt32(jList[i]["ChannlID"]) == tag) return jList[i]["File"].ToString();
                }
            }

            return "";
        }

        public static string GetIndexFile(string tag)
        {
            return GetIndexFile(Convert.ToInt32(tag));
        }

        public static string GetIndexFile()
        {
            if (jList != null && jList.Count > 0)
            {
                return jList[0]["File"].ToString();
            }
            return "";
        }

        public static string GetTagByParentAndName(int pid, string name)
        {

            if (jChannel != null)
            {
                for (int i = 0; i < jChannel.Count; i++)
                {
                    if (Convert.ToInt32(jChannel[i]["ParentID"]) == pid && jChannel[i]["Name"].ToString() == name) return jChannel[i]["ID"].ToString();
                }
            }

            return "";
        }

        public static List<JToken> GetJTokens(int tag)
        {
            return GetJTokens(tag.ToString());

            //List<JToken> tokens = new List<JToken>();

            //if (jList != null)
            //{
            //    for (int i = 0; i < jList.Count; i++)
            //    {
            //        if (Convert.ToInt32(jList[i]["Tag"]) == tag)
            //            tokens.Add(jList[i]);
            //    }
            //}

            //return tokens;
        }
        //
        public static List<JToken> GetJTokens(string tag)
        {
            List<JToken> tokens = new List<JToken>();

            if (jList != null)
            {
                for (int i = 0; i < jList.Count; i++)
                {
                    if (jList[i]["ChannlID"].ToString() == tag || string.IsNullOrEmpty(tag))
                        tokens.Add(jList[i]);
                }
            }

            return tokens;
        }

        public static string GetFileByIndex(int index)
        {
            JToken token = GetListJToken(index);
            if (token != null)
            {
                return token["File"].ToString();
            }

            return "";
        }

        public static string GetJson(string tag)
        {
            List<JToken> tokens = GetJTokens(tag);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                sb.Append("{\"ID\":" + tokens[i]["ID"].ToString() + ",\"title\":\"" + tokens[i]["Title"].ToString() + "\",\"word\":\"\", \"index\":" + i + ",\"fileType\":" + tokens[i]["FileType"].ToString() + ",\"tag\":\"" + tokens[i]["ChannlID"].ToString() + "\"},");
            }

            return "{ \"list\":[" + sb.ToString().TrimEnd(',') + "]}";
        }

        //通过 ParentID 获取 JChannel list
        public static List<JToken> GetJChannelTokensByParentID(string tag)
        {
            List<JToken> tokens = new List<JToken>();

            if (jChannel != null)
            {
                for (int i = 0; i < jChannel.Count; i++)
                {
                    if (jChannel[i]["ParentID"].ToString() == tag || string.IsNullOrEmpty(tag))
                        tokens.Add(jChannel[i]);
                }
            }

            return tokens;
        }
        //通过 Depth 获取 JChannel list
        public static List<JToken> GetJChannelTokensByDepth(string tag)
        {
            List<JToken> tokens = new List<JToken>();

            if (jChannel != null)
            {
                for (int i = 0; i < jChannel.Count; i++)
                {
                    if (jChannel[i]["Depth"].ToString() == tag || string.IsNullOrEmpty(tag))
                        tokens.Add(jChannel[i]);
                }
            }

            return tokens;
        }

        //根据 ID 获取 JChannelToken
        public static JToken GetChannelTokenByID(string id)
        {

            if (jChannel != null)
            {

                for (int i = 0; i < jChannel.Count; i++)
                {
                    if (jChannel[i]["ID"].ToString() == id) return jChannel[i];
                }
            }

            return null;
        }
    }
}

