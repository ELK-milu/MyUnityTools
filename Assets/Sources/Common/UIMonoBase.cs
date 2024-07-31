/****************************************************
    文件：UIMonoBase.cs
	作者：Plusbe_hmr
    邮箱: 2698971533@qq.com
    日期：2022/9/23 10:28:57
	功能：常用
*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMonoBase : MonoBehaviour 
{
    public bool IsShow
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 查找子物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comName">名称路径</param>
    /// <returns></returns>
    public T FindChildComponent<T>(string comName) where T : Object
    {
        return transform.Find(comName).GetComponent<T>();
    }

    /// <summary>
    /// 查找子物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childIndex">数据下标</param>
    /// <returns></returns>
    public T FindChildComponent<T>(int childIndex) where T : Object
    {
        return transform.GetChild(childIndex).GetComponent<T>();
    }


    /// <summary>
    /// 查找子物体节点下所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comName">名称路径</param>
    /// <param name="includeInactive">是否包括查询失活物体</param>
    /// <returns></returns>
    public T[] FindRootComponents<T>(string comName, bool includeInactive = false) where T : Object
    {
        return transform.Find(comName).GetComponentsInChildren<T>(includeInactive);
    }

    /// <summary>
    /// 查找子物体节点下所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childIndex">数据下标</param>
    /// <param name="includeInactive">是否包括查询失活物体</param>
    /// <returns></returns>
    public T[] FindRootComponents<T>(int childIndex, bool includeInactive = false) where T : Object
    {
        return transform.GetChild(childIndex).GetComponentsInChildren<T>(includeInactive);
    }
}

//拓展方法
public static class Extensions
{ 
    public static T FindChildComponent<T>(this Transform t, string comName) where T : Object
    {
        return t.Find(comName).GetComponent<T>();
    }

    public static T FindChildComponent<T>(this Transform t, int childIndex) where T : Object
    {
        return t.GetChild(childIndex).GetComponent<T>();
    }



    public static T[] FindRootComponents<T>(this Transform t, string comName, bool includeInactive = false) where T : Object
    {
        return t.Find(comName).GetComponentsInChildren<T>(includeInactive);
    }

    public static T[] FindRootComponents<T>(this Transform t, int childIndex, bool includeInactive = false) where T : Object
    {
        return t.GetChild(childIndex).GetComponentsInChildren<T>(includeInactive);
    }
}
