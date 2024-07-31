using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadImage 
{
    /// <summary>
    /// 加载文件夹内图片
    /// </summary>
    public static List<Sprite> LoadAllSprites(string path)
    {
        List<Sprite> sprites = new List<Sprite>();
        List<string> filePaths = new List<string>();

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles(path, ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }

        for (int i = 0; i < filePaths.Count; i++)
        {
            Texture2D tx = new Texture2D(100, 100);
            tx.LoadImage(GetImageByte(filePaths[i]));
            Sprite sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
            sprites.Add(sprite);
        }
        return sprites;
    }

    /// <summary>
    /// 加载文件夹内图片
    /// </summary>
    public static List<Texture2D> LoadAllTexture2D(string path)
    {
        List<Texture2D> sprites = new List<Texture2D>();
        List<string> filePaths = new List<string>();

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles(path, ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }

        for (int i = 0; i < filePaths.Count; i++)
        {
            Texture2D tx = new Texture2D(100, 100);
            tx.LoadImage(GetImageByte(filePaths[i]));
            tx.Apply();
            sprites.Add(tx);
        }
        return sprites;
    }

    /// <summary>
    /// 获取文件夹内所有图片的地址
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> LoadAllFilePaths(string path)
    {
        List<string> filePaths = new List<string>();

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles(path, ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }
        return filePaths;
    }

    /// <summary>  
    /// 根据图片路径返回图片的字节流byte[]  
    /// </summary>  
    /// <param name="imagePath">图片路径</param>  
    /// <returns>返回的字节流</returns>  
    private static byte[] GetImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }

    /// <summary>
    /// 加载单张图片
    /// </summary>
    public static Sprite LoadSingle(string path)
    {
        Texture2D tx = new Texture2D(100, 100);
        tx.LoadImage(GetImageByte(path));
        Sprite sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
        return sprite;
    }

    /// <summary>
    /// 加载单张图片
    /// </summary>
    public static Texture2D LoadTexture2D(string path)
    {
        Texture2D tx = new Texture2D(100, 100);
        tx.LoadImage(GetImageByte(path));
        tx.Apply();
        return tx;
    }

    /// <summary>
    /// 全路径查找图片名称
    /// </summary>
    /// <param name="fullPath">全路径</param>
    /// <param name="c">断点字符 '/'</param>
    /// <returns></returns>
    public static string GetNameByFullPath(string fullPath, char c)
    {
        int startIndex = fullPath.LastIndexOf(c) + 1;
        int length = fullPath.Length - startIndex;
        return fullPath.Substring(startIndex, length);
    }
}
