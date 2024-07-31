using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ButtonBase : MonoBehaviour
{
    public GameObject goSelect;
    public GameObject goUnselect;

    //public GameObject goUnselect222;

    private bool isSelect;
    private Action<ButtonBase> userClick;
    public int index;

    //protected override void Start()
    //{
    //    this.onClick.AddListener(OnClick);
    //    base.Start();
    //}

    public virtual void Init(int index, Action<ButtonBase> userClick)
    {
        this.index = index;
        this.userClick = userClick;
        isSelect = false;
    }

    public virtual void ShowSelect()
    {
        if (!isSelect)
        {
            isSelect = true;
            if (goSelect != null) goSelect.SetActive(isSelect);
            if (goUnselect != null) goUnselect.SetActive(!isSelect);
        }
    }

    public virtual void ShowUnselect(bool must = false)
    {
        if (isSelect || must)
        {
            isSelect = false;
            if (goSelect != null) goSelect.SetActive(isSelect);
            if (goUnselect != null) goUnselect.SetActive(!isSelect);
        }
    }

    public virtual void OnClick()
    {
        userClick?.Invoke(this);
    }
}