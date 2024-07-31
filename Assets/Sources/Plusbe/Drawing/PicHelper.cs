using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
namespace Plusbe.Drawing

{
    public class PicHelper
    {

        public static byte[] CutFace(Image source, int left, int top, int right, int bottom)
        {
            System.Drawing.Image imgSource = source;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;

            int offW = Convert.ToInt32((right - left) * 0.3);
            int offH = Convert.ToInt32((bottom - top) * 0.3);

            int startX = left - offW > 0 ? left - offW : 0;
            int startY = top - offH > 0 ? top - offH : 0;
            int sW = sWidth - (right + offW) > 0 ? right + offW - startX : sWidth - startX;
            int sH = sHeight - (bottom + offH) > 0 ? bottom + offH - startY : sHeight - startY;

            Bitmap outBmp = new Bitmap(sW, sH);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
            g.Clear(System.Drawing.Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle(0, 0, sW, sH), new Rectangle(startX, startY, sW, sH), GraphicsUnit.Pixel);
            g.Dispose();

            //outBmp.Save(@"C:\Users\Administrator\Desktop\zhuotemp\" + DateTime.Now.ToString("HHmmssfff") + ".png");

            return imageToBytes(outBmp);
        }
        public static byte[] imageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }

                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public static Bitmap BytesToBit(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            return new Bitmap(stream);
        }

