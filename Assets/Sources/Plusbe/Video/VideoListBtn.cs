/****************************************************
    文件：VideoListBtn.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2021/8/5 14:25:40
	功能：视频播放器列表按钮管理
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class VideoListBtn : MonoBehaviour 
{
    public Text videoName;
    public int Index;

    public void SetBtnData(string videoName,int index)
    {
        this.videoName.text = videoName;
        this.Index = index;
    }

}