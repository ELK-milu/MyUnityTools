using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlusbeQuickPlugin.HttpService;
using System;
using Plusbe.Config;

public static class HttpServer
{
    public static UnityAction CallHomeEvents;
    public static UnityAction<string> CallContentEvents;

    public static void Init()
    {
        //创建标准服务（Http接口调用）和网页调试服务
        HttpServiceManager.CreateStandardHttpService(AppConfig.Instance.GetValueByKeyInt("Port"));
        HttpServiceManager.CreateWebDebugHttpService(8066);

        //Pad标准中控接口
        StandardZKPadAPI.Instance.ColumnPlayAt += PlayAtColumn;
        StandardZKPadAPI.Instance.ContentPlayAt += PlayAtContent;
        StandardZKPadAPI.Instance.CreateByCloudWeb();

       
        HttpServiceManager.StandardService.RegisterCallCommand("返回", "act,states", "unity,home", "object", CallHome);
    }

    private static void CallHome(string[] keyValues)
    {
        CallHomeEvents?.Invoke();
    }

    private static void PlayAtColumn(PlusbeWebColumn column)
    {
        Debug.Log(column.Name);
    }

    private static void PlayAtContent(PlusbeWebContent content)
    {
        Debug.Log(content.Title);
        CallContentEvents?.Invoke(content.ID.ToString());
    }
}
