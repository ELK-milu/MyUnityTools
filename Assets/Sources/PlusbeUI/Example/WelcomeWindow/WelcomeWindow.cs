using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Plusbe.Welcome;
using Plusbe.Message;
using System.Collections.Specialized;
using Plusbe.Serialization;
using System;
using DG.Tweening;
using Plusbe.Drawing;
using Plusbe.Core;
using Plusbe.AppManager;
using AppCustom;
using System.Collections.Generic;
using Plusbe.Config;
using TMPro;

public class WelcomeWindow : UIWindowBase
{
    [Header("字体组件是否为TextMeshPro")]
    public bool isTMP;
    public RawImage rawBg;
    public Text txtContent;
    public TMP_Text txtContent_TMP;

    private int currLoc;
    private int totalLoc;

    private WelcomeMode playMode = WelcomeMode.Loop;
    private bool isPlaying;

    private List<string> pics;
    public override void OnInit()
    {
        WelcomeXml.Init();

        pics = new List<string>();
        pics.Add(GlobalSetting.DataPath + "Welcome/bg.png");
        pics.Add(GlobalSetting.DataPath + "Welcome/bg2.png");
        pics.Add(GlobalSetting.DataPath + "Welcome/bg3.png");

        BgUpdate("0");
        totalLoc = WelcomeXml.Instance.Count;
    }

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        isPlaying = true;        
        StartCoroutine(ShowNextWelcome());
        ShowNext();
        NotificationCenter.Instance.AddObserver(this, "UnityWelcome");
    }

    private IEnumerator ShowNextWelcome()
    {
        int time = AppConfig.Instance.GetValueByKeyInt("WelcomeTime");
        if (time == 0) time = 60000;
        print(time);
        while (isPlaying)
        {
            yield return new WaitForSeconds(time);
            ShowNext();
        }
    }
    private void ShowNext()
    {
        if (playMode == WelcomeMode.List)
        {
            currLoc = ++currLoc % (totalLoc == 0 ? 1 : totalLoc);
            PlayWelcome(currLoc);
        }
        else if (playMode == WelcomeMode.Loop)
        {
            PlayWelcome(currLoc);
        }
    }
    ////UI的进入动画 调用 base.EnterAnim 表示进入动画播放完成
    //public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    //{
    //    //AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
    //    //{
    //    //StartCoroutine(base.EnterAnim(animComplete, callBack, objs));
    //    //});

    //    yield return new WaitForEndOfFrame();
    //}
    ////UI的退出动画 调用 base.ExitAnim 表示退出动画播放完成
    //public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    isPlaying = false;
    //    //AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
    //    //{
    //    StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
    //    //});
    //    yield return new WaitForEndOfFrame();
    //}

    private void UnityWelcome(Notification notify)
    {
        string data = notify.data.ToString();
        NameValueCollection datas = MessageSerializerHelper.ParseHttpUrl(data);
        string type = datas["t"];
        string id = datas["id"];
        string title = datas["title"];
        ///title = !string.IsNullOrEmpty(title) ? HttpUtility.UrlDecode(title) : "";
        title = !string.IsNullOrEmpty(title) ? UnityEngine.Networking.UnityWebRequest.UnEscapeURL(title) : "";
        string size = datas["size"];
        string obj = datas["object"];
        switch (type)
        {
            case "add": Add(title, size); break;
            case "delete": Delete(id); break;
            case "update": UpdateXml(id, title, size); break;
            case "back": BackWelcome(); break;
            case "openindex": PlayWelcome(obj); break;
            case "loop": LoopWelcome(); break;
            case "bg": BgUpdate(obj); break;
            case "color": getTextColor(obj); break;
            default: break;
        }
    }

    /// <summary>
    /// 添加欢迎词
    /// </summary>
    /// <param name="title"></param>
    /// <param name="size"></param>
    private void Add(string title, string size)
    {
        WelcomeXml.Instance.Add(title, size);
        totalLoc = WelcomeXml.Instance.Count - 1;
        PlayWelcome(totalLoc);
    }
    /// <summary>
    /// 删除欢迎词
    /// </summary>
    /// <param name="id"></param>
    private void Delete(string id)
    {
        WelcomeXml.Instance.Delete(id);
    }
    /// <summary>
    /// 修改欢迎词
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="size"></param>
    private void UpdateXml(string id, string title, string size)
    {
        WelcomeXml.Instance.Update(id, title, size);
        PlayWelcome(id);
    }
    /// <summary>
    /// 返回欢迎词
    /// </summary>
    private void BackWelcome()
    {
        ApplicationStatusManager.EnterStatus<PlusbeWelcomeStatus>();
    }
    /// <summary>
    /// 点播欢迎词
    /// </summary>
    /// <param name="index"></param>
    private void PlayWelcome(string index)
    {
        PlayWelcome(Convert.ToInt32(index));
    }
    private void PlayWelcome(int index)
    {
        currLoc = index;
        ShowWelcome(WelcomeXml.Instance.GetTitle(currLoc), WelcomeXml.Instance.GetSize(currLoc));
    }

    
    private void ShowWelcome(string data, int size)
    {
        if (isTMP)
        {
            txtContent_TMP.transform.localPosition = new Vector3(txtContent.transform.localPosition.x, -1080, 0);
            txtContent_TMP.transform.DOLocalMoveY(180, 0.8f);
            txtContent_TMP.text = data;
            txtContent_TMP.fontSize = size;
        }
        else
        {
            txtContent.transform.localPosition = new Vector3(txtContent.transform.localPosition.x, -1080, 0);
            txtContent.transform.DOLocalMoveY(0, 0.8f);
            txtContent.text = data;
            txtContent.fontSize = size;
        }

    }
    /// <summary>
    /// 欢迎词循环切换
    /// </summary>
    void LoopWelcome()
    {
        if (playMode == WelcomeMode.List)
        {
            playMode = WelcomeMode.Loop;
        }
        else if (playMode == WelcomeMode.Loop)
        {
            playMode = WelcomeMode.List;
        }
    }
    /// <summary>
    /// 欢迎词背景切换
    /// </summary>
    /// <param name="str"></param>
    private void BgUpdate(string str)
    {
        rawBg.texture = PicHelper.LoadPicTexture(pics[Convert.ToInt32(str)]);
    }
    void getTextColor(string color)
    {
        if (isTMP)
        {
            switch (color)
            {
                case "red":
                    txtContent_TMP.color = Color.red;

                    break;
                case "yellow":
                    txtContent_TMP.color = Color.yellow;
                    break;
                case "blue":
                    txtContent_TMP.color = Color.blue;
                    break;
                case "green":
                    txtContent_TMP.color = Color.green;
                    break;
                case "white":
                    txtContent_TMP.color = Color.white;
                    break;
                default: break;
            }
        }
        else
        {
            switch (color)
            {
                case "red":
                    txtContent.color = Color.red;
                    break;
                case "yellow":
                    txtContent.color = Color.yellow;
                    break;
                case "blue":
                    txtContent.color = Color.blue;
                    break;
                case "green":
                    txtContent.color = Color.green;
                    break;
                case "white":
                    txtContent.color = Color.white;
                    break;
                default: break;
            }
        }



    }
    public enum WelcomeMode
    {
        Loop,
        List
    }


}