using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class IInputProxyBase
{
    private static bool isActive = true;

    public static bool IsActive
    {
        get { return IInputProxyBase.isActive; }
        set { IInputProxyBase.isActive = value; }
    }

    public static T GetEvent<T>(string eventKey) where T : IInputEventBase, new()
    {
        T temp = HeapObjectPool<T>.GetObject();
        temp.EventKey = eventKey;

        return temp;
    }
}

public class InputEventRegisterInfo : IHeapObjectInterface
{
    public string eventKey;
    public void OnInit() { }
    public void OnPop() { }
    public void OnPush() { }

    public virtual void AddListener(bool isSole) { }

    public virtual void RemoveListener(bool isSole) { }
}

public class InputEventRegisterInfo<T> :InputEventRegisterInfo where T : IInputEventBase
{
    public InputEventHandle<T> callBack;

    public InputEventRegisterInfo()
    { 
    }

    public override void AddListener(bool isSole)
    {
        //base.AddListener(isSole);
        InputManager.AddListener<T>(eventKey, callBack);
    }

    /// <summary>
    /// 移除监听和派发
    /// </summary>
    /// <param name="isRegister">这是不是这个eventKey最后一个监听事件，如果是则移除派发</param>
    public override void RemoveListener(bool isSole)
    {
        InputManager.RemoveListener<T>(eventKey, callBack);
        HeapObjectPool<InputEventRegisterInfo<T>>.PutObject(this);
    }
}

