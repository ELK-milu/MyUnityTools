using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Events;
using Plusbe.Core;
public class JsonDownloadTask : DownloadTaskAbstract
{
    public class MyCompleteEvent : UnityEvent<string> { };
    public MyCompleteEvent CompleteEvent = new MyCompleteEvent();
    public UnityEvent StartEvent = new UnityEvent();
    public override void Init(string taskName, string api, string serverHost)
    {
        TaskName = taskName;
        DownloadAPI = api;
        ServerHost = serverHost;
    }

    public override void Start()
    {
        //任务已经下载时返回
        if (Status == DownloadStatus.Downloading) return;
        Status = DownloadStatus.Downloading;
        //获取接口内容并筛选下载列表
        string jsonContent = GetHtmlContent(DownloadAPI);
        ////获取本地json数据
        //string path = GlobalSetting.DataPath + TaskName + "/Config.json";
        //if (File.Exists(path))
        //{
        //    string localJsonData = "";
        //    using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
        //    {
        //        localJsonData = streamReader.ReadToEnd();
        //        streamReader.Close();
        //    }
        //    if(localJsonData==jsonContent&&jsonContent!="")
        //    {
        //        CompleteEvent?.Invoke(TaskName);
        //    }
        //}
        if (jsonContent=="")
        {
            //没有获取到接口内容
            Debug.Log(TaskName + "没有获取到接口" + DownloadAPI + "的内容");
        }
        else
        {
            //将接口内容转为下载列表
            Info.files = ConvertJsonToFileList(jsonContent);
        }

        if(Info.files.Count!=0)
        {
            StartEvent?.Invoke();
            downloader = new FilesDownloader();
            Info.downloader = downloader;
            downloader.DownloadBreakedEvent.AddListener(Stop);
            for(int i=0;i<Info.files.Count;i++)
            {
                downloader.AddFile(Info.files[i]);
            }
            Action<string> d = downloader.Start;
            d.BeginInvoke(TaskName, OnCallBack, d);
        }
        
    }

    private void OnCallBack(IAsyncResult ar)
    {
        Action<string> d = ar.AsyncState as Action<string>;
        d.EndInvoke(ar);
        End();
    }

    public override void Stop()
    {
        Status = DownloadStatus.Stop;
        downloader.ClearList();
    }

    public override void End()
    {
        Status = DownloadStatus.End;
        string jsonContent = GetHtmlContent(DownloadAPI);
        if (jsonContent == "") return;
        downloader.DownloadJson(jsonContent);
        CompleteEvent?.Invoke(TaskName);
    }

    public override bool CheckUpdate()
    {
        if (Status == DownloadStatus.Downloading) return false;
        //Debug.Log("check update");
        //获取配置文件
        string path = GlobalSetting.DataPath + TaskName + "/Config.json";
        if(File.Exists(path))
        {
            string localJsonContent = "";
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                localJsonContent = streamReader.ReadToEnd();
                streamReader.Close();
            }
            
            string webJsonContent = GetHtmlContent(DownloadAPI);

            //Debug.Log(localJsonContent);
            //Debug.Log(webJsonContent);

            if (webJsonContent == "") return false;
            return webJsonContent != localJsonContent;
        }
        return false;
    }

    private List<string> ConvertJsonToFileList(string json)
    {
        List<string> list=new List<string>();
        try
        {
            //Debug.Log("json: " + json);
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while(reader.Read())
            {
                if(reader.Value!=null)
                {
                    string value = reader.Value.ToString();
                    bool IsDownloadAble = CheckIsDownloadAble(value);
                    if(IsDownloadAble)
                    {
                        string[] arr = value.Split('|');
                        for(int i=0;i<arr.Length;i++)
                        {
                            string file = arr[i];
                            if (file == "") continue;//过滤空字符串
                            if(file.IndexOf("http") != 0)
                            {
                                file = ServerHost + "/" + file;
                            }
                            //Debug.Log("file: " + file);
                            list.Add(file);
                        }
                    }
                }
            }
        }catch(Exception ex)
        {
            Debug.Log(TaskName + " 从Json中获取列表时出现异常:\n" + ex.Message);
        }
        return list;
    }

    private bool CheckIsDownloadAble(string value)
    {
        string checkStr = value.ToLower();
        string[] fileFormats = new string[] { ".mp4", ".mp3", ".txt", ".json", ".jpg", ".jpeg", ".png", ".gif", ".ppt", ".pptx", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };
        foreach(string fmt in fileFormats)
        {
            if (checkStr.LastIndexOf(fmt) == -1) continue;
            if (checkStr.LastIndexOf(fmt) == checkStr.Length - fmt.Length)
            {
                return true;
            }
        }
        return false;
    }

    private string GetHtmlContent(string url)
    {
        string str = "";
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.KeepAlive = false;
            request.Method = "Get";
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = 5000;
            request.AllowAutoRedirect = true;//是否允许302
            ServicePointManager.DefaultConnectionLimit = 20;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    str = streamReader.ReadToEnd();
                    streamReader.Close();
                    response.Close();
                }
            }
            request.Abort();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return str;
    }

    
}
