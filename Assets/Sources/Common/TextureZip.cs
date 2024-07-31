

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextureZip : MonoBehaviour 
{
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

    /// <summary>
    /// 压缩图片
    /// </summary>
    /// <param name="orginalTexture"></param>
    /// <param name="limitRange"></param>
    /// <param name="type"> 0 :无限制  1: 缩小  2:放大</param>
    /// <returns></returns>
    private static Texture2D CompressionTexture(Texture2D orginalTexture, Vector2 limitRange, int type = 0)
    {
        if (!orginalTexture.isReadable)//读写权限
            return orginalTexture;

        //等比例
        Vector2 size = AdaptSize(limitRange, new Vector2(orginalTexture.width, orginalTexture.height));
        Color color;
        float pixo = limitRange.x / size.x;
        float pix = orginalTexture.width / size.x;

        if (pix > 1 && type == 2) //缩小 
            return orginalTexture;
        else if (pix < 1 && type == 1) //放大 
            return orginalTexture;

        Texture2D newTexture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGBA32, false);
        for (int i = 0; i < newTexture.width; i++)
        {
            for (int j = 0; j < newTexture.height; j++)
            {
                color = orginalTexture.GetPixel((int)(i * pix), (int)(j * pix));
                newTexture.SetPixel(i, j, color);
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    /// <summary>
    /// 将Texture转为本地JPG...
    /// </summary>
    /// <param name="filePath">输出路径</param>
    /// <param name="teture">Texture2D</param>
    /// <returns></returns>
    private static bool SaveMainTextureToJPG(string filePath, Texture teture)
    {
        if (teture.GetType() != typeof(Texture2D))
        {
            return false;
        }
        Texture2D savedTexture = (Texture2D)teture;
        try
        {
            Texture2D newTexture = new Texture2D(savedTexture.width, savedTexture.height, TextureFormat.RGBA32, false);
            newTexture.SetPixels(0, 0, savedTexture.width, savedTexture.height, savedTexture.GetPixels());
            newTexture.Apply();
            byte[] bytes = newTexture.EncodeToJPG();
            if (bytes != null && bytes.Length > 0)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.WriteAllBytes(filePath, bytes);
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("保存图片出错：" + ex);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 单图压缩
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="sourceName">源图片名称</param>
    /// <param name="outPath">输出路径</param>
    /// <returns></returns>
    public static string OnSingleTextureZip(string sourcePath, string sourceName, string outPath)
    {
        if (File.Exists(outPath + sourceName))
        {
            Debug.Log("无需压缩：" + outPath + sourceName);
            return outPath + sourceName;
        }

        Texture2D texture = CompressionTexture(ImageTools.LoadTexture2d(sourcePath + sourceName), new Vector2(1920, 1080), 0);
        if (SaveMainTextureToJPG(outPath + sourceName, texture))
        {
            return outPath + sourceName;
        }

        return "";
    }


    /// <summary>
    /// 多图压缩
    /// </summary>
    /// <param name="sourceFullPaths">源全路径</param>
    /// <param name="sourceName">断点字符 '/'</param>
    /// <param name="outPath">输出路径</param>
    /// <returns></returns>
    public static List<string> OnGroupTextureZip(List<string> sourceFullPaths, char symbol, string outPath)
    {
        List<string> outFullPaths = new List<string>();
        foreach (var source in sourceFullPaths)
        {
            string sourceName = LoadImage.GetNameByFullPath(source, symbol);

            if (File.Exists(outPath + sourceName))
            {
                Debug.Log("无需压缩：" + outPath + sourceName);
                continue;
            }

            Texture2D texture = CompressionTexture(ImageTools.LoadTexture2d(source), new Vector2(1920, 1080), 0);

            if (SaveMainTextureToJPG(outPath + sourceName, texture))
            {
                Debug.Log(outPath + sourceName);
                outFullPaths.Add(outPath + sourceName);
            }
        }
        return outFullPaths;
    }

}
