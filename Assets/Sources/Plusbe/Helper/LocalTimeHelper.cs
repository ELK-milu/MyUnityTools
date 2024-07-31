using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


public class LocalTimeHelper
{
    [DllImport("Kernel32")]
    public static extern void GetLocalTime(ref SystemTime lpSystemTime);

    [DllImport("Kernel32")]
    public static extern bool SetLocalTime(ref SystemTime lpSystemTime);


    public static void SetTempTime()
    {
        SetTempTime(new DateTime(2020, 2, 2));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date">2020/02/02</param>
    public static void SetTempTime(string tempTime)
    {
        DateTime time;
        if (DateTime.TryParse(tempTime, out time))
            SetTempTime(time);
        else
            Debug.Log("非法字符串:"+ tempTime);
    }

    public static void SetTempTime(DateTime tempTime)
    {
        Debug.Log("SetFaceTime:" + SetLocalTime(tempTime));
    }

    /// <summary>
    /// 获取本地时间
    /// </summary>
    /// <returns></returns>
    private static DateTime GetLocalTime()
    {
        SystemTime sysTime = new SystemTime();
        GetLocalTime(ref sysTime);
        return SystemTime2DateTime(sysTime);
    }

    /// <summary>
    /// 设置本地时间
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private static bool SetLocalTime(DateTime dateTime)
    {
        SystemTime sysTime = DateTime2SystemTime(dateTime);
        return SetLocalTime(ref sysTime);

        //if (grantPrivilege(SE_SYSTEMTIME_NAME))
        //{
        //    // 授权成功
        //    SystemTime sysTime = DateTime2SystemTime(dateTime);
        //    bool success = SetLocalTime(ref sysTime);
        //    if (!revokePrivilege(SE_SYSTEMTIME_NAME))
        //    {
        //        // 撤权失败
        //    }
        //    return success;
        //}
        //// 授权失败
        //return false;
    }

    /// <summary>
    /// 将SystemTime转换为DateTime
    /// </summary>
    /// <param name="sysTime"></param>
    /// <returns></returns>
    private static DateTime SystemTime2DateTime(SystemTime sysTime)
    {
        return new DateTime(sysTime.year, sysTime.month, sysTime.day, sysTime.hour, sysTime.minute, sysTime.second, sysTime.milliseconds);
    }

    /// <summary>
    /// 将DateTime转换为SystemTime
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private static SystemTime DateTime2SystemTime(DateTime dateTime)
    {
        SystemTime sysTime = new SystemTime();
        sysTime.year = Convert.ToUInt16(dateTime.Year);
        sysTime.month = Convert.ToUInt16(dateTime.Month);
        sysTime.day = Convert.ToUInt16(dateTime.Day);
        sysTime.hour = Convert.ToUInt16(dateTime.Hour);
        sysTime.minute = Convert.ToUInt16(dateTime.Minute);
        sysTime.second = Convert.ToUInt16(dateTime.Second);
        sysTime.milliseconds = Convert.ToUInt16(dateTime.Millisecond);
        return sysTime;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        [MarshalAs(UnmanagedType.U2)]
        internal ushort year; // 年
        [MarshalAs(UnmanagedType.U2)]
        internal ushort month; // 月
        [MarshalAs(UnmanagedType.U2)]
        internal ushort dayOfWeek; // 星期
        [MarshalAs(UnmanagedType.U2)]
        internal ushort day; // 日
        [MarshalAs(UnmanagedType.U2)]
        internal ushort hour; // 时
        [MarshalAs(UnmanagedType.U2)]
        internal ushort minute; // 分
        [MarshalAs(UnmanagedType.U2)]
        internal ushort second; // 秒
        [MarshalAs(UnmanagedType.U2)]
        internal ushort milliseconds; // 毫秒
    }
}
