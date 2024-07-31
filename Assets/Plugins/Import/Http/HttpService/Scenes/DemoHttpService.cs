using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlusbeQuickPlugin.HttpService;
using System;
using UnityEngine.UI;

public class DemoHttpService : MonoBehaviour
{
    public Text text;

    void Start()
    {
        //请先用下载下载数据 192.168.60.250:8886 192.168.demo.http 1847

        //创建标准服务（Http接口调用）和网页调试服务
        HttpServiceManager.CreateStandardHttpService(8030);//从配置文件获取
        HttpServiceManager.CreateWebDebugHttpService(8066);//默认8066，同台主机上多个unity客户端时递增8067,8068,8069
        //自定义Http接口(纯调用和数据返回)
        HttpServiceManager.StandardService.RegisterCommand("显示文本", "ShowText", ShowText);
        HttpServiceManager.StandardService.RegisterResponseCommand("计算加法", "act", "add", "a,b", Add);

        //使用后台数据生成标准Pad点播接口
        //json接口 127.0.0.1:8030/?act=unity&states=json&object=
        StandardZKPadAPI.Instance.ColumnPlayAt += PlayAtColumn;
        StandardZKPadAPI.Instance.ContentPlayAt += PlayAtContent;
        StandardZKPadAPI.Instance.CreateByCloudWeb();

        //使用自定义栏目和内容点播
        //StandardZKPadAPI.Instance.AddTag(0, "银河系", -1);
        //StandardZKPadAPI.Instance.AddTag(1, "太阳系", 0);
        //StandardZKPadAPI.Instance.AddTag(2, "太阳", 1);
        //StandardZKPadAPI.Instance.AddTag(3, "地球", 1);
        //StandardZKPadAPI.Instance.AddTag(4, "土星", 1);
        //StandardZKPadAPI.Instance.AddTag(5, "火星", 1);

        //StandardZKPadAPI.Instance.AddContent(0, "地球内容A", 3);
        //StandardZKPadAPI.Instance.AddContent(1, "地球内容B", 3);
        //StandardZKPadAPI.Instance.AddContent(2, "地球内容B", 3);
        //StandardZKPadAPI.Instance.ColumnPlayAtID += PlayAtColumnID;
        //StandardZKPadAPI.Instance.ContentPlayAtID += PlayAtContentID;
        //StandardZKPadAPI.Instance.CreateToUse();

        //非标准接口和pad开发人员对接
    }

    private void PlayAtColumn(PlusbeWebColumn column)
    {
        ShowText(column.Name);
    }

    private void PlayAtContent(PlusbeWebContent content)
    {
        ShowText(content.Title);
    }

    private void PlayAtColumnID(int id)
    {
        ShowText(id.ToString());
    }

    private void PlayAtContentID(int id)
    {
        ShowText(id.ToString());
    }

    private string Add(string[] keyValues)
    {
        float a = int.Parse(keyValues[0]);
        float b = int.Parse(keyValues[1]);
        return (a + b).ToString();
    }

    private void ShowText(string txt)
    {
        text.text = txt.ToString();
    }

}
