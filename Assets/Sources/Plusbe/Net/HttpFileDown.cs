using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Http
{
    public class HttpFileDown
    {
        public enum DownResultType
        {
            None,
            Exist,
            Success,
            Error,
            Unknow
        }

        private static long GetDownLength(string url)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                myrp.Close();
                return myrp.ContentLength;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>        
        /// c#,.net 下载文件        
        /// </summary>        
        /// <param name="URL">下载文件地址</param>       
        /// 
        /// <param name="Filename">下载后的存放地址</param>        
        /// <param name="Prog">用于显示的进度条</param>        
        /// Form
        //public static bool DownloadFile(string url, string filename, ProgressBar prog, Label label1, bool bForce)
        public static DownResultType DownloadFile(string url, string filename, bool bForce)
        {
            string fileName2 = filename + ".plusbedownload";

            long startPos = 0;
            try
            {

                Stream fs = null;

                if (!bForce && File.Exists(filename))
                {
                    return DownResultType.Exist;
                }
                else 
                if (!bForce && File.Exists(fileName2))
                {
                    long allLeng = GetDownLength(url);

                    //打开文件，并得到已经下载量
                    fs = File.OpenRead(fileName2);
                    if (fs.Length >= allLeng && allLeng>1024*5)
                    {
                        fs.Close();
                        File.Move(fileName2, filename);
                        return DownResultType.Success;
                        //return "文件已经下载";
                    }
                    fs.Close();

                    fs = File.OpenWrite(fileName2);
                    startPos = fs.Length;
                    if (fs.Length == allLeng && allLeng > 1024 * 5)
                    {
                        fs.Close();
                        File.Move(fileName2, filename);
                        return DownResultType.Success;
                        //return "文件已经下载";
                    } //移动文件流中的当前指针
                    fs.Seek(startPos, SeekOrigin.Current);
                }
                else
                {
                    fs = new System.IO.FileStream(fileName2, FileMode.Create);
                    startPos = 0;
                }

                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                if (startPos > 0)
                    Myrq.AddRange((int)startPos); //设置Range值 

                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();

                long totalBytes = myrp.ContentLength;

                System.IO.Stream st = myrp.GetResponseStream();
                //System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    //System.Windows.Forms.Application.DoEvents();
                    fs.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                    //System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }

                long len2 = fs.Length;
                fs.Close();
                st.Close();
                if (totalBytes == len2)
                {
                    File.Move(fileName2, filename);
                    return DownResultType.Success;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("下载错误," + ex.Message + ";地址：" + url);
                return DownResultType.Error; 
            }
            return DownResultType.Unknow;
        }
    }
}
