using Plusbe.Utils;
using Plusbe.Http;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using PlusbeHelper;
using System.Threading;

namespace Plusbe.Config
{
    public class DownConfig
    {
        private static DownConfig downConfig;

        public static string url_channel = "channellist.aspx";
        public static string url_version = "versionxml.aspx";

        public static string url_v = "";
        public static string url_ip = "192.168.10.101";
        public static string url_host = "http://test.cloud.plusbe.com/";

        public static string file_path = "";
        private static bool isDownFileConfig = false;

        //http://test.cloud.plusbe.com/versionxml.aspx?ip=192.168.1.88&v=455


        //private int updateTime = 60;

        public static DownConfig GetInstance()
        {
            if (downConfig == null)
            {
                downConfig = new DownConfig();
            }

            return downConfig;
        }

        public void Init(string host,string ip,string path)
        {
            url_host = host;
            url_ip = ip;
            file_path = path;
        }

        /// <summary>
        /// 栏目信息更新
        /// 目前用到的更新不多
        /// </summary>
        public string UpdateChannel()
        {
            return HttpHelper.HtmlCode(Url_Channel);
        }

        public void CheckVersion()
        {
            if (!isDownFileConfig)
            {
                new Thread(UpdateVersion).Start();
            } 
        }

        private void UpdateVersion()
        {
            if (isDownFileConfig) return;
            isDownFileConfig = true;
            string nowVersion = FileConfig.Instance.NowVersion;
            url_v = string.IsNullOrEmpty(nowVersion) ? "-1" : nowVersion;
            string url = Url_Version;
            string result = HttpHelper.HtmlCode(url);
            HttpFileDown.DownResultType downResult = HttpFileDown.DownResultType.None;
            Debug.Log(result);
            if (result != "" && result != "false")
            {
                FileConfig temp = FileConfig.getTempConfig(result);
                //StringBuilder sb = new StringBuilder();
                bool downOK = true;
                if (temp.NowVersion != nowVersion)
                {
                    for (int i = 0; i < temp.FileList.Count; i++)
                    {
                        if (PlusbeMedia.isDownFileType(temp.FileList[i].FileType))
                        {
                            string[] files = temp.FileList[i].File.Split('|');
                            for (int j = 0; j < files.Length; j++)
                            {
                                if (PlusbeMedia.isDownFile(files[j]))
                                {
                                    downResult = HttpFileDown.DownloadFile(url_host + files[j], file_path + files[j], false);
                                    if (downResult == HttpFileDown.DownResultType.Exist)
                                    {
                                        //PlusbeDebug.Log("已经存在--" + files[j]);
                                    }
                                    else if (downResult == HttpFileDown.DownResultType.Success)
                                    {
                                        PlusbeDebug.Log("下载成功--" + files[j]);
                                    }
                                    else
                                    {
                                        if (files[j].IndexOf("UploadFiles") != -1)
                                        {
                                            downOK = false;
                                            PlusbeDebug.Log("下载失败--" + files[j]);
                                        }
                                        else
                                        {
                                            downResult = HttpFileDown.DownloadFile(url_host +"UploadFiles/"+ files[j], file_path + files[j], false);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (downOK)
                    {
                        PlusbeDebug.Log("版本数据更新成功(Yes,成功)--" + nowVersion + "-->" + temp.NowVersion);
                        FileConfig.Instance.setSingle(temp);
                        FileConfig.Instance.save();
                        //if (showDownSuccess != null)
                        //{
                        //    showDownSuccess(1);
                        //}
                    }
                }
                else
                {
                    PlusbeDebug.Log("无需更新，版本号：" + nowVersion);
                }
            }
            else if (result == "false")
            {
                PlusbeDebug.Log("无需更新，版本号：" + nowVersion);
            }
            else
            {
                PlusbeDebug.Log("更新请失败，请查看网络链接及远程服务器->" + url);
            }
                
            isDownFileConfig = false;
        }

        #region 更新远程地址

        private string Url_Channel
        {
            get{ return url_host + url_channel + "?r=" + new System.Random().Next();}
        }

        private string Url_Version
        {
            get { return url_host + url_version + "?ip=" + url_ip + "&v=" + url_v + "&r=" + new System.Random().Next(); }
        }

        #endregion
    }
}
