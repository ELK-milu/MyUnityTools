using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeapObjectPool
{
    public static Dictionary<string, object> GetSODict()
    {
        return HeapObjectPool<Dictionary<string, object>>.GetObject();
    }

    public static void PutSODict(Dictionary<string, object> dict)
    {
        dict.Clear();
        HeapObjectPool<Dictionary<string, object>>.PutObject(dict);
    }
}

#region HeapObject

public interface IHeapObjectInterface
{
    void OnInit();

    void OnPop();

    void OnPush();
}

#endregion

public static class HeapObjectPool<T> where T : new()
{
    static Stack<T> pool = new Stack<T>();

    public static int GetCount()
    {
        return pool.Count;
    }

    public static T GetObject()
    {
        T obj;
        IHeapObjectInterface heapObj;
        if (pool.Count > 0)
        {
            obj = pool.Pop();
            heapObj = obj as IHeapObjectInterface;
        }
        else
        {
            obj = new T();
            heapObj = obj as IHeapObjectInterface;
            if (heapObj != null)
            {
                heapObj.OnInit();
            }
        }

        if (heapObj != null)
        {
            heapObj.OnPop();
        }

        return obj;
    }

    public static void PutObject(T obj)
    {
        IHeapObjectInterface heapObj = obj as IHeapObjectInterface;
        if (heapObj != null)
        {
            heapObj.OnPush();
        }

        pool.Push(obj);
    }
}

