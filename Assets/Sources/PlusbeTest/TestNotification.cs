using Plusbe.Message;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PlusbeTest
{
    public class TestNotification : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            NotificationCenter.Instance.AddObserver(this, "HelloWorld");
        }

        public void ToClick()
        {
            NotificationCenter.Instance.PostNotification(this, "HelloWorld", "test about notification");
        }

        public void ToThreadClick()
        {
            new Thread(ThreadClick).Start();
        }

        private void ThreadClick()
        {
            NotificationCenter.Instance.PostNotification(this, "HelloWorld", "test about notification from thread");
        }

        public void HelloWorld(Notification notification)
        {
            Debug.Log("Received notification from " + notification.sender);
            if (notification.data == null)
            {
                Debug.Log("And the data object was null");
            }
            else
            {
                Debug.Log("And it include a data object:" + notification.data);
            }
        }
    }
}
