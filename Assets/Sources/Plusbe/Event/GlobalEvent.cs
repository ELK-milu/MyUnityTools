using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Event
{
    public class GlobalEvent
    {
        #region 以Enum为key的事件派发

        public static Dictionary<Enum, EventHandle> eventDic = new Dictionary<Enum, EventHandle>();
        public static Dictionary<Enum, List<EventHandle>> useOnceEventDic = new Dictionary<Enum, List<EventHandle>>();

        public static void AddEvent(Enum type, EventHandle handle, bool isUseOnce = false)
        {
            if (isUseOnce)
            {
                if (useOnceEventDic.ContainsKey(type))
                {
                    if (!useOnceEventDic[type].Contains(handle))
                    {
                        useOnceEventDic[type].Add(handle);
                    }
                    else
                    {
                        Debug.LogWarning("already existing eventtype ->" + type + "<-handle->" + handle + "<-");
                    }
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    useOnceEventDic.Add(type, temp);
                }
            }
            else
            {
                if (eventDic.ContainsKey(type))
                {
                    eventDic[type] += handle;
                }
                else
                {
                    eventDic.Add(type, handle);
                }
            }
        }

        public static void RemoveEvent(Enum type, EventHandle handle)
        {
            if (useOnceEventDic.ContainsKey(type))
            {
                if (useOnceEventDic[type].Contains(handle))
                {
                    useOnceEventDic[type].Remove(handle);
                    if (useOnceEventDic[type].Count == 0)
                    {
                        useOnceEventDic.Remove(type);
                    }
                }
            }

            if (eventDic.ContainsKey(type))
            {
                eventDic[type] -= handle;
            }
        }

        internal static void AddTypeEvent<T>()
        {
            throw new NotImplementedException();
        }

        public static void RemoveEvent(Enum type)
        {
            if (useOnceEventDic.ContainsKey(type))
            {
                useOnceEventDic.Remove(type);
            }

            if (eventDic.ContainsKey(type))
            {
                eventDic.Remove(type);
            }
        }

        public static void DispatchEvent(Enum type, params object[] args)
        {
            if (eventDic.ContainsKey(type))
            {
                try
                {
                    if (eventDic[type] != null)
                    {
                        eventDic[type](args);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("GlobalEvent dispatchEvent error :" + ex.ToString());
                }
            }

            if (useOnceEventDic.ContainsKey(type))
            {
                for (int i = 0; i < useOnceEventDic[type].Count; i++)
                {
                    //???
                    foreach (EventHandle handle in useOnceEventDic[type][i].GetInvocationList())
                    {
                        try
                        {
                            handle(args);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("GlobalEvent dispatchEvent error :" + ex.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region 复制 以String为Key的事件派发
        private static Dictionary<string, List<EventHandle>> m_stringEventDic = new Dictionary<string, List<EventHandle>>();
        private static Dictionary<string, List<EventHandle>> m_stringOnceEventDic = new Dictionary<string, List<EventHandle>>();

        /// <summary>
        /// 添加事件及回调
        /// </summary>
        /// <param name="type">事件枚举</param>
        /// <param name="handle">回调</param>
        /// <param name="isUseOnce"></param>
        public static void AddEvent(string eventKey, EventHandle handle, bool isUseOnce = false)
        {
            if (isUseOnce)
            {
                if (m_stringOnceEventDic.ContainsKey(eventKey))
                {
                    if (!m_stringOnceEventDic[eventKey].Contains(handle))
                        m_stringOnceEventDic[eventKey].Add(handle);
                    else
                        Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    m_stringOnceEventDic.Add(eventKey, temp);
                }
            }
            else
            {
                if (m_stringEventDic.ContainsKey(eventKey))
                {
                    if (!m_stringEventDic[eventKey].Contains(handle))
                        m_stringEventDic[eventKey].Add(handle);
                    else
                        Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    m_stringEventDic.Add(eventKey, temp);
                }
            }
        }

        /// <summary>
        /// 移除某类事件的一个回调
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handle"></param>
        public static void RemoveEvent(string eventKey, EventHandle handle)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {
                if (m_stringEventDic[eventKey].Contains(handle))
                {
                    m_stringEventDic[eventKey].Remove(handle);
                    //if (m_stringEventDic[eventKey].Count == 0)
                    //{
                    //    m_stringEventDic.Remove(eventKey);
                    //}
                }
            }

            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {
                if (m_stringOnceEventDic[eventKey].Contains(handle))
                {
                    m_stringOnceEventDic[eventKey].Remove(handle);
                    //if (m_stringOnceEventDic[eventKey].Count == 0)
                    //{
                    //    m_stringOnceEventDic.Remove(eventKey);
                    //}
                }
            }
        }

        /// <summary>
        /// 移除某类事件
        /// </summary>
        /// <param name="eventKey"></param>
        public static void RemoveEvent(string eventKey)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {

                m_stringEventDic.Remove(eventKey);
            }

            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {

                m_stringOnceEventDic.Remove(eventKey);
            }
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        public static void RemoveAllEvent()
        {
            useOnceEventDic.Clear();

            eventDic.Clear();

            m_stringEventDic.Clear();
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="args"></param>
        public static void DispatchEvent(string eventKey, params object[] args)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {
                for (int i = 0; i < m_stringEventDic[eventKey].Count; i++)
                {
                    //遍历委托链表
                    foreach (EventHandle callBack in m_stringEventDic[eventKey][i].GetInvocationList())
                    {
                        try
                        {
                            callBack(args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                }
            }

            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {
                for (int i = 0; i < m_stringOnceEventDic[eventKey].Count; i++)
                {
                    //遍历委托链表
                    foreach (EventHandle callBack in m_stringOnceEventDic[eventKey][i].GetInvocationList())
                    {
                        try
                        {
                            callBack(args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                }
                RemoveEvent(eventKey);
            }
        }
        #endregion

        #region 复制 以Type为Key的事件派发

        private static Dictionary<Type, EventDispatcher> mTypeEventDic = new Dictionary<Type, EventDispatcher>();
        private static Dictionary<Type, EventDispatcher> mTypeUseOnceEventDic = new Dictionary<Type, EventDispatcher>();

        /// <summary>
        /// 添加事件及回调
        /// </summary>
        /// <param name="type">事件枚举</param>
        /// <param name="handle">回调</param>
        /// <param name="isUseOnce"></param>
        public static void AddTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack += handle;
        }

        /// <summary>
        /// 移除某类事件的一个回调
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handle"></param>
        public static void RemoveTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack -= handle;
        }

        /// <summary>
        /// 移除某类事件
        /// </summary>
        /// <param name="type"></param>
        public static void RemoveTypeEvent<T>(bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack = null;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public static void DispatchTypeEvent<T>(T e, params object[] args)
        {
            GetEventDispatcher<T>(false).Call(e, args);

            //只调用一次的调用后就清除
            GetEventDispatcher<T>(true).Call(e, args);
            GetEventDispatcher<T>(true).m_CallBack = null;
        }

        static EventDispatcher<T> GetEventDispatcher<T>(bool isOnce)
        {
            Type type = typeof(T);

            if (isOnce)
            {
                if (mTypeUseOnceEventDic.ContainsKey(type))
                {
                    return (EventDispatcher<T>)mTypeUseOnceEventDic[type];
                }
                else
                {
                    EventDispatcher<T> temp = new EventDispatcher<T>();
                    mTypeUseOnceEventDic.Add(type, temp);
                    return temp;
                }
            }
            else
            {
                if (mTypeEventDic.ContainsKey(type))
                {
                    return (EventDispatcher<T>)mTypeEventDic[type];
                }
                else
                {
                    EventDispatcher<T> temp = new EventDispatcher<T>();
                    mTypeEventDic.Add(type, temp);
                    return temp;
                }
            }
        }

        abstract class EventDispatcher { }

        class EventDispatcher<T> : EventDispatcher
        {
            public EventHandle<T> m_CallBack;

            public void Call(T e, params object[] args)
            {
                if (m_CallBack != null)
                {
                    try
                    {
                        m_CallBack(e, args);
                    }
                    catch (Exception exc)
                    {
                        Debug.LogError(exc.ToString());
                    }
                }
            }
        }

        #endregion
    }

    public class EventHandRegisterInfo
    {
        public Enum eventKey;
        public EventHandle handle;

        public void RemoveListener()
        {
            GlobalEvent.RemoveEvent(eventKey, handle);
        }
    }

    public delegate void EventHandle(params object[] objs);
    public delegate void EventHandle<T>(T e, params object[] objs);
}


