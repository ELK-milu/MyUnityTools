using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlusbeHelper
{
    public class PlusbeDebug
    {
        public static void Log(string msg)
        {
            Debug.Log(msg);
        }

        public static void LogError(string msg)
        {
            Log(msg);
        }

        public static void LogTip(string msg)
        {
            Log(msg);
        }
    }
}
