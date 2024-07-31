using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ConfigDataBase
{
    private JsonDataBase _JsonDataBase = new JsonDataBase();

    /// <summary>
    /// 创建Json
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="t">实体数据</param>
    /// <param name="configPath">地址</param>
    /// <param name="callback">回调</param>
    public void CreateJson<T>(T t,string configPath, Action callback = null)
    {
        if (String.IsNullOrEmpty(configPath))
        {
            Debug.LogError("文件路径为空！");
            return;
        }
        _JsonDataBase.CreateJson(t, configPath, callback);
    }

    /// <summary>
    /// 读取Json
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="configPath">地址</param>
    /// <param name="callback">回调</param>
    public void ReadJson<T>(string configPath, Action<T> callback = null)
    {
        if (String.IsNullOrEmpty(configPath))
        {
            Debug.LogError("文件路径为空！");
            return;
        }
        _JsonDataBase.ReadJson(configPath, callback);
    }


    private XmlDataBase _XmlDataBase = new XmlDataBase();

    /// <summary>
    /// 创建Xml
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="t">实体数据</param>
    /// <param name="configPath">地址</param>
    /// <param name="callback">回调</param>
    public void CreateXml<T>(T t, string configPath, Action callback = null)
    {
        if (String.IsNullOrEmpty(configPath))
        {
            Debug.LogError("文件路径为空！");
            return;
        }
        _XmlDataBase.CreateXml(t, configPath, callback);
    }

    /// <summary>
    /// 读取Xml
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="configPath">地址</param>
    /// <param name="callback">回调</param>
    public void ReadXml<T>(string configPath, Action<T> callback = null)
    {
        if (String.IsNullOrEmpty(configPath))
        {
            Debug.LogError("文件路径为空！");
            return;
        }
        _XmlDataBase.ReadXml(configPath, callback);
    }
}

public class JsonDataBase
{
    public void CreateJson<T>(T t, string path, Action callback = null)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        string json = JsonUtility.ToJson(t, true);
        File.WriteAllText(path, json);

        Debug.Log("Json保存成功");
        callback?.Invoke();
    }

    public void ReadJson<T>(string path, Action<T> callback = null)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("读取的文件不存在！");
            return;
        }
        string json = File.ReadAllText(path);
        Debug.Log(json);
        T t = JsonUtility.FromJson<T>(json);

        Debug.Log("Json读取成功");
        callback?.Invoke(t);
    }
}

public class XmlDataBase
{
    public void CreateXml<T>(T t, string path, Action callback = null)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(fileStream, t);
        }

        Debug.Log("Xml保存成功");
        callback?.Invoke();
    }

    public void ReadXml<T>(string path, Action<T> callback = null)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("读取的文件不存在！");
            return;
        }
        T t;
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            t = (T)xmlSerializer.Deserialize(fileStream);
        }

        Debug.Log("Xml读取成功");
        callback?.Invoke(t);
    }
}