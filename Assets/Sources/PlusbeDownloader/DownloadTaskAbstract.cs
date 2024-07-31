using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DownloadTaskAbstract
{
    public string TaskName;
    public string DownloadAPI;
    public string ServerHost;
    public DownloadStatus Status=DownloadStatus.Stop;
    public FilesDownloader downloader = new FilesDownloader();
    public TaskInfo Info = new TaskInfo();
    public abstract void Start();
    public abstract void Stop();
    public abstract void End();
    public abstract void Init(string taskName,string api,string serverHost);
    public abstract bool CheckUpdate();
}

public enum DownloadStatus
{
    Downloading,
    Stop,
    End
}

public class TaskInfo
{
    public List<string> files = new List<string>();

    public FilesDownloader downloader;
    public long DownloadBytes { get { return downloader.currLoadedBytes; } }
    public long TotalBytes { get { return downloader.currTotalBytes; } }
    public int CurrIndex { get { return downloader.currIndex; } }
    public string CurrFileName { get { return downloader.CurrFileName; } }
    public int TotalTaskNum { get { return files.Count; } }
}
