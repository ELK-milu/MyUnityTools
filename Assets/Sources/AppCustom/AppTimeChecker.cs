using Plusbe.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppTimeChecker : MonoBehaviour
{
    private static AppTimeChecker instance;

    private bool isShowTip;
    private string showTip;

    public static AppTimeChecker Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ApplicationManager.Instance.gameObject.AddComponent<AppTimeChecker>();
            }

            return instance;
        }
    }

    void OnGUI()
    {
        if (isShowTip)
        {
            GUI.Label(new Rect(0,0,1000,1000),showTip);
        }
    }

    public void CheckKey()
    {
        StartCoroutine(ToCheckDateTime());
    }

    private IEnumerator ToCheckDateTime()
    {
        yield return new WaitForSeconds(1f);

        DateTime time = DateTime.Parse(AppConfig.Instance.GetValueByKey("KeyTime"));

        TimeSpan span = time - DateTime.Now;

        //Debug.Log(time.ToString("yyyyMMdd") + "," + span.Days);

        //showTip = "试用软件," + span.Days + "天后即将过期，请联系管理人员延长有效期！";
        //Debug.Log(showTip);

        if (span.Days < 0)
        {
            Application.Quit();
        }
        else if (span.Days < 10)
        {
            //Text text = Instance(txtx)

            //txtTipDate.gameObject.SetActive(true);
            //txtTipDate.text = "试用软件," + span.Days + "天后即将过期，请联系管理人员延长有效期！";
            isShowTip = true;
            showTip = "试用软件," + span.Days + "天后即将过期，请联系管理人员延长有效期！";
            Debug.Log(showTip);
        }
        //else
        //{
        //    Application.Quit();
        //}
    }
}
