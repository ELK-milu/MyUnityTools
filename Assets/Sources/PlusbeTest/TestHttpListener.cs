using Plusbe.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHttpListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        addHttp();
	}

    #region Http建立观察
    private void addHttp()
    {
        NotificationCenter.Instance.AddObserver(this, "TestHttpHello");
    }

    public void TestHttpHello(Notification notification)
    {
        Debug.Log("Received notification from " + notification.sender);
        if (notification.data == null)
        {
            Debug.Log("And the data object was null");
        }
        else
        {
            //获取到相关数据 进行处理
            Debug.Log("And it include a data object:" + notification.data);
        }
    }

    #endregion

}
