using com.google.zxing;
using com.google.zxing.common;
using Plusbe.Core;
using Plusbe.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestUploadFile
{
    class CodeData
    {
        public string fileName;
        public string content;
        public string path;
        public Action<string, string,string> action;
    }

    public class CodeHelper2
    { 
        private static string serverRootPath = "UploadFiles/";

        private static string urlContent;

        private static string urlPhotos = "photo.aspx?product={0}&pic={1}"; //参考 photo.aspx? product = hzdsj & pic = 20190821110612805.png | 20190821110612805.png

        public static string SaveCode(string product, string fileName,string codeName,Action<string,string,string> action)
        {
            string codePath = GlobalSetting.CodePath + codeName;
            if (File.Exists(codePath)) return codePath;

            string content = GlobalSetting.CodeServerIP + serverRootPath + product + "/" + fileName;

            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(SaveQRCode);
            Thread myThread = new Thread(ParStart);
            CodeData obj = new CodeData { fileName=fileName, content = content, path = codePath, action = action };
            myThread.Start(obj);

            return codePath;
        }

        public static string SaveCodeContent(string product, string fileNames, string codeName, Action<string, string, string> action)
        {
            string codePath = GlobalSetting.CodePath + codeName;
            if (File.Exists(codePath)) return codePath;

            string content =  GlobalSetting.CodeServerIP + string.Format(urlPhotos, product, fileNames);

            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(SaveQRCode);
            Thread myThread = new Thread(ParStart);
            CodeData obj = new CodeData { fileName = fileNames, content = content, path = codePath, action = action };
            myThread.Start(obj);

            return codePath;
        }


        private static void SaveQRCode(object data)
        {
            CodeData codeDatas = data as CodeData;
            SaveQRCode(codeDatas.fileName,codeDatas.content, codeDatas.path,codeDatas.action);
        }

        private static void SaveQRCode(string fileName,string content, string path, Action<string, string,string> action)
        {
            ByteMatrix byteMatrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, 410, 410);
            byteMatrix = DeleteWhite(byteMatrix, 30);
            Bitmap bitmap = ToBitmap(byteMatrix);
            ImageFormat format = ImageFormat.Png;
            bitmap.Save(path);
            bitmap.Dispose();

            action?.Invoke(fileName, content, path);
        }


        public static string codeDir = "SignPhoto/"; //"Sign/";//

        //public static void toSaveCodeMainThread(object file)
        //{
        //    string fileName = file.ToString();
        //    string path = GlobalSetting.CodePath + fileName;
        //    if (File.Exists(path)) return;

        //    string url = GlobalSetting.ServerIP + codeDir + fileName;

        //    ByteMatrix byteMatrix = new MultiFormatWriter().encode(url, BarcodeFormat.QR_CODE, 410, 410);
        //    byteMatrix = deleteWhite(byteMatrix, 30);
        //    Bitmap bitmap = toBitmap(byteMatrix);
        //    ImageFormat format = ImageFormat.Png;
        //    bitmap.Save(path);
        //    bitmap.Dispose();
        //}

        //public static string saveCode(string fileName)
        //{
        //    //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        //    //fileName = Path.GetFileName(url);

        //    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(toSaveCode);
        //    Thread myThread = new Thread(ParStart);
        //    object obj = fileName;
        //    myThread.Start(obj);

        //    string path = GlobalSetting.CodePath + fileName;
        //    return path;
        //}

        //private static void toSaveCode(object file)
        //{
        //    string fileName = file.ToString();

        //    string url = GlobalSetting.ServerIP + codeDir + fileName;
        //    ByteMatrix byteMatrix = new MultiFormatWriter().encode(url, BarcodeFormat.QR_CODE, 410, 410);
        //    byteMatrix = deleteWhite(byteMatrix, 30);
        //    Bitmap bitmap = toBitmap(byteMatrix);
        //    ImageFormat format = ImageFormat.Png;
        //    //string path = System.Web.HttpContext.Current.Request.MapPath("EwmPic");
        //    string path = GlobalSetting.CodePath + fileName;
        //    if (!File.Exists(path))
        //    {
        //        bitmap.Save(path);
        //    }
        //    bitmap.Dispose();
        //}

        private static ByteMatrix DeleteWhite(ByteMatrix matrix, int space)
        {
            int w = matrix.Width;
            int h = matrix.Height;

            int resw = matrix.Width - space * 2;
            int resh = matrix.Height - space * 2;


            ByteMatrix resMatrix = new ByteMatrix(resw, resh);
            for (int x = space; x < resw + space; x++)
            {
                for (int y = space; y < resh + space; y++)
                {
                    resMatrix.set_Renamed(x - space, y - space, matrix.get_Renamed(x, y));
                }
            }

            return resMatrix;
        }

        private static Bitmap ToBitmap(ByteMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (matrix.get_Renamed(x, y) != -1)
                    {

                    }
                    bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }
            return bmap;
        }
    }
}
