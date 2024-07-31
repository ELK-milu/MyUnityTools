using UnityEngine;
using System.Collections;
using Plusbe.Encrypt;
using System;
using AppCustom;

public class TestEncryptionWindow : UIWindowBase 
{


    //UI打开的初始化请放在这里
    public override void OnOpen()
    {
        //string keyCode = KeyHelper.GetCheckKeyCode(publicKey + "12", SystemInfo.deviceUniqueIdentifier + "", "2020/2/23", "Test", 3);

        //Debug.Log(keyCode);

        //Debug.Log("测试 >> 无效key >> " + KeyHelper.ActiveCode(keyCode, publicKey, SystemInfo.deviceUniqueIdentifier, DateTime.Now, produceName, version).message);

        //Debug.Log("测试 >> " + KeyHelper.ActiveCode(KeyHelper.GetCheckKeyCode(publicKey, SystemInfo.deviceUniqueIdentifier + "", "2020/3/10", "Test", 3), publicKey, SystemInfo.deviceUniqueIdentifier, DateTime.Now, produceName, version).message);

        string keyCode = KeyHelper.GetActiveCode("2020/03/15");
        Debug.Log(keyCode);


        btn.Init(0,OnClickBtnBase);

        PlusbeCommandCenter.receiveMessage += OnReceiveMessage;

        PlusbeCommandCenter.SendMessage("asldkjaslkdjlaksdkjlsakdj");
    }

    private void OnReceiveMessage(object data)
    {
        Debug.Log("OnReceiveMessage:" + data.ToString());
    }

    private void OnClickBtnBase(ButtonBase obj)
    {
        Debug.Log("OnClickBtnBase");
    }

    public ButtonBase btn;


}