/****************************************************
    文件：SendHttpMessage.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2020/10/14 17:18:36
	功能：发送 http 命令
*****************************************************/

using Plusbe.Config;
using Plusbe.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendHttpMessage : MonoBehaviour 
{


    

    /// <summary>
    /// 发送控制命令函数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerator SendHttpCommand(string act,string obj,string states)
    {
        Debug.Log("发送命令：" + "http://" + GlobalSetting.ToIP + ":" + GlobalSetting.ToPort + "/?act="+act+"&object=" + obj+ "&states="+states);

        string command = "http://" + GlobalSetting.ToIP + ":" + GlobalSetting.ToPort + "/?act=" + act + "&object=" + obj + "&states=" + states;
        using (UnityWebRequest www = UnityWebRequest.Get(command))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("download:" + www.downloadHandler.text);
            }
        }
    }
}