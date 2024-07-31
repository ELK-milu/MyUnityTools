using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static GameObject s_UIManagerGo;

    public static UILayerManager s_UILayerManager;
    public static UIAnimManager s_UIAnimManager;
    public static UIStackManager s_UIStackManager;
    public static Camera s_UICamera;
    

    public static Dictionary<string, List<UIWindowBase>> s_UIs = new Dictionary<string, List<UIWindowBase>>();
    public static Dictionary<string, List<UIWindowBase>> s_HideUIs = new Dictionary<string, List<UIWindowBase>>();

    #region 初始化

    public static void Init()
    {
        GameObject instance = GameObject.Find("UIManager");

        if (instance == null)
        {
            instance = GameObjectManager.CreateGameObject("UIManager");
        }

        SetUIManager(instance);
    }

    public static void InitAsync()
    {
        GameObject instance = GameObject.Find("UIManager");

        if (instance == null)
        {
            GameObjectManager.CreateGameObjectByPoolAsync("UIManager", (obj) =>
            {
                SetUIManager(obj);
            });
        }
        else
        {
            SetUIManager(instance);
        }
    }

    private static void SetUIManager(GameObject instance)
    {
        s_UIManagerGo = instance;

        s_UILayerManager = instance.GetComponent<UILayerManager>();
        s_UIAnimManager = instance.GetComponent<UIAnimManager>();
        s_UIStackManager = instance.GetComponent<UIStackManager>();
        s_UICamera = instance.GetComponentInChildren<Camera>();

        DontDestroyOnLoad(instance);
    }

    #endregion

    #region UI的打开与关闭

    #region 创建
    /// <summary>
    /// 创建一个UI，一般用于部分初始化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateUIWindowOne<T>() where T:UIWindowBase
    {
        return (T)CreateUIWindowOne(typeof(T).Name);
    }

    public static UIWindowBase CreateUIWindowOne(string uiName)
    {
        UIWindowBase uiBase = GetHideUI(uiName);

        if (uiBase == null)
        {
            uiBase = CreateUIWindow(uiName);
        }

        return uiBase;
    }

    public static T CreateUIWindow<T>() where T : UIWindowBase
    {
        return (T)CreateUIWindow(typeof(T).Name);
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public static UIWindowBase CreateUIWindow(string uiName)
    {
        GameObject uiTemp = GameObjectManager.CreateGameObject(uiName, s_UIManagerGo);
        // GameObject uiTemp = GameObjectManager.CreateGameObject(Resources.Load<GameObject>(), s_UIManagerGo);
        UIWindowBase uiBase = uiTemp.GetComponent<UIWindowBase>();
        UISystemEvent.Dispatch(uiBase, UIEvent.OnInit);  //OnInit初始化事件派发


        uiBase.windowStatus = UIWindowBase.WindowStatus.Create;

        try
        {
            uiBase.Init(GetUIID(uiName));
        }
        catch (Exception ex)
        {
            Debug.LogError("OnInit error " + ex.ToString());
        }

        AddHideUI(uiBase);

        s_UILayerManager.SetLayer(uiBase);

        return uiBase;
    }

    #endregion

    public static T OpenUIWindow<T>() where T : UIWindowBase
    {
        return (T)OpenUIWindow(typeof(T).Name,true);
    }

    public static T OpenUIWindow<T>(UIWindowBase uiBaseClose) where T : UIWindowBase
    {
        CloseUIWindow(uiBaseClose);
        return (T)OpenUIWindow(typeof(T).Name);
    }

    public static T OpenUIWindow<T>(bool OnlyOne) where T : UIWindowBase
    {
        return (T)OpenUIWindow(typeof(T).Name, OnlyOne);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uiName">name</param>
    /// <param name="OnlyOne">遍历hides+uis，只生成一个</param>
    /// <param name="callBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public static UIWindowBase OpenUIWindow(string uiName, bool onlyOne = true, UICallBack callBack = null, params object[] objs)
    {
        UIWindowBase uiBase = GetHideUI(uiName);

        bool isInUI = false;

        if (onlyOne && uiBase == null)
        {
            isInUI = true;
            uiBase = GetUI(uiName);
        }
        else if (uiBase != null)
        {
            RemoveHideUI(uiBase);
            AddUI(uiBase);
        }

        if (uiBase == null)
        {
            isInUI = false;
            uiBase = CreateUIWindow(uiName);
            RemoveHideUI(uiBase);
            AddUI(uiBase);
        }

        s_UIStackManager.OnUIOpen(uiBase);
        s_UILayerManager.SetLayer(uiBase);

        uiBase.windowStatus = UIWindowBase.WindowStatus.OpenAnim;

        UISystemEvent.Dispatch(uiBase, UIEvent.OnOpen);

        try
        {
            if(!isInUI)
                uiBase.OnOpen();
        }
        catch (Exception ex)
        {
            Debug.LogError(uiName + " OnOpen Exception :" + ex.ToString());
        }

        s_UIAnimManager.StartEnterAnim(uiBase, callBack, objs);

        return uiBase;
    }

   

    public static void CloseUIWindow(UIWindowBase uiBase, bool isPlayAnim = true, UICallBack callBack = null, params object[] objs)
    {
        //RemoveUI(uiBase);
        //uiBase.RemoveAllEventListener();
        //s_UILayerManager.RemoveUI(uiBase);

        //AddHideUI(uiBase); //ccj

        if (isPlayAnim)
        {
            if (callBack != null)
            {
                callBack += CloseUIWindowCallBack;
            }
            else
            {
                callBack = CloseUIWindowCallBack;
            }

            
            s_UIAnimManager.StartExitAnim(uiBase,callBack,objs);
            uiBase.windowStatus = UIWindowBase.WindowStatus.CloseAnim;
        }
        else
        {
            CloseUIWindowCallBack(uiBase, objs);
        }
    }

    public static void CloseUIWindow(string uiName, bool isPlayAnim = true, UICallBack callBack = null, params object[] objs)
    { 
        UIWindowBase uiBase = GetUI(uiName);

        if (uiBase == null)
        {
            Debug.LogWarning("UIManager CloseUIWindow error dont find ui ->" + uiName + "<-");
            //Debug.LogError("UIManager CloseUIWindow error dont find ui ->" + uiName + "<-");
        }
        else
        {
            CloseUIWindow(uiBase, isPlayAnim, callBack, objs);
        }
    }

    public static void CloseUIWindow<T>(bool isPlayAnim = true, UICallBack callBack = null, params object[] objs) where T : UIWindowBase
    {
        CloseUIWindow(typeof(T).Name, isPlayAnim, callBack, objs);
    }

    private static void CloseUIWindowCallBack(UIWindowBase uiBase, params object[] objs)
    {
        RemoveUI(uiBase);
        uiBase.RemoveAllEventListener();
        s_UILayerManager.RemoveUI(uiBase);

        uiBase.windowStatus = UIWindowBase.WindowStatus.Close;

        UISystemEvent.Dispatch(uiBase, UIEvent.OnClose);
        try
        {
            uiBase.OnClose();
        }
        catch (Exception ex)
        {
            Debug.LogError(uiBase.UIName + "OnClose Exception : " + ex.ToString());
        }
        s_UIStackManager.OnUIClose(uiBase);
        AddHideUI(uiBase);
    }

    public static UIWindowBase ShowUI(string uiName)
    {
        UIWindowBase uiBase = GetUI(uiName);
        return ShowUI(uiBase);
    }

    public static UIWindowBase ShowUI(UIWindowBase uiBase)
    {
        uiBase.windowStatus = UIWindowBase.WindowStatus.Open;
        UISystemEvent.Dispatch(uiBase, UIEvent.OnShow);
        try
        {
            uiBase.Show();
            uiBase.OnShow();
        }
        catch (Exception ex)
        {
            Debug.LogError("UIManager OnShow Exception ->" + uiBase.UIName + "<-" + ex.ToString());
        }

        return uiBase;
    }

    public static UIWindowBase HideUI(string uiName)
    {
        return HideUI(GetUI(uiName));
    }

    public static UIWindowBase HideUI(UIWindowBase uiBase)
    {
        uiBase.windowStatus = UIWindowBase.WindowStatus.Hide;
        UISystemEvent.Dispatch(uiBase, UIEvent.OnHide);
        try
        {
            uiBase.Hide();
            uiBase.OnHide();
        }
        catch (Exception ex)
        {
            Debug.LogError("UIManager OnHide Exception ->" + uiBase.UIName + "<-" + ex.ToString());
        }
        return uiBase;
    }

    public static void HideOtherUI<T>() where T : UIWindowBase
    {
        HideOtherUI(typeof(T).Name);
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="uiName"></param>
    public static void HideOtherUI(string uiName)
    {
        List<string> keys = new List<string>(s_UIs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            List<UIWindowBase> list = s_UIs[keys[i]];
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].UIName != uiName)
                {
                    HideUI(list[j]);
                }
            }
        }

        //keys = new List<string>(s_UIs.Keys);
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    List<UIWindowBase> list = s_UIs[keys[i]];
        //    for (int j = 0; j < list.Count; j++)
        //    {
        //        if (list[j].UIName != uiName)
        //        {
        //            HideUI(list[j]);
        //        }
        //    }
        //}
    }

    public static void ShowOtherUI(string uiName)
    {
        List<string> keys = new List<string>(s_UIs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            List<UIWindowBase> list = s_UIs[keys[i]];
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].UIName != uiName)
                {
                    ShowUI(list[j]);
                }
            }
        }
    }

    public static void CloseAllUI(bool isPlayAnim = false)
    {
        List<string> keys = new List<string>(s_UIs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            List<UIWindowBase> list = s_UIs[keys[i]];
            for (int j = 0; j < list.Count; j++)
            {
                CloseUIWindow(list[i], isPlayAnim);
            }
        }
    }

    public static void CloseLastUI(UIType uiType = UIType.Normal)
    {
        s_UIStackManager.CloseLastUIWindow(uiType);
    }

    private static void AddUI(UIWindowBase uiBase)
    { 
        if(!s_UIs.ContainsKey(uiBase.name))
        {
            s_UIs.Add(uiBase.name, new List<UIWindowBase>());
        }

        s_UIs[uiBase.name].Add(uiBase);

        uiBase.Show();
    }

    private static void RemoveUI(UIWindowBase uiBase)
    {
        if (uiBase == null)
        {
            throw new Exception("UIManager : RemoveUI error ui is null ");
        }

        if (!s_UIs.ContainsKey(uiBase.name) || !s_UIs[uiBase.name].Contains(uiBase))
        {
            throw new Exception("UIManager : RemoveUI error dont find ui ->" + uiBase.name + "<-" + uiBase);  
        }

        //if (!s_UIs[uiBase.name].Contains(uiBase))
        //{
        //    throw new Exception("UIManager : RemoveUI error dont find ui ->" + uiBase.name + "<-" + uiBase);  
        //}

        s_UIs[uiBase.name].Remove(uiBase);

    }


    private static int GetUIID(string uiName)
    {
        if (!s_UIs.ContainsKey(uiName))
        {
            return 0;
        }
        else
        {
            int id = s_UIs[uiName].Count;
            for (int i = 0; i < s_UIs[uiName].Count; i++)
            {
                if (s_UIs[uiName][i].UIID == id)
                {
                    id++;
                    i = 0;
                }
            }

            return id;
        }
    }

    public static UIWindowBase GetUI(string uiName)
    {
        if (!s_UIs.ContainsKey(uiName)) return null;
        if (s_UIs[uiName].Count == 0) return null;
        return s_UIs[uiName][s_UIs[uiName].Count - 1] ;
    }

    public static T GetUI<T>() where T : UIWindowBase
    {
        return (T)GetUI(typeof(T).Name);
    }

    public static void DestoryAllActiveUI()
    {
        foreach (List<UIWindowBase> uis in s_UIs.Values)
        {
            for (int i = 0; i < uis.Count; i++)
            {
                try
                {
                    uis[i].DestroyUI();
                }
                catch
                {
                    Debug.LogError("UIManager error DestroyActiveUI :" + uis[i].name);
                }

                Destroy(uis[i].gameObject);
            }
        }

        s_UIs.Clear();
    }

    public static void DestroyAllHideUI()
    {
        foreach (List<UIWindowBase> uis in s_HideUIs.Values)
        {
            for (int i = 0; i < uis.Count; i++)
            {
                try
                {
                    uis[i].DestroyUI();
                }
                catch
                {
                    Debug.LogError("UIManager error DestroyHideUI :" + uis[i].name);
                }

                Destroy(uis[i].gameObject);
            }
        }

        s_HideUIs.Clear();
    }


    #endregion

    #region UI内存管理

    public static void DestroyUI(UIWindowBase uiBase)
    {
        if (GetHideUI(uiBase.name))
        {
            RemoveHideUI(uiBase);
        }
        else if (GetIsExitHide(uiBase))
        {
            RemoveUI(uiBase);
        }

        UISystemEvent.Dispatch(uiBase, UIEvent.OnDestory);
        try
        {
            uiBase.DestroyUI();
        }
        catch (Exception ex)
        {
            Debug.LogError("UIManager error OnDestroy " + ex.ToString());
        }
        Destroy(uiBase.gameObject);
    }

    public static void DestroyAll()
    {
        DestoryAllActiveUI();
        DestroyAllHideUI();
    }

    #endregion

    #region 隐藏列表的管理

    private static UIWindowBase GetHideUI(string uiName)
    {
        if (!s_HideUIs.ContainsKey(uiName))
        {
            return null;
        }
        else
        {
            if (s_HideUIs[uiName].Count == 0)
            {
                return null;
            }
            else
            {
                return s_HideUIs[uiName][s_HideUIs[uiName].Count - 1];
            }
        }
    }

    private static bool GetIsExitHide(UIWindowBase uiBase)
    {
        if (!s_HideUIs.ContainsKey(uiBase.name))
        {
            return false;
        }
        return s_HideUIs[uiBase.name].Contains(uiBase);
    }

    private static void AddHideUI(UIWindowBase uiBase)
    {
        if (!s_HideUIs.ContainsKey(uiBase.name))
        {
            s_HideUIs.Add(uiBase.name, new List<UIWindowBase>());
        }

        s_HideUIs[uiBase.name].Add(uiBase);

        uiBase.Hide();
    }

    private static void RemoveHideUI(UIWindowBase uiBase)
    {
        if (uiBase == null)
        {
            throw new Exception("UIManager:RemoveUI error ui is null !");
        }

        if (!s_HideUIs.ContainsKey(uiBase.name))
        {
            throw new Exception("UIManager:RemoveUI error dont find :" + uiBase.name + "--" + uiBase);
        }

        if (!s_HideUIs[uiBase.name].Contains(uiBase))
        {
            throw new Exception("UIManager:RemoveUI error dont find :" + uiBase.name + "--" + uiBase);
        }

        s_HideUIs[uiBase.name].Remove(uiBase);
    }

    #endregion
}



public delegate void UICallBack(UIWindowBase uiBase,params object[] objs);
public delegate void UIAnimCallBack(UIWindowBase uiBase,UICallBack callBack,params object[] objs);

public enum UIType
{ 
    GameUI,
    Fixed,
    Normal,
    TopBar,
    PopUp
}

public enum UIEvent
{ 
    OnOpen,
    OnClose,
    OnShow,
    OnHide,

    OnInit,
    OnDestory,

    OnRefresh,

    OnStartEnterAnim,
    OnCompleteEnterAnim,

    OnStartExitAnim,
    OnCompleteExitAnim
}