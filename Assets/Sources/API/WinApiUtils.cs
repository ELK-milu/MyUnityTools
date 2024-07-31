using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PlusbeQuickPlugin.API
{
    class WinApiUtils
    {
        #region win api
        [DllImport("user32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public const int WM_CLOSE = 0x10;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        #endregion

        /// <summary>
        /// 查找窗口句柄
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns></returns>
        public static IntPtr API_FindWindow(string lpClassName, string lpWindowName)
        {
            return FindWindow(lpClassName, lpWindowName);
        }
        public static bool API_GetWindowRect(IntPtr hwnd, ref RECT lpRect)
        {
            return GetWindowRect(hwnd, ref lpRect);
        }
        public static IntPtr API_FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow)
        {
            return FindWindowEx(hwndParent, hwndChildAfter, lpszClass, lpszWindow);
        }

        public enum NCmdShow : int
        {
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10
        }
        /// <summary>
        /// 控制句柄窗口的显示方式，隐藏，最大化，最小化，默认，还原等等
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        public static int API_ShowWindow(IntPtr hwnd, NCmdShow nCmdShow)
        {
            return ShowWindow(hwnd, (int)nCmdShow);
        }

        /// <summary>
        /// 设置窗口样式（无边框等）
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nindex"></param>
        /// <param name="dwNewLong"></param>
        public static void API_SetWindowLong(IntPtr hwnd,int nindex,int dwNewLong)
        {
            SetWindowLong(hwnd, nindex, dwNewLong);
        }

        public enum UFlags : uint
        {
            SWP_NOSIZE = 1,//忽略 cx、cy, 保持大小
            SWP_NOMOVE = 2,//忽略 X、Y, 不改变位置
            SWP_NOZORDER = 4,//忽略 hWndInsertAfter, 保持 Z 顺序
            SWP_NOREDRAW = 8,//不重绘
            SWP_NOACTIVATE = 0x10,//不激活
            SWP_FRAMECHANGED = 0x20,//强制发送 WM_NCCALCSIZE 消息, 一般只是在改变大小时才发送此消息
            SWP_SHOWWINDOW = 0x40,//显示窗口
            SWP_HIDEWINDOW = 0x80//隐藏窗口 后面还有更多选项不列举了。。。= =
        }

        public static void API_SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UFlags uFlags)
        {
            SetWindowPos(hwnd, hWndInsertAfter, X, Y, cx, cy, (uint)uFlags);
        }

        /// <summary>
        /// 发送信息到句柄窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        public static void API_SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam)
        {
            SendMessage(hwnd, wMsg, wParam, lParam);
        }

        /// <summary>
        /// 显示任务栏
        /// </summary>
        public static void API_ShowTaskBar()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), (int)NCmdShow.SW_RESTORE);
        }
        
        /// <summary>
        /// 隐藏任务栏
        /// </summary>
        public static void API_HideTaskBar()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), (int)NCmdShow.SW_HIDE);
        }
    }
}

