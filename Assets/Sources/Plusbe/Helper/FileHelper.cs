using PlusbeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Helper
{
    public class FileHelper
    {
        public static void CreateFile(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("File Create Fail! \n" + e.Message);
            }
        }

        public static void WriteText(string path, string data)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(data);
                }
            }

        }

        public static string ReadText(string path)
        {
            if (!File.Exists(path)) return "";
            return File.ReadAllText(path, Encoding.UTF8);//读取文件
        }
    }
}
