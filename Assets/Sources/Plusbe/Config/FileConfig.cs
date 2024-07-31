using Plusbe.Utils;
using Plusbe.Config;
using PlusbeSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Plusbe.Core;
using UnityEngine;

namespace Plusbe.Config
{
    [XmlType("Config")]
    public class FileConfig
    {
        #region 变量声明
        private static string dataPath = GlobalSetting.FileConfigPath;
        private static FileConfig fileConfig;
        
        public bool NeedUpdate;
        public string NowVersion;
        public List<NetFile> FileList;

        #endregion

        #region 单列

        public static void Init()
        {
            dataPath = GlobalSetting.FileConfigPath;
            Debug.Log("file>>list:" + Instance.getCount());
        }

        public static FileConfig Instance
        {
            get
            {
                if (fileConfig == null)
                {
                    if (File.Exists(dataPath))
                    {
                        fileConfig = XmlSerializerHelper.Load(typeof(FileConfig), dataPath) as FileConfig;
                    }
                    else
                    {
                        fileConfig = new FileConfig();
                        fileConfig.FileList = new List<NetFile>();
                        fileConfig.NeedUpdate = true;
                        fileConfig.NowVersion = "-1";

                        fileConfig.save();
                    }
                }

                return fileConfig;
            }
            
        }
        #endregion

        #region JSON数据返回

        //public string getJson(string tag)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int count = getCount();
        //    if (tag == "" || tag == "0")
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            sb.Append("{\"title\":\"" + getSingle().FileList[i].Title + "\",\"index\":" + getSingle().FileList[i].ID + "},");
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (getSingle().FileList[i].Tag == tag)
        //            {
        //                sb.Append("{\"title\":\"" + getSingle().FileList[i].Title + "\",\"index\":" + getSingle().FileList[i].ID + "},");
        //            }
        //        }

        //        currTag = tag;
        //    }

        //    return "{\"list\":[" + sb.ToString().Trim(',') + "]}";
        //}

        //public string getJsonByCurrIndex()
        //{
        //    return getJsonByIndex(currIndex);
        //}

        public string getJsonByIndex(int index)
        {
            StringBuilder sb = new StringBuilder();
            if (index < getCount())
            {
                sb.Append("{\"title\":\"" + Instance.FileList[index].Title + "\",\"index\":" + Instance.FileList[index].ID + "},");
            }

            return "{\"list\":[" + sb.ToString() + "]}";
        }

        //public string getJsonPic()
        //{
        //    string file = "";
        //    if (getCount() != 0 && currIndex < getCount())
        //    {
        //        if (getSingle().FileList[currIndex].FileType == "4")
        //        {
        //            file = getSingle().FileList[currIndex].File;
        //        }
        //    }

        //    return getJsonPicByFile(file);
        //}

        public string getJsonPicByFile(string file)
        {
            if (file == "") return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root></root>";

            string xml = "";
            xml += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xml += "<root>";
            string[] piclist = file.Split('|');

            for (int j = 0; j < piclist.Length; j++)
            {
                xml += "<content id=\"" + j + "\" name=\"" + "HHH" + "\">";
                xml += "<pic>../" + piclist[j] + "</pic>";
                xml += "<word></word>";
                xml += "</content>";
            }
            xml += "</root>";
            return xml;
        }

        #endregion

        #region 功能操作

        public int getCount()
        {
            return Instance.FileList.Count;
        }

        public void setSingle(FileConfig config)
        {
            fileConfig = config;
        }

        public void save()
        {
            XmlSerializerHelper.SaveXml(Instance, dataPath);
        }
        #endregion

        #region 临时FileConfig
        public static FileConfig getTempConfig(string strInfo)
        {
            return XmlSerializerHelper.FromXml<FileConfig>(strInfo);
        }


        public string GetFile(int id)
        {
            for (int i = 0; i < FileList.Count; i++)
            {
                if (id == FileList[i].ID) return FileList[i].File;
            }

            return "";
        }

        #endregion
    }

    //视频 = 1,
    //链接 = 2,
    //ppt = 3,
    //图片 = 4,
    //客户端 = 5

    public class NetFile
    {
        public int ID;
        public string Title;
        public string File;
        public string Word;
        public int Tag;
        public int FileType;
        public bool ISrh;
        public bool IsJz;
        public string Bejson;
    }
}
