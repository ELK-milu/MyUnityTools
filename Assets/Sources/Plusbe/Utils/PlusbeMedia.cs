using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plusbe.Utils
{
    public class PlusbeMedia
    {
        //视频 = 1,
        //链接 = 2,
        //ppt = 3,
        //图片 = 4,
        //客户端 = 5

        public static bool isVideo(string path)
        {
            return getPlusbeMediaType(path) == PlusbeMediaType.Video ? true : false;
        }

        public static bool isImage(string path)
        {
            return getPlusbeMediaType(path) == PlusbeMediaType.Image ? true : false;
        }

        public static bool isDownFileType(int type)
        {
            return type == 1 || type == 3 || type == 4 || type == 5 || type == 6;
        }

        public static bool isDownFile(string url)
        {
            return isDownFile(getPlusbeMediaType(url));
        }

        public static bool isDownFile(PlusbeMediaType type)
        {
            return type == PlusbeMediaType.Video || type == PlusbeMediaType.PPT|| type == PlusbeMediaType.Image|| type == PlusbeMediaType.Excel|| type == PlusbeMediaType.Word|| type == PlusbeMediaType.Excel|| type == PlusbeMediaType.PDF || type == PlusbeMediaType.Flash;
        }

        public static PlusbeMediaType getPlusbeMediaType(string url)
        {
            url = url.ToLower();

            if (url.EndsWith(".mp4") || url.EndsWith(".mpeg") || url.EndsWith(".avi") || url.EndsWith(".flv"))
            {
                return PlusbeMediaType.Video;
            }
            else if (url.EndsWith(".ppt") || url.EndsWith(".pptx"))
            {
                return PlusbeMediaType.PPT;
            }
            else if (url.EndsWith(".doc") || url.EndsWith(".docx"))
            {
                return PlusbeMediaType.Word;
            }
            else if (url.EndsWith(".xls") || url.EndsWith(".xlsx"))
            {
                return PlusbeMediaType.Excel;
            }
            else if (url.EndsWith(".pdf"))
            {
                return PlusbeMediaType.PDF;
            }
            else if (url.EndsWith(".png") || url.EndsWith(".jpg") || url.EndsWith(".jpeg") || url.EndsWith(".bmp"))
            {
                //return PlusbeMediaType.PicBox;
                return PlusbeMediaType.Image;
            }else if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return PlusbeMediaType.Web;
            }

            return PlusbeMediaType.None; 
        }

        /// <summary>
        /// 后台更新下来的文件类型
        /// </summary>
        public enum PlusbeDownMediaType
        { 
            None = 0,
            Video = 1,
            Web = 2,
            PPT = 3,
            Image = 4,
            Exe = 5
        }

        /// <summary>
        /// 文件类型 根据后缀判断对应类型
        /// </summary>
        public enum PlusbeMediaType
        { 
            None,
            Video,
            Web,
            PPT,
            Image,
            Exe,
            Word,
            Excel,
            PDF,
            Flash
        }
    }
}
