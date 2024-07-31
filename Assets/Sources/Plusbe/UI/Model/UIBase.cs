using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour {

    public Canvas canvas;
    public List<GameObject> m_objectList = new List<GameObject>();

    #region 重载方法

    //第一次打开调用，优先级在OnOpen之前
    public virtual void OnInit() { }

    public virtual void OnUIDestroy() { }

    public void DestroyUI()
    {
        RemoveAllListener();

        try
        {
            OnUIDestroy();
        }
        catch (Exception e)
        {
            Debug.LogError("UIBase Dispose Exception -> UIEventKey: " + UIEventKey + " Exception: " + e.ToString());
        }
    }

    #endregion

    #region id初始化

    private int uiID = -1;
    private string uiName = null;

    public int UIID
    {
        get { return uiID; }
    }

    public string UIName
    {
        get 
        {
            if (uiName == null)
            {
                uiName = name;
            }
            return uiName;
        }

        set 
        {
            uiName = value;
        }
    }

    public string UIEventKey
    {
        get { return UIName + UIID; }
    }

    public void Init(int id)
    {
        uiID = id;
        canvas = GetComponent<Canvas>();
        uiName = null;
        CreateObjectTable();
        OnInit();
    }

    #endregion

    private void CreateObjectTable()
    {
        m_objects.Clear();

        m_buttons.Clear();

        for (int i = 0; i < m_objectList.Count; i++)
        {
            if (m_objectList[i] != null)
            {
                //Debug.Log("===>"+m_objectList[i].name);
                if (m_objects.ContainsKey(m_objectList[i].name))
                {
                    Debug.LogError("CreateObjectTable ContainsKey ->" + m_objectList[i].name + "<-");
                }
                else
                {
                    m_objects.Add(m_objectList[i].name, m_objectList[i]);
                }
            }
            else
            {
                Debug.LogWarning(name + " m_objectList[" + i + "] is Null !");
            }
        }
    }

    protected List<InputEventRegisterInfo> m_OnClickEvents = new List<InputEventRegisterInfo>();
    protected List<Enum> m_EventNames = new List<Enum>();


    #region 添加监听

    bool GetRegister(List<InputEventRegisterInfo> list, string eventKey)
    {
        int registerCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].eventKey == eventKey)
            {
                registerCount++;
            }
        }

        return registerCount == 0;
    }

    public void AddOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
    {
        InputButtonClickRegisterInfo info = InputUIEventProxy.GetOnClickListener(GetButton(buttonName), UIEventKey, buttonName, parm, callback);
        info.AddListener(GetRegister(m_OnClickEvents, info.eventKey));
        m_OnClickEvents.Add(info);
    }

    #endregion

    #region 移除监听

    //TODO 逐步添加所有的移除监听方法

    public virtual void RemoveAllListener()
    {
        for (int i = 0; i < m_OnClickEvents.Count; i++)
        {
            m_OnClickEvents[i].RemoveListener(true);
        }
        m_OnClickEvents.Clear();

    }

    public InputButtonClickRegisterInfo GetClickRegisterInfo(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm)
    {
        string eventKey = InputUIOnClickEvent.GetEventKey(UIEventKey, buttonName, parm);
        for (int i = 0; i < m_OnClickEvents.Count; i++)
        {
            InputButtonClickRegisterInfo info = (InputButtonClickRegisterInfo)m_OnClickEvents[i];
            if (info.eventKey == eventKey
                && info.callBack == callback)
            {
                return info;
            }
        }

        throw new Exception("GetClickRegisterInfo Exception not find RegisterInfo by " + buttonName + " parm " + parm);
    }



    public void RemoveOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
    {
        InputButtonClickRegisterInfo info = GetClickRegisterInfo(buttonName, callback, parm);
        m_OnClickEvents.Remove(info);
        info.RemoveListener(GetRegister(m_OnClickEvents, info.eventKey));
    }

    #endregion

    Dictionary<string, GameObject> m_objects = new Dictionary<string, GameObject>();

    Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();

    public GameObject GetGameObject(string name)
    {
        if (m_objects == null)
        {
            CreateObjectTable();
        }

        if (m_objects.ContainsKey(name))
        {
            GameObject go = m_objects[name];

            if (go == null)
            {
                throw new Exception("UIWindowBase GetGameObject error: " + UIName + " m_objects[" + name + "] is null !!");
            }

            return go;
        }
        else
        {
            throw new Exception("UIWindowBase GetGameObject error: " + UIName + " dont find ->" + name + "<-");
        }
    }

    public Button GetButton(string name)
    {
        if (m_buttons.ContainsKey(name))
        {
            return m_buttons[name];
        }

        Button tmp = GetGameObject(name).GetComponent<Button>();

        if (tmp == null)
        {
            throw new Exception(m_EventNames + " GetButton ->" + name + "<- is Null !");
        }

        m_buttons.Add(name, tmp);
        return tmp;
    }
}
