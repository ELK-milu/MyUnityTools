using UnityEngine;

/// <summary>
/// Mono类单例模式基类模块
/// </summary>
/// <typeparam name="T">泛型 传入类对象</typeparam>
/// 泛型必须满足有一个无参构造函数形式的约束
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Inseance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                    Debug.LogError("场景中未找到类的对象，类名为:" + typeof(T).Name);
            }

            return _instance;
        }
    }


    private static bool origional = true;

    protected virtual void Awake()
    {
        //if (_instance == null)
        //    DontDestroyOnLoad(gameObject);
        //else
        //    Destroy(gameObject);

        if (origional)
        {
            _instance = this as T;
            origional = false;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
