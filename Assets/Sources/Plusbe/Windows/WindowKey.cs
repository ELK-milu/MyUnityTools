using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WindownKey
{
    public static void Init()
    {
        ApplicationManager.s_OnApplicationUpdate += OnUpdate;
    }

    static void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Debug.Log("Quit By Press Esc");
            Application.Quit();
        }

        // if (Input.GetKeyDown(KeyCode.Alpha1))
        //    QualitySettings.SetQualityLevel(0, true);
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //    QualitySettings.SetQualityLevel(1, true);
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    QualitySettings.SetQualityLevel(2, true);
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //    QualitySettings.SetQualityLevel(3, true);
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //    QualitySettings.SetQualityLevel(4, true);
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //    QualitySettings.SetQualityLevel(5, true);
    }
    
}

