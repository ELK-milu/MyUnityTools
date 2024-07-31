using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



public class DownloaderDataAnalyzer
{
    private List<DownloaderDataModle> downloadDataList = new List<DownloaderDataModle>();
    private string downloadJsonPath;

    public void AnalyzeJson(string jsonPath)
    {
        downloadJsonPath = jsonPath;
        string jsonContent = "";
        using (StreamReader streamReader = new StreamReader(jsonPath, Encoding.UTF8))
        {
            jsonContent = streamReader.ReadToEnd();
            streamReader.Close();
        }
        if(jsonContent=="")
        {
            Debug.Log("解析的json内容为空");
        }
        AnalyzeData(jsonContent);
    }

    /// <summary>
    /// 将json内容转为数据模型并存入列表
    /// </summary>
    /// <param name="jsonContent"></param>
    private void AnalyzeData(string jsonContent)
    {
        downloadDataList.Clear();
        JObject jo = JsonConvert.DeserializeObject(jsonContent) as JObject;
        JArray jArray = jo["list"] as JArray;
        //Debug.Log("<color=#ffffcc>" + jArray.Count+"</color>");
        foreach(JObject obj in jArray)
        {
            DownloaderDataModle dataModle = new DownloaderDataModle();
            dataModle.EatIt(obj);
            downloadDataList.Add(dataModle);
        }
    }

    /// <summary>
    /// 匹配所有指定标签的数据
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public List<DownloaderDataModle> GetDatasByTag(string tagName)
    {
        List<DownloaderDataModle> result = new List<DownloaderDataModle>();
        for(int i=0;i<downloadDataList.Count;i++)
        {
            DownloaderDataModle dataModle = downloadDataList[i];
            if(dataModle.TagName==tagName)
            {
                result.Add(dataModle);
            }
        }
        return result;
    }

    /// <summary>
    /// 匹配所有指定Title的数据
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public List<DownloaderDataModle> GetDatasByTitle(string title)
    {
        List<DownloaderDataModle> result = new List<DownloaderDataModle>();
        for (int i = 0; i < downloadDataList.Count; i++)
        {
            DownloaderDataModle dataModle = downloadDataList[i];
            if (dataModle.Title == title)
            {
                result.Add(dataModle);
            }
        }
        return result;
    }

    /// <summary>
    /// 匹配所有该类型的数据
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public List<DownloaderDataModle> GetDatasByType(DownloadDataType dataType)
    {
        List<DownloaderDataModle> result = new List<DownloaderDataModle>();
        for (int i = 0; i < downloadDataList.Count; i++)
        {
            DownloaderDataModle dataModle = downloadDataList[i];
            if (dataModle.FileType == dataType)
            {
                result.Add(dataModle);
            }
        }
        return result;
    }




}
