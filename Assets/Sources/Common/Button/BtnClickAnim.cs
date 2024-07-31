/****************************************************
    文件：BtnClickAnim.cs
	作者：Plusbe_hmr
    邮箱: 2698971533@qq.com
    日期：2022/11/15 11:2:44
	功能：Nothing
*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class BtnClickAnim : MonoBehaviour 
{
    private Button btn;
    private Vector3 _defaultPos;
    private Vector3 _defaultScale;
    private Vector3 _defaultEuler;

    private Tweener _Tweener;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnbtnClicked);
        _defaultPos = transform.position;
        _defaultScale = transform.localScale;
        _defaultEuler = transform.eulerAngles;
    }

    void OnbtnClicked()
    {
        if (_Tweener == null)
        {

            _Tweener = transform.DOPunchScale(new Vector3(-0.2f, -0.2f, 0), 0.2f, 4, 0.5f).OnComplete(() =>
            {
                
            });
            _Tweener.SetAutoKill(false);
        }
        else
        {
            _Tweener.Restart();
        }
    }
}
