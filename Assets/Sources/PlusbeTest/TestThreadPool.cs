using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using UnityEngine;

public class TestThreadPool : MonoBehaviour {

    public delegate string MyDelegate(string msg);

    public Queue<string> queue;

    // Use this for initialization
    void Start()
    {
        queue = new Queue<string>();
        //testThread1();
        testThread2();
    }

    // Update is called once per frame
    void Update()
    {
        string result = dequeue();
        if (!string.IsNullOrEmpty(result))
        {
            Debug.Log(result);
        }
    }

    /// <summary>
    /// delegate
    /// </summary>
    private void testThread2()
    {
        enqueue("主线程：开始工作");

        MyDelegate myDelegate = new MyDelegate(getMsg);
        //IAsyncResult result = myDelegate.BeginInvoke("hello", new AsyncCallback(threadCom), "helloagain");

        myDelegate.BeginInvoke("hello", new AsyncCallback(threadCom), "helloagain");

        //调用EndInvoke(IAsyncResult)获取运行结果，一旦调用了EndInvoke，即使结果还没来得及返回，主线程也阻塞等待了
        //string data = myDelegate.EndInvoke(result);
        //enqueue(data);

        //比上个例子，只是利用多了一个IsCompleted属性，来判断异步线程是否完成
        //while (!result.IsCompleted)
        //{
        //    Thread.Sleep(500);
        //    enqueue("异步线程还没完成，主线程干其他事!");
        //}

        //WaitHandle[] waitHandleList = new WaitHandle[] { result.AsyncWaitHandle };
        ////是否全部异步线程完成
        //while (!WaitHandle.WaitAll(waitHandleList, 200))
        //{
        //    Console.WriteLine("异步线程未全部完成，主线程继续干其他事!");
        //}

        //string data = myDelegate.EndInvoke(result);
        //enqueue(data);

        enqueue("主线程：结束工作");
    }

    public string getMsg(string msg)
    {
        enqueue("子线程：开始工作");
        //try
        //{
        //    Thread.CurrentThread.Name = "nononoThread";
        //}
        //catch (Exception ex)
        //{
        ////Thread.Name can only be set once.
        //    enqueue(ex.ToString());
        //}
        enqueue("子线程：" + msg.ToString());
        Thread.Sleep(2000);
        enqueue("子线程：结束工作");
        return "ok-" + msg;
    }

    public void threadCom(IAsyncResult result)
    {
        AsyncResult _result = (AsyncResult)result;
        MyDelegate mydel = (MyDelegate)_result.AsyncDelegate;
        string data = mydel.EndInvoke(_result);
        enqueue(data);
        string data2 = _result.AsyncState.ToString();
        enqueue(data2);
        enqueue("子线程回调结束" + Thread.CurrentThread.Name);
    }


    #region 基础使用
    /// <summary>
    /// 基础使用
    /// </summary>
    private void testThread1()
    {
        enqueue("主线程：开始工作");

        //适用于无返回值
        ThreadPool.QueueUserWorkItem(new WaitCallback(runWork), "hello");

        enqueue("主线程：结束工作");
    }

    private void runWork(object msg)
    {
        enqueue("子线程：开始工作");
        enqueue("子线程：" + msg.ToString());
        Thread.Sleep(2000);
        enqueue("子线程：结束工作");
    }

    #endregion

    public string dequeue()
    {
        return queue.Count > 0 ? queue.Dequeue() : "";
    }

    public void enqueue(string msg)
    {
        queue.Enqueue(DateTime.Now.ToString("yyyyMMddHHmmss.fff") + ":" + msg);
    }
}
