using PlusbeSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;



public class ResourceConfig
{
    public static string dataPath = Application.dataPath + "/Resources/Config/ResourcesConfig.xml";
    private static ResourceConfig instance;

    public List<ResourceItem> ResourceItems;

    public static ResourceConfig Instance
    {
        get
        {
            if (instance == null)
            {
                if (Application.isEditor)
                {
                    if (File.Exists(dataPath))
                    {
                        string data = Resources.Load<TextAsset>("Config/ResourcesConfig").ToString();
                        instance = XmlSerializerHelper.FromXml<ResourceConfig>(data);
                    }
                    else
                    {
                        instance = new ResourceConfig();
                        instance.ResourceItems = new List<ResourceItem>();
                        instance.Save();
                    }
                }
                else
                {
                    string data = Resources.Load<TextAsset>("Config/ResourcesConfig").ToString();
                    instance = XmlSerializerHelper.FromXml<ResourceConfig>(data);
                }
            }

            return instance;
        }
    }

    public void Save()
    {
        XmlSerializerHelper.SaveXml(Instance, dataPath);
    }

    public Dictionary<string, string> GetResource()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        for (int i = 0; i < ResourceItems.Count; i++)
        {
            dictionary.Add(ResourceItems[i].name, ResourceItems[i].path);
        }

        return dictionary;
    }

    /// <summary>
    /// 编辑器添加指定内容
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    public static void AddResourceItem(string name, string path)
    {
        ResourceItem item = new ResourceItem() { name = name, path = path };
        Instance.ResourceItems.Add(item);
        Instance.Save();
    }
}

public class ResourceItem
{
    public string name;
    public string path;
}

