using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Timers;
public class DownloaderManager
{
    private static List<DownloadTaskAbstract> tasks = new List<DownloadTaskAbstract>();
    public static void CreateDownloadTask(string host, string pid, string myIP, string taskName)
    {
        if(host.Contains("http")==false)
        {
            host = "http://" + host;
        }
        string api = host + "/Admin/api/GetChannelFile.aspx?pid=" + pid + "&ip=" + myIP;
        CreateDownloadTask(TaskType.JsonAPIType, taskName, api, host);
    }

    /// <summary>
    /// 创建下载任务
    /// </summary>
    /// <param name="taskType"></param>
    /// <param name="taskName">任务的名称，会生成对应的文件夹名</param>
    /// <param name="api">获取下载内容的接口</param>
    /// <param name="host">服务器地址比如http://www.baidu.com</param>
    public static void CreateDownloadTask(TaskType taskType,string taskName,string api,string host)
    {
        if(taskType==TaskType.JsonAPIType)
        {
            JsonDownloadTask task = new JsonDownloadTask();
            task.Init(taskName, api, host);
            task.CompleteEvent.AddListener(TaskCompleted);
            tasks.Add(task);
        }
    }

    private static Timer timer = new Timer(5000);
    private static int completedNum = 0;
    private static void TaskCompleted(string taskName)
    {
        Debug.Log(taskName + " 任务完成");
        completedNum++;
        if(completedNum==tasks.Count)
        {
            Debug.Log("所有任务下载完成,启动检测更新");
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
    }

    private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Debug.Log("检测更新");
        for(int i=0;i<tasks.Count;i++)
        {
            DownloadTaskAbstract task = tasks[i];
            if(task.CheckUpdate()==true)
            {
                Debug.Log("内容发生改变，需要更新");
                completedNum--;
                task.Start();
            }
            else
            {
                Debug.Log("内容一致，不需要更新");
            }
        }
    }

    public static void StartAllTasks()
    {
        for(int i=0;i<tasks.Count;i++)
        {
            tasks[i].Start();
        }
    }

    public static DownloadTaskAbstract GetTaskByName(string name)
    {
        DownloadTaskAbstract task = tasks.Find(a => a.TaskName == name);
        return task;
    }

    public static void Reset()
    {
        tasks.Clear();
        timer.Stop();
        completedNum = 0;
    }

}
public enum TaskType
{
    JsonAPIType
}