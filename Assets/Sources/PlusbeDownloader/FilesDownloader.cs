using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using UnityEngine.Events;
using UnityEngine;
using Plusbe.Core;
using UnityEngine.Networking;
//下载文件
public class FilesDownloader 
{
    //下载列表
    public List<string> filesNeedToDownload = new List<string>();
    //当前任务下标
    public int currIndex = 0;
    //当前任务下载字节
    public long currLoadedBytes = 0;
    //当前任务总字节
    public long currTotalBytes = 0;
    private string foldName = "";

    public UnityEvent DownloadBreakedEvent = new UnityEvent();

    public string CurrFileName
    {
        get
        {
            if (filesNeedToDownload.Count <= currIndex)
            {
                return "";
            }
            string str = filesNeedToDownload[currIndex];
            return Path.GetFileName(str);
        }
    }

    //添加要下载的文件
    public void AddFile(string filePath)
    {
        filesNeedToDownload.Add(filePath);
    }

    //逐个下载列表中的文件到指定文件夹
    public void Start(string foldName)
    {
        this.foldName = foldName;
        
        checkFilePath( GlobalSetting.DataPath+ foldName + "/UploadFiles/");//先检查文件夹

        for (int i = 0; i < filesNeedToDownload.Count; i++)
        {
            currIndex = i;

            string url = filesNeedToDownload[i];
            string fileName = GetFileName(url);
            string fmt = Path.GetExtension(url);
            string savePath = GlobalSetting.DataPath + foldName + "\\UploadFiles\\" + fileName + fmt;

            Console.WriteLine("savePath:" + savePath);
            bool IsSuccess = DownLoadFile(savePath, url);

            if (IsSuccess)
            {
                Console.WriteLine(url + "下载成功");
            }
            else
            {
                Console.WriteLine(url + "下载失败");
            }
        }
    }

    //下载json文件
    public void DownloadJson(string content)
    {
        SaveTxt(GlobalSetting.DataPath + foldName + "/Config.json", content);
    }

    private void SaveTxt(string filePath, string text)
    {
        bool IsExist = checkFilePath(filePath);//检查文件夹是否存在
        if (IsExist)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        StreamWriter streamWrite = File.AppendText(filePath);
        streamWrite.Write(text);
        streamWrite.Flush();
        streamWrite.Close();
    }

    //下载文件
    private bool DownLoadFile(string filePath, string url)
    {
        bool isDownloadSuccessed = false;//是否下载成功

        long startPosition = 0;
        FileStream fileStream;
        string tempDownloadFile = Path.ChangeExtension(filePath, ".downloadTemp");//临时下载文件路径
        
        //1.判断要下载的文件是否存在否则新建文件流或者断点续传
        if (File.Exists(filePath))
        {
            return true;
        }

        if (File.Exists(tempDownloadFile))
        {
            fileStream = File.OpenWrite(tempDownloadFile);//打开已经下载的文件
            startPosition = fileStream.Length;//获取已经下载的长度
            fileStream.Seek(startPosition, SeekOrigin.Current);//本地文件写入位置定位
            currLoadedBytes = startPosition;
        }
        else
        {
            fileStream = new FileStream(tempDownloadFile, FileMode.Create);
            currLoadedBytes = 0;
        }

        //2.从网页获取数据流并写入到文件流中
        try
        {
            currTotalBytes = GetHttpFileLen(url);
            if (currLoadedBytes == currTotalBytes && currTotalBytes > 1024)
            {
                //文件已经下载完成
                return true;
            }
            HttpWebRequest httpWebReq = (HttpWebRequest)WebRequest.Create(url);
            if (startPosition > 0)
            {
                httpWebReq.AddRange((int)startPosition);
            }
            HttpWebResponse httpWebRes = (HttpWebResponse)httpWebReq.GetResponse();
            Stream readStream = httpWebRes.GetResponseStream();

            byte[] bytes = new byte[1024];
            while (true)
            {
                int contentSize = readStream.Read(bytes, 0, bytes.Length);
                fileStream.Write(bytes, 0, contentSize);
                currLoadedBytes = fileStream.Length;
                if (contentSize <= 0) break;
            }

            readStream.Close();
            fileStream.Close();

            File.Move(tempDownloadFile, filePath);
            isDownloadSuccessed = true;

        }
        catch (Exception ex)
        {
            Console.WriteLine("=====下载出现异常====\n" + ex.ToString());
            fileStream.Close();
            DownloadBreakedEvent?.Invoke();
        }
        return isDownloadSuccessed;
    }

    //检查文件夹是否存在，不存在就创建文件夹
    private bool checkFilePath(string filePath)
    {
        int startIndex = filePath.LastIndexOf("/");
        int count = filePath.Count() - startIndex;
        string directory = filePath.Remove(startIndex, count);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            return false;
        }
        else
        {
            return true;
        }
    }

    //获取文件名
    private string GetFileName(string str)
    {
        return Path.GetFileNameWithoutExtension(str);
    }

    //获取下载文件的长度
    private long GetHttpFileLen(string url)
    {
        HttpWebRequest httpWebReq = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpWebRes = (HttpWebResponse)httpWebReq.GetResponse();
        httpWebRes.Close();
        return httpWebRes.ContentLength;
    }

    //清空列表
    public void ClearList()
    {
        filesNeedToDownload.Clear();
        currIndex = 0;
    }

}