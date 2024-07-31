using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Develop
{
    public class GUIConsoleManager
    {
        public delegate void OnUpdateCallback();
        public delegate void OnGUICallback();

        static public OnUpdateCallback onUpdateCallback = null;
        static public OnGUICallback onGUICallback = null;
        static public OnGUICallback onGUICloseCallback = null;

        const int margin = 3;
        const int offset = 0;
        //static Rect windowRect = new Rect(margin + Screen.width * 0.6f - offset, margin, Screen.width * 0.6f - (2 * margin) + offset, Screen.height - (2 * margin));

        private static bool showGUI;

        public static void Init()
        {
            ApplicationManager.s_OnApplicationUpdate += OnUpdate;
            ApplicationManager.s_OnApplicationOnGUI += OnGUI;

            Application.logMessageReceivedThreaded += HandleLog;

            FPSCounterHelper.Init();
        }

        private static void OnUpdate()
        {
            if (onUpdateCallback != null) onUpdateCallback();

            if (Input.GetKeyDown(KeyCode.F1)) showGUI = !showGUI;
        }

        private static float ByteToM = 0.000001f;

        private static void OnGUI()
        {
            GUIUtil.SetGUIStyle();

            GUILayout.Label(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")+",F1查看日志");

            GUILayout.Label("内存：" + UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() * ByteToM ); 

            if (onGUICallback != null) onGUICallback();

            //windowRect = new Rect(margin + Screen.width * 0.2f,
            //                    margin,
            //                    Screen.width * 0.8f - (2 * margin),
            //                    Screen.height - (2 * margin));

            //GUILayout.Window(100, windowRect, ConsoleWindow, "Console");

            if (!showGUI) return;

            StringBuilder sb = new StringBuilder();

            for (int i = entries.Count-1; i >= 0; i--)
            {
                sb.AppendLine(entries[i].message);
            }

            GUILayout.Label(sb.ToString());
        }

        static List<ConsoleMessage> entries = new List<ConsoleMessage>();

        static void HandleLog(string message, string stackTrace, LogType type)
        {
            ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
            entries.Add(entry);

            while (entries.Count > 30) entries.RemoveAt(0);
        }
    }

    struct ConsoleMessage
    {
        public readonly string message;
        public readonly string stackTrace;
        public readonly LogType type;

        public ConsoleMessage(string message, string stackTrace, LogType type)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }
}
