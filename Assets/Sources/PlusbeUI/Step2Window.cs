using Plusbe.AppManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step2Window : UIWindowBase
{

    public override void OnOpen()
    {
        //base.OnOpen();
        AddOnClickListener("btn_back", OnClickBack);
    }


    public void OnClickBack(InputUIOnClickEvent e)
    {
        //Debug.Log("OnClickBack");
        ApplicationStatusManager.EnterStatus<FirstStatus>();
    }
}
