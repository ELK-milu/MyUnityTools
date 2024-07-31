using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlusbeHelper
{
    public class PathHelper
    {
        public static string getTempFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
        }

        public static string getTempFileName(string suffix)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + suffix;
        }

        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }
            else
            {
                CreateDirectory(Directory.GetParent(path).FullName);
                Directory.CreateDirectory(path);
            }
        }
		
		//获取字符串
		public static string GetData(string path)
		{
			if (!File.Exists(path)) return "";
			return File.ReadAllText(path, Encoding.UTF8);//读取文件
		}

        public static string[] GetFileImage(string path)
        {
            string[] files = Directory.GetFiles(path);
            List<string> temp = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (IsImage(files[i])) temp.Add(files[i]);
            }

            return temp.ToArray();
        }

        public static string[] GetFileImage(string path, bool asc = true)
        {
            string[] files = Directory.GetFiles(path);
            List<string> temp = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                if (IsImage(files[i])) temp.Add(files[i]);
            }

            if (!asc) temp.Reverse();
            return temp.ToArray();
        }

        public static bool IsImage(string path)
        { 
            if(path.ToLower().EndsWith(".png")|| path.ToLower().EndsWith(".jpg")||path.ToLower().EndsWith(".jpeg")) return true;
            return false;
        }
    }
}
