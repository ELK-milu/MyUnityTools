using Plusbe.Core;
using PlusbeSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Plusbe.Config
{
    //      ┏┓　　　┏┓
    //┏┛┻━━━┛┻┓
    //┃　　　　　　　┃ 　
    //┃　　　━　　　┃
    //┃　┳┛　┗┳　┃
    //┃　　　　　　　┃
    //┃　　　┻　　　┃
    //┃　　　　　　　┃
    //┗━┓　　　┏━┛

    //       ┃　　　┃   神兽保佑　　　　　　　　

    //       ┃　　　┃   代码无BUG！
    //       ┃　　　┗━━━┓
    //       ┃　　　　　　　┣┓
    //       ┃　　　　　　　┏┛
    //       ┗┓┓┏━┳┓┏┛
    //          ┃┫┫　 ┃┫┫
    //          ┗┻┛　 ┗┻┛


    public class AppConfig
    {
        private AppConfig() { }

        private static string dataPath = GlobalSetting.AppConfigPath;

        private static AppConfig appConfig;

        private static string toIP;
        private static string toPort;

        public List<add> appSettings;

        public static void initDataPath(string path)
        {
            dataPath = path;
        }

        public static AppConfig Instance
        {
            get
            {
                if (appConfig == null)
                {
                    if (File.Exists(dataPath))
                    {
                        appConfig = XmlSerializerHelper.Load(typeof(AppConfig), dataPath) as AppConfig;
                    }
                    else
                    {
                        appConfig = new AppConfig();
                        appConfig.appSettings = new List<add>();


                        appConfig.InsertKeyValue("ProductName", "PlusbeUnity", "------产品名称------");

                        //窗体控制 置顶！！！
                        appConfig.InsertKeyValue("Max", "1", "------是否全屏等窗体控制------");
                        appConfig.InsertKeyValue("TopMost", "1", "是否置顶");
                        appConfig.InsertKeyValue("Left", "0", "X");
                        appConfig.InsertKeyValue("Top", "0", "Y");
                        appConfig.InsertKeyValue("Width", "1920", "宽度");
                        appConfig.InsertKeyValue("Height", "1080", "高度");
                        appConfig.InsertKeyValue("LocationTime", "10", "(max=3)重新定位时间 单位秒");
                        appConfig.InsertKeyValue("AutoHide", "0", "------自动最小化------");

                        //单点数据

                        appConfig.InsertKeyValue("HideCursor", "0", "------隐藏鼠标------");
                        appConfig.InsertKeyValue("HideTaskbar", "0", "------隐藏任务栏------");
                        

                        appConfig.InsertKeyValue("Port", "8020", "------启动监听端口------");

                        

                         //是否为调试版本
                        appConfig.InsertKeyValue("Debug", "0", "------是否为调试版本------");
                        appConfig.InsertKeyValue("KillMySelf", "0", "------是否强制检测杀本进程------");

                        //--------------------关联数据 内容配置---------------------------

                        //视频同步转发
                        appConfig.InsertKeyValue("ToIP", "127.0.0.1", "------接收端IP设置------");
                        appConfig.InsertKeyValue("ToPort", "8021", "接收端端口");

                        //数据更新
                        appConfig.InsertKeyValue("ServerMode", "1", "------是否进行数据更新------");
                        appConfig.InsertKeyValue("ServerIP", "192.168.1.20:100", "服务器地址");
                        appConfig.InsertKeyValue("ServerIP2", "192.168.1.20:100", "备用服务器地址");
                        appConfig.InsertKeyValue("MyIP", "192.168.1.3", "本地/数据IP");
                        appConfig.InsertKeyValue("EnabledServerTimer", "1", "是否启用数据自动更新计时器");

                        appConfig.InsertKeyValue("COM", "COM5", "端口号");

                        appConfig.InsertKeyValue("TargetFrame", "60", "帧率");
                        appConfig.InsertKeyValue("QualityLevel", "2", "画质");

                        appConfig.InsertKeyValue("CameraWidth", "640", "摄像头宽度");
                        appConfig.InsertKeyValue("CameraHeight", "480", "摄像头高度");

                        appConfig.Save();
                    }

                    InitData();
                    
                }

                return appConfig;
                
            }
        }

        public void Init()
        { 
        
        }

        #region 根据键获取值 键值初始化
        public int GetValueByKeyInt(string key)
        {
            return Convert.ToInt32(GetValueByKey(key));
        }

        public bool GetValueByKeyBool(string key)
        {
            string value = GetValueByKey(key);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else if (value == "0")
            {
                return false;
            }
            else if (value == "1")
            {
                return true;
            }

            return Convert.ToBoolean(value);
        }

        public string GetValueByKey(string key)
        {
            return GetValueByKey(key, "", "");
        }

        public string GetValueByKey(string key, string value)
        {
            return GetValueByKey(key, value, "");
        }

        public string GetValueByKey(string key, string value, string brief)
        {
            int index = GetIndexByKey(key);
            if (index != -1)
            {
                return appSettings[index].value;
            }

            InsertKeyValue(key, value, brief);
            Save();
            return value;
        }

        private void InsertKeyValue(string key, string value, string brief)
        {
            int index = GetIndexByKey(key);
            if (index != -1)
            {
                appSettings[index].value = value;
                appSettings[index].brief = brief;
                return;
            }

            add item = new add();
            item.key = key;
            item.value = string.IsNullOrEmpty(value) ? "0" : value;
            item.brief = brief;
            appSettings.Add(item);
        }

        public void InsertKeyValue(string key, string value)
        {
            int index = GetIndexByKey(key);
            if (index != -1)
            {
                appSettings[index].value = value;
            }
        }

        public int GetIndexByKey(string key)
        {
            for (int i = 0; i < appSettings.Count; i++)
            {
                if (appSettings[i].key == key)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion

        #region 功能操作
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            XmlSerializerHelper.SaveXml(Instance, dataPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < appSettings.Count; i++)
            {
                sb.Append("key:" + appSettings[i].key + "*****value:" + appSettings[i].value + "*****brief:" + appSettings[i].brief + "\r\n");
            }

            return sb.ToString();
        }
        #endregion

        #region 数据静态化

        public static void InitData()
        {
            
        }

        #endregion
    }

    public class add
    {
        [XmlAttribute("key")]
        public string key;

        [XmlAttribute("value")]
        public string value;

        [XmlAttribute("brief")]
        public string brief;
    }
}
