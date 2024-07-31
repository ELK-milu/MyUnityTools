using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo.Demos;
using Plusbe.Core;
using System;
using UnityEngine.UI;
using Plusbe.Config;
using AppCustom;


/*
 1.常规控制功能 
 *  播放，暂停，重播，停止，点播
 *  控制 loop，mute，rate
 *  循环模式，列表循环，单个循环
 
 */
public class AVVideoPlayerWindow : UIWindowBase 
{

    //public VCR videoPlayer;

    private float lastVideoTime = 0;

    public override void OnInit()
    {
        //videoPlayer.Init();

        //videoPlayer.videoEnd = OnVideoEndEvent;
        //videoPlayer.videoFirstFrame = OnFirstFrameReadyEvent;
        //videoPlayer.videoFinishSeeking = OnFinishedSeekingEvent;
    }


    //public Action videoEnd;
    //public Action videoFirstFrame;
    //public Action videoFinishSeeking;

    private void OnFirstFrameReadyEvent()
    {
        //SwapPlayers();
        //Debug.Log(lastVideoTime + "," + (110 * 1000 - videoPlayer.GetTimeMs()));
        VCRSeekingTemp(110 * 1000 - lastVideoTime );
    }

    private void OnVideoEndEvent()
    {

    }

    private void OnFinishedSeekingEvent()
    {

    }

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("btn_next", OnClickNext);
        AddOnClickListener("btn_play", OnClickPlay);
        AddOnClickListener("btn_pause", OnClickPause);
        AddOnClickListener("btn_replay", OnClickReplay);
        AddOnClickListener("btn_stop", OnClickStop);

        OpenZK();

        OnClickNext(null);
    }

    private void OpenZK()
    {
        PlusbeCommandCenter.s_play += ZKPlay;
        PlusbeCommandCenter.s_pause += ZKPause;
        PlusbeCommandCenter.s_replay += ZKReplay;


        PlusbeCommandCenter.s_open += ZKOpeVideo;
    }

    private void CloseZK()
    {
        PlusbeCommandCenter.s_play -= ZKPlay;
        PlusbeCommandCenter.s_pause -= ZKPause;
        PlusbeCommandCenter.s_replay -= ZKReplay;

        PlusbeCommandCenter.s_open -= ZKOpeVideo;
    }

    private void ZKOpeVideo(string obj)
    {
        int index = Convert.ToInt32(obj);

        string path = GlobalSetting.DataPath + JsonDataManager.GetFileByIndex(index);

        if (!string.IsNullOrEmpty(path))
        {
            //videoPlayer.OnOpenVideoFile(path);
        }
    }

    private void ZKPlay()
    {
        OnClickPlay(null);
    }

    private void ZKPause()
    {
        OnClickPause(null);
    }

    private void ZKReplay()
    {
        OnClickReplay(null);
    }

    private void ZKNext(string obj)
    {
        OnClickNext(null);
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
        //videoPlayer.OnStop();
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));

        yield return new WaitForEndOfFrame();
    }

    public void OpenFile(string filePath)
    {
        VCROpenFile(filePath);
    }

    public void OnClickNext(InputUIOnClickEvent e)
    {
        VCRTestOpenFile();
    }

    public void OnClickStop(InputUIOnClickEvent e)
    {
        VCRStop();
    }

    public void OnClickPlay(InputUIOnClickEvent e)
    {
        VCRPlay();
    }

    public void OnClickPause(InputUIOnClickEvent e)
    {
        VCRPause();
    }
    public void OnClickReplay(InputUIOnClickEvent e)
    {
        VCRReplay();
    }

    public void OnClickLoop(Toggle go)
    {
        VCRSetLoop(go.isOn);
    }

    #region VCR对外接口参考

    public void VCRTestOpenFile()
    {
        //lastVideoTime = videoPlayer.GetTimeMs();
        //videoPlayer.OnOpenVideoFile();
    }

    public void VCROpenFile(string path)
    {
        //lastVideoTime = videoPlayer.GetTimeMs();
        //videoPlayer.OnOpenVideoFile(path);
    }

    public void VCRPlay()
    {
        //videoPlayer.OnPlayButton();
    }

    public void VCRPause()
    {
        //videoPlayer.OnPauseButton();
    }

    public void VCRReplay()
    {
        //videoPlayer.OnRewindButton();
    }

    public void VCRStop()
    {
        //videoPlayer.OnStop();
    }

    public void VCRSetLoop(bool loop)
    {
        //videoPlayer.SetLoop(loop);
    }

    public void VCRSetRate(float rate)
    {
        //videoPlayer.SetRate(rate);
    }

    public void VCRSeeking(float time)
    {
        //videoPlayer.Seek(time);
    }

    public void VCRSeekingTemp(float time)
    {
        //videoPlayer.SeekTemp(time);
    }

    #endregion
}