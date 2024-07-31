using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayerManager : MonoBehaviour {

    public Transform gameUILayerParent;
    public Transform fixedLayerParent;
    public Transform normalLayerParent;
    public Transform topBarLayerParent;
    public Transform popUpLayerParent;

    public List<UIWindowBase> normalUIList = new List<UIWindowBase>();

    public void Awake()
    {
        if (gameUILayerParent == null)
        {
            Debug.LogError("UILayerManager : gameUILayerParent is null !");
        }

        if (fixedLayerParent == null)
        {
            Debug.LogError("UILayerManager : fixedLayerParent is null !");
        }

        if (normalLayerParent == null)
        {
            Debug.LogError("UILayerManager : normalLayerParent is null !");
        }

        if (topBarLayerParent == null)
        {
            Debug.LogError("UILayerManager : topBarLayerParent is null !");
        }

        if (popUpLayerParent == null)
        {
            Debug.LogError("UILayerManager : popUpLayerParent is null !");
        }
    }

    public void SetLayer(UIWindowBase uiBase)
    {
        RectTransform rt = uiBase.GetComponent<RectTransform>();
        switch (uiBase.m_UIType)
        {
            case UIType.GameUI: uiBase.transform.SetParent(gameUILayerParent); break;
            case UIType.Fixed: uiBase.transform.SetParent(fixedLayerParent); break;
            case UIType.Normal: uiBase.transform.SetParent(normalLayerParent); normalUIList.Add(uiBase); break;
            case UIType.TopBar: uiBase.transform.SetParent(topBarLayerParent); break;
            case UIType.PopUp: uiBase.transform.SetParent(popUpLayerParent); break;
        }

        rt.localScale = Vector3.one;
        rt.sizeDelta = Vector2.zero;

        if (uiBase.m_UIType != UIType.GameUI)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector3.one;

            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector3.zero;

            rt.SetAsLastSibling();
        }
    }

    public void RemoveUI(UIWindowBase uiBase)
    {
        switch (uiBase.m_UIType)
        {
            case UIType.GameUI:
            case UIType.Fixed:
            case UIType.TopBar:
            case UIType.PopUp: break;

            case UIType.Normal: normalUIList.Remove(uiBase); break;
        }
    }


}
