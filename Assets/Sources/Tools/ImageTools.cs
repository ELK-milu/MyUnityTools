using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageTools : MonoBehaviour {

    public static Texture2D LoadTexture2d(string path)
    {
        Texture2D t = new Texture2D(0, 0, TextureFormat.RGB24, false);
        try
        {
            t.LoadImage(File.ReadAllBytes(path));
            return t;
        }
        catch (Exception ex)
        {
            Debug.LogError("路径有错___" + ex.ToString());
            return null;
        }
    }

  
    //public static Texture2D LoadTexture2d(string path)
    //{
    //    Texture2D t = new Texture2D(0, 0, TextureFormat.RGB24, false);
    //    try
    //    {
    //        t.LoadImage(File.ReadAllBytes(path));
    //        t.filterMode = FilterMode.Bilinear;
    //        t.wrapMode = TextureWrapMode.Clamp;    
    //        t.Apply();
    //        return t;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("路径有错___" + ex.ToString());
    //        return null;
    //    }
    //}

    public static Sprite LoadSprite(string path)
    {
        Texture2D t = LoadTexture2d(path);
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.one * 0.5f);
    }
}
