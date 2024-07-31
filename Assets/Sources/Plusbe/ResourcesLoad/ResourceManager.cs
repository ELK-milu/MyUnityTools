using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static T Load<T>(string name) where T : UnityEngine.Object
    {
        return Resources.Load<T>(name);
    }

    public static void LoadAsync(string name, LoadCallBack callBack)
    {
        ResourceIOTool.ResourceLoadAsync(name, callBack);
    }
}
