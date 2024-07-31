using UnityEngine;

public class Debug2
{
    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    public static void Log(string msg, Color color)
    {
        //Debug.Log("<color=>" + color.ToString() + msg + "</color>");
    }

    public static void LogRed(string msg)
    {
        Debug.Log("<color=red>" + msg + "</color>");
    }

    public static void LogYellow(string msg)
    {
        Debug.Log("<color=yellow>" + msg + "</color>");
    }

    public static void LogBlue(string msg)
    {
        Debug.Log("<color=blue>" + msg + "</color>");
    }

    public static void LogGreen(string msg)
    {
        Debug.Log("<color=green>" + msg + "</color>");
    }

    
}