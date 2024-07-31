using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugUIManager : MonoBehaviour
{
    private static DebugUIManager _instance;
    public static DebugUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DebugUIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("DebugUIManager");
                    _instance = go.AddComponent<DebugUIManager>();
                }
            }
            return _instance;
        }
    }
    [SerializeField]
    private UIPage _debugUIPage;
    private UIPage _nowPage;
    public List<UIPage> UIPageList;
    public event Action OnStartHandler;
    public event Action OnUpdateHandler;
    public event Action OnFixedUpdateHandler;
    private int _nowIndex;

    #region 生命周期管理
    private void Awake()
    {
        if (_nowPage == null)
        {
            _nowPage = UIPageList[0];
            _nowIndex = 0;
        }
        _debugUIPage.OnOpen();
    }

    void Start()
    {
        OnStartHandler?.Invoke();
    }

    private void Update()
    {
        OnUpdateHandler?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdateHandler?.Invoke();
    }
    #endregion

    #region 页面管理方法
    private bool IsUnLegal()
    {
        bool isUnLegal = UIPageList == null || UIPageList.Count <= 1 || !UIPageList.Contains(_nowPage);
        if (isUnLegal)
        {
            Debug.LogError("UIPageList is null or empty");
        }
        return isUnLegal;
    }
    public void OnPage(UIPage page)
    {
        StartCoroutine(PageChange(page));
    }

    IEnumerator PageChange(UIPage page)
    {
        if (_nowPage != null)
        {
            yield return StartCoroutine(_nowPage.OnClose());
            if (_nowIndex == UIPageList.Count - 1)
            {
                _nowIndex = 0;
            }
            else if (_nowIndex == 0)
            {
                _nowIndex = UIPageList.Count - 1;
            }
        }
        _nowPage = UIPageList[_nowIndex];
        if (!_nowPage.isActiveAndEnabled)
        {
            _nowPage.gameObject.SetActive(true);
        }
        yield return StartCoroutine(_nowPage.OnOpen());
    }
    
    
    public void ResetPageList()
    {
        UIPageList.Clear();
    }
    public void OnNext()
    {
        if(IsUnLegal()) return;
        _nowIndex = UIPageList.IndexOf(_nowPage);

        OnPage(UIPageList[_nowIndex]);
    }
    public void OnPrev()
    {
        if(IsUnLegal()) return;
        _nowIndex = UIPageList.IndexOf(_nowPage);

        OnPage(UIPageList[_nowIndex]);
    }
    public void SetNowPage(UIPage page)
    {
        _nowPage = page;
    }
    public void AddPage (UIPage page)
    {
        UIPageList.Add(page);
    }
    #endregion

}
