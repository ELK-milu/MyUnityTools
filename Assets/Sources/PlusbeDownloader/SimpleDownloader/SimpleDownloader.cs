using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Plusbe.Message;
using Plusbe.Core;

public class SimpleDownloader : MonoBehaviour
{
    public string taskName;
    public string pid;
    public string myIP;
    public string host;
    private JsonDownloadTask task;
    private Text text;
    private Text curr;
    private Text total;
    private Slider slider;

    public delegate void DownloadCom();
    public event DownloadCom DownLoadComEvent;
    public DownloaderDataAnalyzer Analyzer = new DownloaderDataAnalyzer();
    public DownloadStatus Status { get { return task.Status; } }

    private void Awake()
    {
        curr = transform.Find("Curr").GetComponent<Text>();
        total = transform.Find("Total").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();
        slider = transform.Find("Slider").GetComponent<Slider>();
        NotificationCenter.Instance.AddObserver(this, "Show");
        NotificationCenter.Instance.AddObserver(this, "Hide");
    }
    private void Start()
    {
        DownloaderManager.CreateDownloadTask(host, pid, myIP, taskName);
        task = (JsonDownloadTask)DownloaderManager.GetTaskByName(taskName);
        task.CompleteEvent.AddListener(TaskComplete);
        task.StartEvent.AddListener(TaskStart);
        task.Start();
    }

    private void TaskComplete(string taskName)
    {
        Debug.Log("任务 " + taskName + " 完成");
        NotificationCenter.Instance.PostNotification(this, "Hide");
    }
    private void TaskStart()
    {
        NotificationCenter.Instance.PostNotification(this, "Show");
    }

    private void Update()
    {
        ShowProcess();
    }

    private void Hide(Notification notification)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.DOFade(0, .6f);
        Analyzer.AnalyzeJson(GlobalSetting.DataPath + taskName + "/Config.json");
        DownLoadComEvent?.Invoke();
    }
    private void Show(Notification notification)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.DOFade(1, .6f);
    }

    private void ShowProcess()
    {
        text.text = task.Info.CurrFileName;
        slider.maxValue = task.Info.TotalBytes;
        slider.value = task.Info.DownloadBytes;
        curr.text = (task.Info.CurrIndex + 1).ToString();
        total.text = task.Info.TotalTaskNum.ToString();
    }

    private void OnApplicationQuit()
    {
        DownloaderManager.Reset();
    }



}
