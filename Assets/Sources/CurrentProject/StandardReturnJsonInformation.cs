/****************************************************
    文件：StandardReturnJsonInformation.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2021/1/5 9:37:8
	功能：返回json列表标准化
*****************************************************/

using Newtonsoft.Json.Linq;
using Plusbe.Config;
using System.Collections.Generic;
using UnityEngine;

public class StandardReturnJsonInformation : MonoBehaviour 
{

    private void Awake()
    {
        _instance = this;
    }
    //public static void Init()
    //{
    //    GameObject go = new GameObject("StandardJsonCreater");
    //    _instance = go.AddComponent<StandardReturnJsonInformation>();
    //    DontDestroyOnLoad(go);
    //}

    private static StandardReturnJsonInformation _instance;
    public static StandardReturnJsonInformation Instance
    {
        get
        {
            if (_instance == null)
            {
               // Init();
            }
            return _instance;
        }
    }

    public delegate void StandardPlayAtID(int contentID);
    public StandardPlayAtID playAt;

    public void DealHttpCommand(string act,string obj,string state)
    {
        if (act == "unity")
        {
            if(state == "13")
            {
                int id;
                if(int.TryParse(obj,out id))
                {
                    playAt?.Invoke(id);                   
                }
            }
        }
    }

    private List<JToken> tokens = new List<JToken>();
    public bool VerifyBackMessage(string act,string obj,string state,out string backMessage)
    {
        backMessage = "";
        if (act == "unity")
        {
            //获取全部列表或者获取tag列表
            if (state == "14")
            {
                int tag;
                if(int.TryParse(obj,out tag))
                {
                    print("标签列表");
                    tokens.Clear();
                    list.Clear();
                    tokens = JsonDataManager.GetJTokens(obj);
                    print(tokens.Count + ":Tokens.Count");
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        AddChild(int.Parse(tokens[i]["ID"].ToString()),tokens[i]["Title"].ToString(),int.Parse(tokens[i]["Tag"].ToString()));
                    }
                    backMessage = GetTagList(tag);
                    print(backMessage);
                    return true;
                }
                else
                {
                    print("全列表");
                    backMessage = GetFullList();
                    print(backMessage);
                    return true;
                }
            }
        }
        return false;   
    }

    public List<StandardContent> list = new List<StandardContent>();
    public void AddChild(int ID, string title, int tag)
    {
        StandardContent content = new StandardContent
        {
            ID = ID,
            title = title,
            tag = tag
        };
        list.Add(content);
    }

    public void AddData(JToken token)
    {
        AddChild(int.Parse(token["ID"].ToString()), token["Title"].ToString(), int.Parse(token["Tag"].ToString()));
    }

    public void AddDatas(List<JToken> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            AddChild(int.Parse(tokens[i]["ID"].ToString()), tokens[i]["Title"].ToString(), int.Parse(tokens[i]["Tag"].ToString()));
        }
    }

    public string GetFullList()
    {
        JObject jo = new JObject();
        JArray jarr = new JArray();
        jo.Add("list", jarr);
        
        StandardContent.StartToConvert();
        for (int i = 0; i < list.Count; i++)
        {
            jarr.Add(list[i].ConvertToListJObject());
        }

        return jo.ToString();
    }

    public string GetTagList(int tag)
    {
        JObject jo = new JObject();
        JArray jarr = new JArray();
        jo.Add("list", jarr);

        StandardContent.StartToConvert();
        print("list.count:" + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
           // if (list[i].tag == tag)
                jarr.Add(list[i].ConvertToListJObject());
        }
        
        return jo.ToString();
        
    }


}


public class StandardContent
{
    public int ID;
    public string title;
    public string word = "";
    public static int index = 0;
    public int fileType = 1;
    public int tag = 1;


    public static void StartToConvert()
    {
        index = 0;
    }

    public JObject ConvertToListJObject()
    {
        JObject jo = new JObject();
        jo.Add("ID", ID);
        jo.Add("title", title);
        jo.Add("word", word);
        jo.Add("index", index);
        jo.Add("fileType", fileType);
        jo.Add("tag", tag);
        index++;
        return jo;
    }
}