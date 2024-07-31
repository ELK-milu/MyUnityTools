using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;
//using System.Windows.Forms;
/// <summary>
/// Desc:本地记录日志文件
/// _____
/// Auth:Zero
/// _____
/// Data:2019.1.17
/// _____
/// Ver:1.0.0.1
/// </summary>
public class DebugTrace
{
    private FileStream fileStream;
    private StreamWriter streamWriter;

    private bool isEditorCreate = false;//是否在编辑器中也产生日志文件
    private int showFrames = 1000;  //所有帧 

    #region instance
    private static readonly object obj = new object();
    private static DebugTrace m_instance;
    public static DebugTrace Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (obj)
                {
                    if (m_instance == null)
                        m_instance = new DebugTrace();
                }
            }
            return m_instance;
        }
    }
    #endregion

    private DebugTrace()
    {

    }



    /// <summary>
    /// 开启跟踪日志信息
    /// </summary>
    public void StartTrace()
    {
        if (Debug.unityLogger.logEnabled)
        {
            if (Application.isEditor)
            {
                //在编辑器中设置isEditorCreate==true时候产生日志
                if (isEditorCreate)
                {
                    CreateOutlog();
                }
            }
            //不在编辑器中 是否产生日志由  Debug.unityLogger.logEnabled 控制
            else
            {
                CreateOutlog();
            }
        }
    }
    private void Application_logMessageReceivedThreaded(string logString, string stackTrace, LogType type)
    {
        //  Debug.Log(stackTrace);  //打包后staackTrace为空 所以要自己实现
        if (type != LogType.Warning)
        {
            // StackTrace stack = new StackTrace(1,true); //跳过第二?（1）帧
            StackTrace stack = new StackTrace(true);  //捕获所有帧
            string stackStr = string.Empty;

            int frameCount = stack.FrameCount;  //帧数
            if (this.showFrames > frameCount) this.showFrames = frameCount;  //如果帧数大于总帧速 设置一下

            //如果堆栈帧数大于showFrames 就获取最后两帧 反之全部输出
            for (int i = stack.FrameCount - this.showFrames; i < stack.FrameCount; i++)
            {
                StackFrame sf = stack.GetFrame(i);  //堆栈爸爸
                                                    // 1:第一种    ps:GetFileLineNumber 在发布打包后获取不到
                stackStr += "at [" + sf.GetMethod().DeclaringType.FullName +
                            "." + sf.GetMethod().Name +
                            ".Line:" + sf.GetFileLineNumber() + "]\n            ";

                //或者直接调用tostring 显示数据过多 且打包后有些数据获取不到
                // stackStr += sf.ToString();
            }

            //或者 stackStr = stack.ToString();
            string content = string.Format("time: {0}   logType: {1}    logString: {2} \nstackTrace: {3} {4} ",
                                               DateTime.Now.ToString("HH:mm:ss"), type, logString, stackStr, "\r\n");
            streamWriter.WriteLine(content);
            streamWriter.Flush();
        }
        //if (type == LogType.Exception)
        //{
        //    Debug.LogError("程序异常");
        //    StackTrace stack = new StackTrace(true);  //捕获所有帧
        //    File.WriteAllText("错误日志.txt", logString + "\n" + stack.ToString());
        //    if (MessageBox.Show("程序出现异常 请联系管理员或者重新启动\n" + logString, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
        //    {
        //        Application.Quit();
        //    }
        //}
    }
    private void CreateOutlog()
    {
        if (!Directory.Exists(Application.dataPath + "/../" + "OutLog"))
            Directory.CreateDirectory(Application.dataPath + "/../" + "OutLog");
        string path = Application.dataPath + "/../OutLog" + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_log.txt";
        fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        streamWriter = new StreamWriter(fileStream);
        Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
    }

    /// <summary>
    /// 关闭跟踪日志信息
    /// </summary>
    public void CloseTrace()
    {
        Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
        streamWriter.Dispose();
        streamWriter.Close();
        fileStream.Dispose();
        fileStream.Close();
    }
    /// <summary>
    /// 设置选项
    /// </summary>
    /// <param name="logEnable">是否记录日志</param>
    /// <param name="showFrams">是否显示所有堆栈帧 默认显示所有帧 如果设为0 则显示所有帧</param>
    /// <param name="filterLogType">过滤 默认log级别以上</param>
    /// <param name="editorCreate">是否在编辑器中产生日志记录 默认不需要</param>
    public void SetLogOptions(bool logEnable, int showFrams = 0, LogType filterLogType = LogType.Log, bool editorCreate = false)
    {
        Debug.unityLogger.logEnabled = logEnable;
        Debug.unityLogger.filterLogType = filterLogType;
        isEditorCreate = editorCreate;
        this.showFrames = showFrams == 0 ? 1000 : showFrams;
    }

}
