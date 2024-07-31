using AppCustom;
using Plusbe.Core;
using Plusbe.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowMode : MonoBehaviour
{
    private static WindowMode windowMode;

    #region 静态属性
    private const uint SWP_SHOWWINDOW = 64u;

    private const uint SWP_NOZORDER = 4u;

    private const int GWL_STYLE = -16;

    private const int WS_BORDER = 1;

    private const int WS_POPUP = 8388608;

    private const int WS_NOBORDER = 268435456;

    public static int SW_RESTORE = 9;
    public static int SW_MINIMIZE = 6;
    private const int SW_HIDE = 0;  //隐藏任务栏

    private static int HWND_TOPMOST = -1;
    private static int HWND_TOP = 0;

    ////hWndInsertAfter 参数可选值:
    //HWND_TOP = 0;
    //{ 在前面}
    //HWND_BOTTOM = 1;
    //{ 在后面}
    //HWND_TOPMOST = HWND(-1);
    //{ 在前面, 位于任何顶部窗口的前面}
    //HWND_NOTOPMOST = HWND(-2);
    //{ 在前面, 位于其他顶部窗口的后面}

    #endregion

    #region win api
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
    #endregion

    public static WindowMode Instance
    { 
        get
        {
            if (windowMode == null)
            {
                Init();
            }

            return windowMode;
        }
    }

    private static void Init()
    {
        windowMode = ApplicationManager.Instance.gameObject.AddComponent<WindowMode>();
        Screen.SetResolution(ResolutionData.Instance.w, ResolutionData.Instance.h, ResolutionData.Instance.full);

        PlusbeCommandCenter.s_show_min += windowMode.HideApp;
        PlusbeCommandCenter.s_show_max += windowMode.ShowApp;

        //windowMode.SetWindowMode();
    }

    public void SetWindowMode()
    {
        //if (ResolutionData.Instance.type == MaxType.Custom || ResolutionData.Instance.type == MaxType.Max || ResolutionData.Instance.type == MaxType.CustomReset)
        //{
        //    SetWindowLater();
        //}

        if (ResolutionData.Instance.type == MaxType.Custom || ResolutionData.Instance.type == MaxType.Max )
            SetWindowLater();

        if (ResolutionData.Instance.type == MaxType.CustomReset)
            SetWindowModeLoop();
    }

    /// <summary>
    /// 窗体重复定位到指定位置
    /// 使用情况，在有些融合，led等信号传输过程中存在不稳的情况导致初始化的窗体位置发生变化
    /// </summary>
    private void SetWindowModeLoop()
    {
        StartCoroutine(DoWindowLoop());
    }

    private IEnumerator DoWindowLoop()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            UpdateWindowMode();
            yield return new WaitForSeconds(10);
        }
    }

    private void SetWindowLater()
    {
        StartCoroutine(DoWindowLater());
    }

    private IEnumerator DoWindowLater()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateWindowMode();


        if (ResolutionData.Instance.autoHide)
        {
            yield return new WaitForSeconds(1f);
            HideApp();
        }

    }


    private void UpdateWindowMode()
    {
        if (!Application.isEditor)
        {
            Cursor.visible = !ResolutionData.Instance.hideCursor;
            IntPtr intPtr = WindowMode.FindWindow(null, Application.productName);
            WindowMode.SetWindowLong(intPtr, -16, 268435456);
            WindowMode.SetWindowPos(intPtr, new IntPtr(ResolutionData.Instance.topMost ? HWND_TOPMOST : HWND_TOP), ResolutionData.Instance.x, ResolutionData.Instance.y, ResolutionData.Instance.w, ResolutionData.Instance.h, 64u);
        }
    }

    #region 显示隐藏

    public void ShowApp()
    {
        if (!Application.isEditor)
        {
            IntPtr hwnd = WindowMode.FindWindow(null, Application.productName);
            ShowWindow(hwnd, SW_RESTORE);
        }
    }

    public void HideApp()
    {
        if (!Application.isEditor)
        {
            IntPtr hwnd = WindowMode.FindWindow(null, Application.productName);
            ShowWindow(hwnd, SW_MINIMIZE);
        }
    }

    public void ShowApp(string name)
    {
        if (!Application.isEditor)
        {
            IntPtr hwnd = WindowMode.FindWindow(null, name);
            ShowWindow(hwnd, SW_RESTORE);
        }
    }

    public void HideApp(string name)
    {
        if (!Application.isEditor)
        {
            IntPtr hwnd = WindowMode.FindWindow(null, name);
            ShowWindow(hwnd, SW_MINIMIZE);
        }
    }

    #endregion

    #region 显示隐藏任务栏

    public void ShowTaskbar()
    {
        if (!Application.isEditor)
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
            ShowWindow(FindWindow("Button", null), SW_RESTORE);
        }
        
    }

    public void HideTaskBar()
    {
        if (!Application.isEditor)
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);
            ShowWindow(FindWindow("Button", null), SW_HIDE);
        }
        
    }

    #endregion
}
