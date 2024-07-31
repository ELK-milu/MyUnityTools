using Plusbe.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Core
{
    /// <summary>
    /// 所有目录的路径包含 "/"
    /// </summary>
    public class GlobalSetting
    {
        //streamingAssetsPath目录
        public static string FilePath = Application.streamingAssetsPath; 

        //本地文件路径
        public static string DataPath = Application.streamingAssetsPath;
        public static string CodePath = "Codes/";
        public static string SignPath = "Signs/";
        public static string DownPath = "Downs/";
        public static string PhotoPath = "Photos/";
        public static string PhotoThumbPath = "PhotoThumbs/";
        public static string TempPath = "Temps/";
        public static string SkinPath = "Skins/";

        //本地配置文件路径
        public static string AppConfigPath = "Configs/AppConfig.xml";
        public static string JsonConfigPath = "Configs/FileConfig.json";
        public static string FileConfigPath = "Configs/FileConfig.xml";
        public static string WelcomeXmlPath = "Welcome/WelcomeXml.xml";

        public static string ServerIP = "";
        public static string CodeServerIP = ""; 
        public static string FaceServerIP = "";

        public static string KeyPath = "Key/license";
        public static string UniqueKey = "";
        public static string ProductName = "Test2020";
        public static int ProductVersion = 1;

        public static string ABPath = "AssetBundles/";


        public static string LastPhoto;

        public static string ToIP = "127.0.0.1";
        public static string ToPort = "8020";

        public static string DateDirectory = "";
        public static string CheckedPath = "Nikon/";

        //public static string testA;


        public static void Init()
        {
            DataPath = Application.isEditor ? Application.streamingAssetsPath + "/../../Apps/Datas/" : GlobalSetting.DataPath = Application.streamingAssetsPath + "/../../Datas/";
            DataPath = Path.GetFullPath(DataPath);

            CodePath = DataPath + CodePath;
            SignPath = DataPath + SignPath;
            DownPath = DataPath + DownPath;
            KeyPath = DataPath + KeyPath;
            PhotoPath = DataPath + PhotoPath;
            PhotoThumbPath = DataPath + PhotoThumbPath;
            TempPath = DataPath + TempPath;
            SkinPath = DataPath + SkinPath;

            ABPath = DataPath + ABPath;
            CheckedPath = DataPath + CheckedPath;

            AppConfigPath = DataPath + AppConfigPath;
            JsonConfigPath = DataPath + JsonConfigPath;
            FileConfigPath = DataPath + FileConfigPath;
            WelcomeXmlPath = DataPath + WelcomeXmlPath;


            DateDirectory = DateTime.Now.ToString("yyyy-MM-dd") + "/";

            UniqueKey = "plusbe"+SystemInfo.deviceUniqueIdentifier;

            ToIP = AppConfig.Instance.GetValueByKey("ToIP");
            ToPort = AppConfig.Instance.GetValueByKey("ToPort");
        }
    }
}
