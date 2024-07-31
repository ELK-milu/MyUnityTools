using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemEvent : MonoBehaviour {

    public static Dictionary<UIEvent, UICallBack> s_allUIEvents = new Dictionary<UIEvent, UICallBack>();
    public static Dictionary<string, Dictionary<UIEvent, UICallBack>> s_singleUIEvents = new Dictionary<string, Dictionary<UIEvent, UICallBack>>();

    public static void Dispatch(UIWindowBase uiBase, UIEvent uiEvent,params object[] objs)
    {
        if (uiBase == null)
        {
            Debug.LogError("Dispatch ui is null !");
            return;
        }

        if (s_allUIEvents.ContainsKey(uiEvent))
        {
            try
            {
                if (s_allUIEvents[uiEvent] != null) s_allUIEvents[uiEvent](uiBase, objs);
            }
            catch (Exception ex)
            {
                Debug.LogError("UISystemEvent dispatch allUIEvent error :" + ex.ToString());
            }
        }

        if (s_singleUIEvents.ContainsKey(uiBase.name))
        {
            if (s_singleUIEvents[uiBase.name].ContainsKey(uiEvent))
            {
                try
                {
                    if (s_singleUIEvents[uiBase.name][uiEvent] != null) s_singleUIEvents[uiBase.name][uiEvent](uiBase, objs);
                }
                catch (Exception ex)
                {
                    Debug.LogError("UISystemEvent dispatch singleUIEvents error :" + ex.ToString());
                }
            }
        }
    }
}
