using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Plusbe.Message
{
    //http://wiki.unity3d.com/index.php/NotificationCenterGenerics#Update_Notes

    public class NotificationCenter : MonoBehaviour
    {
        private static NotificationCenter defaultCenter;

        private Hashtable notifications = new Hashtable();

        private List<Notification> newNotifications = new List<Notification>(); 
        private List<Notification> currNotifications = new List<Notification>();

        [Obsolete("Please use <Instance> instead")]
        public static NotificationCenter DefaultCenter()
        {
            //if (!defaultCenter)
            //{
            //    GameObject go = new GameObject("Default Notification Center");
            //    defaultCenter = go.AddComponent<NotificationCenter>();
            //    DontDestroyOnLoad(defaultCenter);
            //}

            //return defaultCenter;

            return Instance;
        }

        public static NotificationCenter Instance
        {
            get
            {
                if (!defaultCenter)
                {
                    defaultCenter = ApplicationManager.Instance.gameObject.AddComponent<NotificationCenter>();
                }

                return defaultCenter;
            }
        }

        public void Init()
        { 
        
        }

        public void AddObserver(Component observer, string name)
        {
            AddObserver(observer, name, null);
        }

        //AddObserver includes a version where the observer can request to only receive notifications from a specific object.  We haven't implemented that yet, so the sender value is ignored for now.
        public void AddObserver(Component observer, string name, object sender)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("Null name specified for notification in AddObserver.");
                return;
            }
            if (!notifications.ContainsKey(name)) notifications[name] = new List<Component>();

            List<Component> notifyList = (List<Component>)notifications[name];
            if (!notifyList.Contains(observer)) notifyList.Add(observer);
        }

        // RemoveObserver removes the observer from the notification list for the specified notification type
        public void RemoveObserver(Component observer, string name)
        {
            List<Component> notifyList = (List<Component>)notifications[name];
            if (notifyList != null)
            {
                if (notifyList.Contains(observer)) notifyList.Remove(observer);
                if (notifyList.Count == 0) notifications.Remove(name);
            }
        }

        public void RemoveAllObserver()
        {
            notifications.Clear();
        }

        public void PostNotification(Component sender, string name)
        {
            PostNotification(sender, name, null);
        }

        public void PostNotification(Component sender,string name,object data)
        {
            PostNotification(new Notification(sender, name, data));
        }

        public void PostNotification(Notification notification)
        {
            newNotifications.Add(notification);
        }
        

        void Update()
        {
            if (newNotifications.Count == 0) return;
            
            lock (newNotifications)
            {
                currNotifications.Clear();
                currNotifications.AddRange(newNotifications);
                newNotifications.Clear();
            }

            foreach (Notification notification in currNotifications)
            {
                if (string.IsNullOrEmpty(notification.name)) { Debug.Log("Null name sent to PostNotification."); return; }
                List<Component> notifyList = (List<Component>)notifications[notification.name];
                if (notifyList == null) { Debug.LogWarning("Notify list not found in PostNotification."); return; }

                notifyList = new List<Component>(notifyList);
                List<Component> observersToRemove = new List<Component>();

                foreach (Component observer in notifyList)
                {
                    if (!observer)
                    {
                        observersToRemove.Add(observer);
                    }
                    else
                    {
                        observer.SendMessage(notification.name, notification, SendMessageOptions.DontRequireReceiver);
                    }
                }

                foreach (Component observer in observersToRemove)
                {
                    notifyList.Remove(observer);
                }
            }
        }
    }
}
