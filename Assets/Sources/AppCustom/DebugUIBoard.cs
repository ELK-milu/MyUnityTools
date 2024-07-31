using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIBoard : MonoBehaviour {

    public void OnClick1()
    {
        UIManager.OpenUIWindow<DemoWindow>(true);
        for (int i = 0; i < 5; i++)
        {
            
        }
    }

    public void OnClick2()
    {
        UIManager.CloseUIWindow<DemoWindow>();
    }

    public void OnClick3()
    {
        UIManager.OpenUIWindow<Step2Window>(true);        
    }
}
