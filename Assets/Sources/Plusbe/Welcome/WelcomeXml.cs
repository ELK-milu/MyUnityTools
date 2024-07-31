using Plusbe.Core;
using PlusbeSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Plusbe.Welcome
{
    public class WelcomeXml
    {
        public string VersionNum;

        public List<Word> List;

        private static WelcomeXml instance;
        public static WelcomeXml Instance
        {
            get
            {
                if (instance == null)
                {
                    if (File.Exists(GlobalSetting.WelcomeXmlPath))
                    {
                        instance = XmlSerializerHelper.Load(typeof(WelcomeXml), GlobalSetting.WelcomeXmlPath) as WelcomeXml;
                    }
                    else
                    {
                        instance = new WelcomeXml();
                        instance.VersionNum = "-1";
                        instance.List = new List<Word>();
                        instance.Add("热烈欢迎1", "200");
                        instance.Save();
                    }
                }
                return instance;
            }
        }
        public static void Init()
        {
            Debug.Log("welcome>>>" + Instance.Count);
        }
        public string GetTitle(int index)
        {
            if (index >= 0 && index < Count)
            {
                string title = List[index].Title;

                return title.Replace("..", "\r\n");
            }
            return "热烈欢迎2";
        }
        public int GetSize(int index)
        {
            if (index >= 0 && index < Count)
            {
                int size = List[index].Size;

                return size;
            }
            return 200;
        }
        public int Count
        {
            get
            {
                return List.Count;
            }
        }
        public void Save()
        {
            XmlSerializerHelper.SaveXml(instance, GlobalSetting.WelcomeXmlPath);
            GetJson();
        }
        public void Add(string title, string size)
        {
            int count = List.Count;
            int id = 1;
            if (count >= 1) id = List[count - 1].ID + 1;

            Word word = new Word();
            word.ID = id;
            word.Title = title;
            word.Size = Convert.ToInt32(size);
            List.Add(word);
            Save();
        }
        public void Delete(string sid)
        {
            int count = instance.List.Count;
            int id = Convert.ToInt32(sid);
            for (int i = 0; i < count; i++)
            {
                if (List[i].ID == id)
                {
                    List.RemoveAt(i);
                    break;
                }
            }
            Save();
        }
        public void Update(string sid, string title, string size)
        {
            int count = List.Count;
            int id = Convert.ToInt32(sid);

            for (int i = 0; i < count; i++)
            {
                if (i == id)
                {
                    List[i].ID = id;
                    List[i].Title = title;
                    List[i].Size = Convert.ToInt32(size);
                }
            }
            Save();
        }
        public string GetJson()
        {
            StringBuilder sb = new StringBuilder();
            int count = List.Count;
            for (int i = 0; i < count; i++)
            {
                sb.Append("{ \"Title\":\"" + List[i].Title + "\", \"Size\":" + List[i].Size + ", \"ID\":" + List[i].ID + "}" + ",");
            }
            Debug.Log("???>>>" + "{\"list\":[" + sb.ToString().Trim(',') + "]}");
            return "{\"list\":[" + sb.ToString().Trim(',') + "]}";
        }
    }
    public class Word
    {
        public int ID;
        public string Title;
        public int Size;
    }
}
