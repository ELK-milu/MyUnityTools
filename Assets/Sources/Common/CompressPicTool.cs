using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

public class CompressPicTool 
{
    /// <summary>
    /// 无损压缩图片
    /// </summary>
    /// <param name="sFile">原图片地址</param>
    /// <param name="dFile">压缩后保存图片地址</param>
    /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
    /// <param name="size">压缩后图片的最大大小</param>
    /// <param name="sfsc">是否是第一次调用</param>
    /// <returns></returns>
    public static bool CompressImage(string sFile, string dFile, int flag = 100, int size = 1024, bool sfsc = true)
    {
        Image iSource = Image.FromFile(sFile);
        ImageFormat tFormat = iSource.RawFormat;
        //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
        FileInfo firstFileInfo = new FileInfo(sFile);
        if (sfsc == true && firstFileInfo.Length < size * 1024)
        {
            firstFileInfo.CopyTo(dFile);
            return true;
        }
        Debug.Log(iSource.Width + "  " + iSource.Height);
        Vector2 limitV2 = AdaptSize(new Vector2(1920, 1080), new Vector2(iSource.Width, iSource.Height));
        Debug.Log(limitV2);
        int dWidth = (int)limitV2.x;
        int dHeight = (int)limitV2.y;

        int sW = 0, sH = 0;
        //按比例缩放
        Size tem_size = new Size(iSource.Width, iSource.Height);
        if (tem_size.Width > dHeight || tem_size.Width > dWidth)
        {
            if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
            {
                sW = dWidth;
                sH = (dWidth * tem_size.Height) / tem_size.Width;
            }
            else
            {
                sH = dHeight;
                sW = (tem_size.Width * dHeight) / tem_size.Height;
            }
        }
        else
        {
            sW = tem_size.Width;
            sH = tem_size.Height;
        }

        Bitmap ob = new Bitmap(dWidth, dHeight);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(ob);

        g.Clear(System.Drawing.Color.WhiteSmoke);
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

        g.Dispose();

        //以下代码为保存图片时，设置压缩质量
        EncoderParameters ep = new EncoderParameters();
        long[] qy = new long[1];
        qy[0] = flag;//设置压缩的比例1-100
        EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        ep.Param[0] = eParam;

        try
        {
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICIinfo = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICIinfo = arrayICI[x];
                    break;
                }
            }
            if (jpegICIinfo != null)
            {
                ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                FileInfo fi = new FileInfo(dFile);
                //if (fi.Length > 1024 * size)
                //{
                //    flag = flag - 10;
                //    CompressImage(sFile, dFile, flag, size, false);
                //}
            }
            else
            {
                ob.Save(dFile, tFormat);
            }
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            iSource.Dispose();
            ob.Dispose();
        }
    }

    /// <summary>
    /// 自适应图片尺寸
    /// </summary>
    /// <param name="limitRange"></param>
    /// <param name="textureSize"></param>
    /// <returns></returns>
    private static Vector2 AdaptSize(Vector2 limitRange, Vector2 textureSize)
    {
        Vector2 size = textureSize;
        float standard_ratio = limitRange.x / limitRange.y;
        float ratio = size.x / size.y;
        if (ratio > standard_ratio)
        {
            //宽于标准宽度，宽固定
            float scale = size.x / limitRange.x;
            size.x = limitRange.x;
            size.y /= scale;
        }
        else
        {
            //高于标准宽度，高固定
            float scale = size.y / limitRange.y;
            size.y = limitRange.y;
            size.x /= scale;
        }
        return size;
    }
}
