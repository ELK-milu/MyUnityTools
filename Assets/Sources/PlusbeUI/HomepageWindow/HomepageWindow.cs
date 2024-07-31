using UnityEngine;
using Plusbe.Video;
using UnityEngine.UI;
using System.Collections;
using System;
using Plusbe.Core;
using PlusbeQuickPlugin.HttpService;

public class HomepageWindow : UIWindowBase 
{
    public static HomepageWindow Instance;
    public AVProVideoPlayer _videoPlayerBg;

    public override void OnInit()
    {
        Instance = this;

        InitVideoPlayer();

        AddMethodListener();
    }

    private void InitVideoPlayer()
    {
        _videoPlayerBg.Init();
        _videoPlayerBg.OpenVideo(GlobalSetting.SkinPath + "bg.mp4");
    }

    private void AddMethodListener()
    {
        PlusbeWebColumn[] columns = PlusbeWebV3.DefaultLoader.GetChildColumnsByPath("测试栏目");
    }

}