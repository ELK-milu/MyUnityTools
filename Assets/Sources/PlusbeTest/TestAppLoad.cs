using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAppLoad : MonoBehaviour {

    public void Init()
    {
        UIManager.CreateUIWindow("DemoWindow");

        Debug.Log("init CreateUIWindow");


        UIManager.OpenUIWindow("DemoWindow");
        //UIManager.ShowUI("DemoWindow");

        //Debug.Log("init ShowUI");
    }
}