        public static Bitmap ReadFile(string path)
        {
            if (!File.Exists(path))
                return null;

            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                byte[] b = new byte[file.Length];
                file.Read(b, 0, b.Length);

                MemoryStream stream = new MemoryStream(b);

                return new Bitmap(stream);
            }
        }


        public static MemoryStream streamFromPath(string path)
        {
            if (!File.Exists(path)) return null;
            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                byte[] b = new byte[file.Length];
                file.Read(b, 0, b.Length);

                return new MemoryStream(b);
            }
        }

        //public static Image CopyRecPic(Image source, int cutX, int cutY, int cutW, int cutH)
        //{
        //    System.Drawing.Image imgSource = source;
        //    System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
        //    // 按比例缩放           
        //    int sWidth = imgSource.Width;
        //    int sHeight = imgSource.Height;

        //    cutW = cutW - cutX;
        //    cutH = cutH - cutY;

        //    Bitmap outBmp = new Bitmap(cutW, cutH);
        //    //Bitmap outBmp = new Bitmap(destWidth, destHeight);
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
        //    g.Clear(System.Drawing.Color.Transparent);
        //    // 设置画布的描绘质量         
        //    g.CompositingQuality = CompositingQuality.HighQuality;
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    g.DrawImage(imgSource, new Rectangle(0, 0, cutW, cutH), new Rectangle(cutX, cutY, cutW, cutH), GraphicsUnit.Pixel);
        //    g.Dispose();

        //    // 以下代码为保存图片时，设置压缩质量     
        //    EncoderParameters encoderParams = new EncoderParameters();
        //    long[] quality = new long[1];
        //    quality[0] = 100;
        //    EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //    encoderParams.Param[0] = encoderParam;
        //    //imgSource.Dispose();

        //    return outBmp;
        //}

        //public static Image CutPic(Image source, int cutW, int cutH, int desW, int desH)
        //{
        //    System.Drawing.Image imgSource = source;
        //    System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
        //    // 按比例缩放           
        //    int sWidth = imgSource.Width;
        //    int sHeight = imgSource.Height;
        //    int sW = sWidth - cutW;
        //    int sH = sHeight - cutH;

        //    Bitmap outBmp = new Bitmap(desW, desH);
        //    //Bitmap outBmp = new Bitmap(destWidth, destHeight);
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
        //    g.Clear(System.Drawing.Color.Transparent);
        //    // 设置画布的描绘质量         
        //    g.CompositingQuality = CompositingQuality.HighQuality;
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    g.DrawImage(imgSource, new Rectangle(0, 0, desW, desH), new Rectangle(0, 0, sW, sH), GraphicsUnit.Pixel);
        //    g.Dispose();

        //    // 以下代码为保存图片时，设置压缩质量     
        //    EncoderParameters encoderParams = new EncoderParameters();
        //    long[] quality = new long[1];
        //    quality[0] = 100;
        //    EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //    encoderParams.Param[0] = encoderParam;
        //    imgSource.Dispose();

        //    return outBmp;
        //}

        #region common

        public static Image LoadImage(string path)
        {
            return LoadImage(StreamFromPath(path));
        }

        public static Image LoadImage(MemoryStream ms)
        {
            return ms == null ? null : Image.FromStream(ms);
        }

        public static MemoryStream StreamFromPath(string path)
        {
            if (!File.Exists(path)) return null;
            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                byte[] b = new byte[file.Length];
                file.Read(b, 0, b.Length);

                return new MemoryStream(b);
            }
        }

        public static byte[] ByteFromImage(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(ms, ImageFormat.Png);
                }

                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        #endregion

        #region 存储图片

        /// <summary>
        /// 存储图片
        /// 不建议同时大量存储图片《texture》，未避免出现卡顿现象，建议在协程中执行，每存储一张暂停0.3s+
        /// 在一般情况下，建议使用jpg替代png
        /// </summary>
        public static void SaveImage(string path, Texture2D texture)
        {
            SaveImage(path, texture.EncodeToJPG());
        }

        public static void SaveImage(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        //private IEnumerator ToSavePic()
        //{
        //    yield return new WaitForEndOfFrame();
        //    string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");
        //    PathHelper.CreateDirectory(GlobalSetting.DataPath + "Temp/");
        //    for (int j = 0; j < 1; j++)
        //    {
        //        for (int i = 0; i < files.Length; i++)
        //        {
        //            WWW www = new WWW(files[i]);
        //            yield return www;
        //            Texture2D texture = www.texture;
        //            GetRaw().texture = texture;
        //            byte[] bytes = texture.EncodeToPNG();
        //            File.WriteAllBytes(GlobalSetting.DataPath + "Temp/" + PathHelper.getTempFileName(".png"), bytes);
        //            yield return new WaitForSeconds(0.5f);
        //        }
        //    }
        //}

        #endregion

        #region 加载图片
        public static UnityEngine.Texture2D LoadPicTexture(string path)
        {
            if (File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                fileStream.Seek(0, SeekOrigin.Begin);
                //创建文件长度缓冲区
                byte[] bytes = new byte[fileStream.Length];
                //读取文件
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                //释放文件读取流
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;

                //创建Texture
                int width = 10;
                int height = 10;
                Texture2D texture = new UnityEngine.Texture2D(width, height);
                texture.LoadImage(bytes);

                return texture;
            }

            return null;
        }

        // [Obsolete("Please use <UnityWebRequestHelper> instead")]
        public static System.Collections.IEnumerator LoadPicTexture(string path, Action<Texture2D> callBack)
        {
            if (File.Exists(path))
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    while (!www.downloadHandler.isDone)
                    {
                        
                    }
                    Texture2D t2 = DownloadHandlerTexture.GetContent(www);
                    callBack(t2);
                }
                else
                {
                   callBack(null);
                }
            }

            callBack(null);
            yield return new WaitForEndOfFrame();
        }

        //public static System.Collections.IEnumerator LoadPicTexture(string path, UnityEngine.UI.RawImage picView)
        //{
        //    if (File.Exists(path))
        //    {
        //        WWW www = new WWW(path);
        //        yield return www;
        //        if (string.IsNullOrEmpty(www.error) && picView != null)
        //        {
        //            picView.texture = www.texture;
        //        }
        //    }
        //    yield return new WaitForEndOfFrame();
        //}

        public static UnityEngine.Sprite loadPic(string path)
        {
            if (File.Exists(path))
            {
                Texture2D texture = LoadPicTexture(path);

                return UnityEngine.Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new UnityEngine.Vector2(0.5f, 0.5f));
            }

            return null;
        }

        #endregion

        #region 裁剪+合成+缩略图

        /*
         * 裁剪模式
         * 1.裁剪指定区域指定大小
         * 2.针对指定区域指定大小进行等比放大裁剪，考虑超出边界问题，
         * 3.针对常用尺寸如A4 A5  4-6寸，单寸照 1920*1080 1280*760 1280*960
         * 
         * 合成模式
         * 1.合成指定带下的图像
         * 2.对比例不对的图像进行裁剪操作
         * 3.需要合成的前景，裁剪前景指定的区域合成在背景指定的区域
         * 
         * 缩略图
         * 1.强制缩放
         * 2.等比缩放，不拉伸
         * 3.达到目标指定分辨率
        */

        public static void CutImage(string path, string toPath, int x, int y, int w, int h)
        {
            Image temp = LoadImage(path);
            Bitmap outBmp = new Bitmap(w, h);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
            g.Clear(System.Drawing.Color.Transparent);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(temp, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
            g.Dispose();

            outBmp.Save(toPath);
        }

        public static void MergeImageThread(string bgPath, string toPath, int w, int h,
            string img1Path, int x1, int y1, int w1, int h1, int tx1, int ty1, int tw1, int th1,
            Action<string> callBack)
        {
            List<ImageData> list = new List<ImageData>();
            list.Add(new ImageData(img1Path, x1, y1, w1, h1, tx1, ty1, tw1, th1));
            MergeImageData data = new MergeImageData(bgPath, toPath, w, h, list, callBack);
            ThreadPool.QueueUserWorkItem(new WaitCallback(MergeImageThreadDo), data);
        }

        private static void MergeImageThreadDo(object obj)
        {
            MergeImageData data = obj as MergeImageData;
            Debug.Log(data);
            if (data != null)
            {
                Image temp = LoadImage(data.bgPath);
                Image bgImg = GetThumbnail(temp, data.w, data.h, true);
                Bitmap outBmp = new Bitmap(data.w, data.h);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
                g.Clear(System.Drawing.Color.Transparent);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bgImg, new Rectangle(0, 0, data.w, data.h), new Rectangle(0, 0, bgImg.Width, bgImg.Height), GraphicsUnit.Pixel);

                for (int i = 0; i < data.imgs.Count; i++)
                {
                    Image img1 = LoadImage(data.imgs[i].path);
                    g.DrawImage(img1, new Rectangle(data.imgs[i].tx1, data.imgs[i].ty1, data.imgs[i].tw1, data.imgs[i].th1), new Rectangle(data.imgs[i].x1, data.imgs[i].y1, data.imgs[i].w1, data.imgs[i].h1), GraphicsUnit.Pixel);
                }
                g.Dispose();
                outBmp.Save(data.toPath);
                if (data.callBack != null) data.callBack(data.toPath);
            }
        }

        /// <summary>
        /// 1.源素材大小
        /// 2.合成分辨率
        /// </summary>
        public static void MergeImage(string bgPath, string toPath, int w, int h,
            string img1Path, int x1, int y1, int w1, int h1, int tx1, int ty1, int tw1, int th1)
        {
            Image temp = LoadImage(bgPath);
            //Image bgImg = GetThumbnail(temp, w, h, true);
            Image img1 = LoadImage(img1Path);

            Bitmap outBmp = new Bitmap(w, h);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
            g.Clear(System.Drawing.Color.Transparent);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(temp, new Rectangle(0, 0, w, h), new Rectangle(0, 0, temp.Width, temp.Height), GraphicsUnit.Pixel);
            g.DrawImage(img1, new Rectangle(tx1, ty1, tw1, th1), new Rectangle(x1, y1, w1, h1), GraphicsUnit.Pixel);
            g.Dispose();

            outBmp.Save(toPath);
        }

        public static bool Thumbnail(string fromPath, string toPath, int w, int h, bool sliced = true)
        {
            if (File.Exists(toPath))
            {
                return true;
            }

            if (File.Exists(fromPath))
            {
                Image fromImage = LoadImage(fromPath);
                Bitmap toBit = GetThumbnail(fromImage, w, h, sliced);
                toBit.Save(toPath);
            }

            return false;
        }

        private static Bitmap GetThumbnailMax(Image b, int maxWidth, int maxHeight, bool sliced, bool veryTrue = false)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;

            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            float offset = veryTrue ? 0.02f : 0.1f; //误差x%以内强制拉伸

            if (sliced)
            {
                float sbili = (float)sWidth / sHeight; // 200 * 200
                float dbili = (float)maxWidth / maxHeight; //200*100

                if (sbili - dbili > offset)
                {
                    // 高度超了
                    sW = maxWidth;
                    sH = Convert.ToInt32(sW / sbili);

                }
                else if (sbili - dbili < -offset)
                {
                    //宽度超了
                    sH = maxHeight;
                    sW = Convert.ToInt32(sH * sbili);
                }
                else
                {
                    sW = maxWidth;
                    sH = maxHeight;
                }
            }
            else
            {
                //拉伸填充
                sW = maxWidth;
                sH = maxHeight;
            }

            Bitmap outBmp = new Bitmap(sW, sH);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
            g.Clear(System.Drawing.Color.Transparent);

            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgSource, new Rectangle(0, 0, sW, sH), new Rectangle(0, 0, sWidth, sHeight), GraphicsUnit.Pixel);

            //g.DrawImage(imgSource, new Rectangle(0, 0, sW, sH), new Rectangle(0,0, sWidth, sHeight), GraphicsUnit.Pixel);

            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();

            return outBmp;
        }

        private static Bitmap GetThumbnail(Image b, int destWidth, int destHeight, bool sliced, bool veryTrue = false)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;

            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            float offset = veryTrue ? 0.02f : 0.1f; //误差x%以内强制拉伸

            if (sliced)
            {
                float sbili = (float)sWidth / sHeight; // 200 * 50
                float dbili = (float)destWidth / destHeight; //200*100

                if (sbili - dbili > offset)
                {
                    //宽>高
                    sH = sHeight;
                    sW = Convert.ToInt32(sHeight * dbili);
                }
                else if (sbili - dbili < -offset)
                {
                    //宽<高
                    sW = sWidth;
                    sH = Convert.ToInt32(sWidth / dbili);
                }
                else
                {
                    sW = sWidth;
                    sH = sHeight;
                }
            }
            else
            {
                //拉伸填充
                sW = sWidth;
                sH = sHeight;
            }

            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);
            g.Clear(System.Drawing.Color.Transparent);

            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgSource, new Rectangle(0, 0, destWidth, destHeight), new Rectangle((int)((sWidth - sW) * 0.5f), (int)((sHeight - sH) * 0.5f), sW, sH), GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();

            return outBmp;
        }


        #endregion

        #region 数据

        public class MergeImageData
        {
            public MergeImageData(string bgPath, string toPath, int w, int h, List<ImageData> imgs, Action<string> callBack)
            {
                this.bgPath = bgPath;
                this.toPath = toPath;
                this.w = w;
                this.h = h;
                this.imgs = imgs;
                this.callBack = callBack;
            }

            public string bgPath;
            public string toPath;
            public int w;
            public int h;
            public List<ImageData> imgs;
            public Action<string> callBack;
        }

        public class ImageData
        {
            public ImageData(string path, int x1, int y1, int w1, int h1, int tx1, int ty1, int tw1, int th1)
            {
                this.path = path;
                this.x1 = x1;
                this.y1 = y1;
                this.w1 = w1;
                this.h1 = h1;
                this.tx1 = tx1;
                this.ty1 = ty1;
                this.tw1 = tw1;
                this.th1 = th1;
            }

            public string path;
            public int x1;
            public int y1;
            public int w1;
            public int h1;
            public int tx1;
            public int ty1;
            public int tw1;
            public int th1;
        }

        #endregion
    }
}
