using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStackManager : MonoBehaviour {

    private List<UIWindowBase> fixedStack = new List<UIWindowBase>();
    private List<UIWindowBase> normalStack = new List<UIWindowBase>();
    private List<UIWindowBase> popUpStack = new List<UIWindowBase>();
    private List<UIWindowBase> topBarStack = new List<UIWindowBase>();

    public void OnUIOpen(UIWindowBase uiBase)
    {
        switch (uiBase.m_UIType)
        {
            case UIType.Fixed: fixedStack.Add(uiBase); break;
            case UIType.Normal: normalStack.Add(uiBase); break;
            case UIType.PopUp: popUpStack.Add(uiBase); break;
            case UIType.TopBar: topBarStack.Add(uiBase); break;
        }
    }

    public void OnUIClose(UIWindowBase uiBase)
    {
        switch (uiBase.m_UIType)
        {
            case UIType.Fixed: fixedStack.Remove(uiBase); break;
            case UIType.Normal: normalStack.Remove(uiBase); break;
            case UIType.PopUp: popUpStack.Remove(uiBase); break;
            case UIType.TopBar: topBarStack.Remove(uiBase); break;
        }
    }

    public void CloseLastUIWindow(UIType uiType)
    {
        UIWindowBase uiBase = GetLastUI(uiType);

        if (uiBase != null)
        {
            UIManager.CloseUIWindow(uiBase);
        }
    }

    public UIWindowBase GetLastUI(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.Fixed: if (fixedStack.Count > 0) return fixedStack[fixedStack.Count - 1];
                else return null;
            case UIType.Normal: if (normalStack.Count > 0) return normalStack[normalStack.Count - 1];
                else return null;
            case UIType.PopUp: if (popUpStack.Count > 0) return popUpStack[popUpStack.Count - 1];
                else return null;
            case UIType.TopBar: if (topBarStack.Count > 0) return topBarStack[topBarStack.Count - 1];
                else return null;
        }

        throw new Exception("CloseLastUIWindow dont support GameUI");
    }
}
