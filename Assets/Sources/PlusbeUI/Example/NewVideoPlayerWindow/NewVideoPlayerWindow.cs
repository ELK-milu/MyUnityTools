using UnityEngine;
using System.Collections;
using Video;
using Plusbe.Core;
using Plusbe.Video;
using System.Collections.Generic;

public class NewVideoPlayerWindow : UIWindowBase 
{

    public GameObject videoView;
    private IVideoPlayer videoPlayer;

    public GameObject videoView2;
    private IVideoPlayer videoPlayer2;


    private int currIndex;
    private int totalIndex;
    private List<string> videos;

    public override void OnInit()
    {
        videoPlayer = videoView.GetComponent<AVProVideoPlayer>();
        videoPlayer.Init();

        videoPlayer2 = videoView2.GetComponent<AVProVideoPlayer>();
        videoPlayer2.Init();
    }

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("btn_open", OnClickOpen);
        AddOnClickListener("btn_play", OnClickPlay);
        AddOnClickListener("btn_pause", OnClickPause);
        AddOnClickListener("btn_replay", OnClickReplay);
        AddOnClickListener("btn_stop", OnClickStop);

        videos = new List<string>();

        videos.Add(GlobalSetting.DataPath + "UploadFiles/Hello.mp4");
        videos.Add(GlobalSetting.DataPath + "UploadFiles/HelloBuck.mp4");
        videos.Add(GlobalSetting.DataPath + "UploadFiles/helloTransparent.mp4");
        videos.Add(GlobalSetting.DataPath + "UploadFiles/4k.mkv");


        currIndex = 0;
        totalIndex = videos.Count;

        OnClickOpen(null);
        //videoPlayer.OpenVideo(GlobalSetting.DataPath + "UploadFiles/HelloBuck.mp4");
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画 调用 base.EnterAnim 表示进入动画播放完成
    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画 调用 base.ExitAnim 表示退出动画播放完成
    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    private void OnClickOpen(InputUIOnClickEvent e)
    {
        string url = videos[++currIndex % totalIndex];
        videoPlayer.OpenVideo(url);

        videoPlayer2.OpenVideo(url);

        Debug.Log(url);
    }

    private void OnClickPlay(InputUIOnClickEvent e)
    {
        videoPlayer.Resume();
    }

    private void OnClickPause(InputUIOnClickEvent e)
    {
        videoPlayer.Pause();
    }

    private void OnClickReplay(InputUIOnClickEvent e)
    {
        videoPlayer.Rewind();
    }

    private void OnClickStop(InputUIOnClickEvent e)
    {
        videoPlayer.Stop();
    }
}