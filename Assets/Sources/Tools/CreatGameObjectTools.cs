/****************************************************
    文件：CreatGameObjectTools.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2019/10/8 10:15:9
	功能：利用Resources.load方法在Canvas下加载预制体
*****************************************************/

using UnityEngine;

public class CreatGameObjectTools : MonoBehaviour 
{
    static Transform uiParent;
    public static Transform UIParent
    {
        get
        {
            if (uiParent == null)
                uiParent = GameObject.Find("Canvas").transform;
            return uiParent;
        }
    }
    public static GameObject CreateUIPanel(string type)
    {
        GameObject go = Resources.Load<GameObject>(type.ToString());
        if (go == null)
        {
            Debug.Log(type.ToString() + "不存在");
            return null;
        }
        else
        {
            GameObject panel = GameObject.Instantiate(go);
            panel.name = type.ToString();
            panel.transform.SetParent(UIParent);
            return go;
        }
    }
}