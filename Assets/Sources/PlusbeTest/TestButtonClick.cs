using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestButtonClick : MonoBehaviour {

    public List<GameObject> gos;

    public delegate void EventHandler();

    public Text txt;

    private List<Action> _actions = new List<Action>();
    private List<Action> _currentActions = new List<Action>();

	// Use this for initialization
	void Start () {
        addOnClickListener("button_test", helloWorld);


        addOnClickListener("button_test2", helloWorld2);
	}
	
	// Update is called once per frame
	void Update () {
        lock (_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }

        foreach (var item in _currentActions)
        {
            item();
        }
	}

    public T GetObj<T>(string name) where T : Button
    {
        for (int i = 0; i < gos.Count; i++)
        {
            if (gos[i].name == name) return gos[i].GetComponent<T>();
        }

        return null;
    }

    private void addOnClickListener(string buttonName,EventHandler callBack)
    {
        Button button = GetObj<Button>(buttonName);

        addOnClickListener(button, callBack);
    }

    private void addOnClickListener(Button button, EventHandler callBack)
    {
        Debug.Log("add onClick");

        //UnityAction m_onClick = () =>
        //{
        //    callBack();
        //};

        UnityAction m_onClick = (() =>
        {
            callBack();
        });

        button.onClick.AddListener(m_onClick);

    }

    private void helloWorld()
    {
        Debug.Log("hello world my click ");
    }

    private void helloWorld2()
    {
        Debug.Log("hello world 2 my click ");

        new Thread(threadHelloWorld).Start();
    }

    private void threadHelloWorld()
    {
        Debug.Log("threadHelloWorld");

        txt.text = "clickThreadFun:" + DateTime.Now.ToString("yyyyMMhhDDmmss");
    }

    public void clickThread()
    {
        Thread thread = new Thread(clickThreadFun);
        thread.Start();
    }

    private void clickThreadFun()
    {
        Debug.Log("clickThreadFun");

        _actions.Add(() =>
        {
            formThread();
        });

        //_actions.Add((() => {
        //    txt.text = "clickThreadFun:" + DateTime.Now.ToString("yyyyMMhhDDmmss");
        //    Debug.Log(txt.text);
        //}));

        //txt.text = "clickThreadFun:" + DateTime.Now.ToString("yyyyMMhhDDmmss");
    }

    private void formThread()
    {
        txt.text = "clickThreadFun:" + DateTime.Now.ToString("yyyyMMhhDDmmss");
        Debug.Log(txt.text);
    }
}
