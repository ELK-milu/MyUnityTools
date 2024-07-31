using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class CreateUIWindowl : Editor
{ 
    static string windotScriptTemplate = "UIWindowScriptTemplate";
    static private string uiScriptPath = Path.Combine(Application.dataPath, "Scripts/UI");
    static List<Bind> bindList = new List<Bind>();


    [MenuItem("GameObject/@CreateUIPrefab", false, 22)]//创建UI预制体
    static void CreateUIPrefab()
    {
        GameObject obj = Selection.activeGameObject;
        GameObject pre = Resources.Load<GameObject>("UI/Plusbe_UIPrefab");
        Instantiate(pre, obj.transform);
    }

    [MenuItem("GameObject/@CreateAndApplyUIPrefab", false, 21)]//创建UI预制体
    static void CreateAndApplyUIPrefab()
    {
        GameObject obj = Selection.activeGameObject;
        if (!obj)
        {
            Debug.LogError("未选择UI对象");
            return;
        }
        obj.name = obj.name.Contains("Window") ? obj.name : obj.name + "Window";
        string path = $"Assets/Resources/UI/{obj.name}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, $"{path}/{obj.name}.prefab", InteractionMode.AutomatedAction);


        //读取UI配置文件
        string xmlPath = $"{Application.dataPath}/Resources/Config/ResourcesConfig.xml";
        List<string> xml = new List<string>(File.ReadAllLines(xmlPath));

        //判断是否存在该UI
        string str = xml.Find(x => { return x.Contains(obj.name); });
        if (str == null)
        {
            xml.RemoveAt(xml.Count - 1);
            xml.RemoveAt(xml.Count - 1);
            xml.Add("    <ResourceItem>");
            xml.Add($"      <name>{obj.name}</name>");
            xml.Add($"      <path>UI/{obj.name}/{obj.name}</path>");
            xml.Add("    </ResourceItem>");
            xml.Add("  </ResourceItems>");
            xml.Add("</ResourceConfig>");
            File.WriteAllLines(xmlPath, xml);
        }

    }

    [MenuItem("GameObject/@CreateUIWindowScript", false, 20)]//创建UI代码
    static void MVCScript()
    {
        GameObject selectObj = Selection.objects.First() as GameObject;

        if (selectObj == null)
        {
            Debug.LogError("未选择UI对象");
            return;
        }
        selectObj.name = selectObj.name.Contains("Window") ? selectObj.name : selectObj.name + "Window";
        CreateScript(selectObj);

        CreateAndApplyUIPrefab();
    }

    static void CreateScript(GameObject obj)
    {
        string[] con = GetUi(obj);
        string uicon = con[0];
        // string findUIcon = con[1];

        string tmp_con = Resources.Load<TextAsset>(windotScriptTemplate).text;
        string tmp_curClassPanelCon = tmp_con.Replace("#class_name#", obj.name);
        tmp_curClassPanelCon = tmp_curClassPanelCon.Replace("#ui#", uicon);
        // tmp_curClassPanelCon = tmp_curClassPanelCon.Replace("#uifind#", findUIcon);



        CreateMvcScript(obj.name, tmp_curClassPanelCon);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static string[] GetUi(GameObject obj)
    {

        bindList.Clear();
        SearchBinds("", obj.transform);
        

        string UIConTemple = "public #UIType# #UIName#;";
        string FindUIConTemple = "#UIName# = this.transform.Find(\"#Path#\").GetComponent<#UIType#>();";

        string uiCon = null;
        string findUiCon = null;
        for (int i = 0; i < bindList.Count; i++)
        {
            Bind item = bindList[i];
            string ui_type = item.bind.ToString();
            string[] sp = item.FindPath.Split('/');
            string ui_name = sp.Length > 1 ? sp[sp.Length - 1] : sp[0];
    
            string con;
            con = UIConTemple.Replace("#UIType#", ui_type);
            con = con.Replace("#UIName#", ui_name);

            uiCon += i != bindList.Count - 1 ? con + "\n    " : con;


            con = null;
            con = FindUIConTemple.Replace("#UIType#", ui_type);
            con = con.Replace("#UIName#", ui_name);
            con = con.Replace("#Path#", item.FindPath);
            findUiCon += i != bindList.Count - 1 ? con + "\n        " : con;

            // GameObject.DestroyImmediate(item);
        }
        return new string[] { uiCon, findUiCon };
    }
    static void SearchBinds(string path, Transform transform)
    {   
        var bind = transform.GetComponent<Bind>();
        var isRoot = string.IsNullOrWhiteSpace(path);
        if (bind && !isRoot)
        {
            bind.FindPath = path;
            bindList.Add(bind);
        }

        foreach (Transform childTrans in transform)
        {
            SearchBinds(isRoot ? childTrans.name : path + "/" + childTrans.name, childTrans);
        }
    }
    static string GetTemplate(string path)
    {
        return File.ReadAllText(path);
    }
  
    static void CreateMvcScript(string name, string con)
    {
        if (!name.Contains("Window"))
            name += "Window";
        
        string path = Path.Combine(uiScriptPath, name);
        string script = Path.Combine(path, name + ".cs");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        if (!File.Exists(script))
        {
            using (FileStream fs = File.Create(script))
            {
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(con);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        else
        {
            using (FileStream fs = File.OpenWrite(script))
            {
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(con);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
       

    }
}
