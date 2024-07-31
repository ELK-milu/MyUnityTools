using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

   // private static Vector3 s_OutOfRange = new Vector3(9000, 9000, 9000);

    private static Transform s_poolParent;

    public static Transform PoolParent
    {
        get
        {
            if (s_poolParent == null)
            {
                GameObject instancePool = new GameObject("ObjectPool");
                s_poolParent = instancePool.transform;
                if (Application.isPlaying) DontDestroyOnLoad(s_poolParent);
            }

            return s_poolParent;
        }
    }

    private static Dictionary<string, List<GameObject>> s_objectPool = new Dictionary<string, List<GameObject>>();

    public static GameObject CreateGameObject(string gameObjectName, GameObject parent = null)
    {
        GameObject goTemp = ResourceManager.Load<GameObject>(ResourceConfigManager.GetPath(gameObjectName));
        if (goTemp == null)
        {
            throw new Exception("CreateGameObject error dont find : " + gameObjectName);
        }

        return CreateGameObject(goTemp, parent);
    }

    public static GameObject CreateGameObject(GameObject prefab, GameObject parent = null)
    {
        if (prefab == null)
        {
            throw new Exception(" CreateGameObject error : prefab is null ");
        }

        GameObject goTemp = Instantiate(prefab);
        goTemp.name = prefab.name;
        if (parent != null)
        {
            goTemp.transform.SetParent(parent.transform);
        }
        return goTemp;
    }

    public static void CreateGameObjectByPoolAsync(string name, CallBack<GameObject> callBack, GameObject parent = null, bool isSetActive = true)
    {
        ResourceManager.LoadAsync(name, (status, res) =>
        {
            if (status.isDone)
            {
                try
                {
                    callBack(CreatGameObjectByPool(name, parent, isSetActive));
                }
                catch (Exception ex)
                {
                    Debug.LogError("CreateGameObjectByPoolAsync Exception : " + ex.ToString());
                }
            }
        });
    }

    public static GameObject CreatGameObjectByPool(string name,GameObject parent = null, bool isSetActive = true)
    {
        GameObject go = null;
        if (IsExist(name))
        {
            go = s_objectPool[name][0];
            s_objectPool[name].RemoveAt(0);
        }
        else
        {
            go = CreateGameObject(name, parent);
        }

        if (isSetActive) go.SetActive(true);
        if (parent == null)
        {
            go.transform.SetParent(null);
        }
        else
        {
            go.transform.SetParent(parent.transform);
        }

        return go;
    }

    public static bool IsExist(string objectName)
    {
        if(string.IsNullOrEmpty(objectName))
        {
            Debug.LogError("GameObjectManager objectName is null !");
            return false;
        }

        if (s_objectPool.ContainsKey(objectName) && s_objectPool[objectName].Count > 0)
        {
            return true;
        }

        return false;
    }
}
