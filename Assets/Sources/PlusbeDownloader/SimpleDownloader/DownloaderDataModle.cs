using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

public enum DownloadDataType
{
    PPT,
    MOVIE,
    PIC,
    UNKNOW
}

public class DownloaderDataModle
{
    public int ID;
    public string Title;
    public string Files;
    public int Tag;
    public string Word;
    public DownloadDataType FileType;
    public string TagName;

    public void EatIt(JObject obj)
    {
        ID = Convert.ToInt32(obj["ID"]);
        Title = obj["Title"].ToString();
        Files = obj["Files"].ToString();
        Tag = Convert.ToInt32(obj["Tag"]);
        Word = obj["PicWord"].ToString();
        int i = Convert.ToInt32(obj["FileType"]);
        if (i == 1)
        {
            FileType = DownloadDataType.MOVIE;
        }
        else if (i == 3)
        {
            FileType = DownloadDataType.PPT;
        }
        else if (i == 4)
        {
            FileType = DownloadDataType.PIC;
        }
        else
        {
            FileType = DownloadDataType.UNKNOW;
        }
        TagName = obj["ChannelName"].ToString();
    }
}
