using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDoTween : MonoBehaviour {

    public Transform target;

    public Transform cubeA, cubeB;

	void Start () {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(target.DOMoveY(2, 1));
        mySequence.Join(target.DORotate(new Vector3(0, 135, 0), 1));
        mySequence.Append(target.DOScaleY(0.2f, 1));
        mySequence.Insert(0, target.DOMoveX(4, mySequence.Duration()).SetRelative());
        mySequence.SetLoops(4, LoopType.Yoyo);
        target.gameObject.GetComponent<Image>().DOColor(Color.red, 3f);

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        cubeA.DOMove(new Vector3(-2, 2, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo);
        DOTween.To(() => cubeB.position, x => cubeB.position = x, new Vector3(-2, 2, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveIn(Transform trans)
    {
        DOTween.Kill(this.transform);
        this.transform.localScale = Vector3.zero;
        this.transform.localPosition = Vector3.zero;
        this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
    }

    public void MoveOut(Transform trans,Action actionCom = null)
    {
        this.transform.DOLocalMoveX(-1920, 0.5f).OnComplete(() =>
        {
            if (actionCom != null) actionCom();

            //for (int i = 0; i < scrollContents.Count; i++)
            //{
            //    scrollContents[i].CloseMe();
            //    GameObject.Destroy(scrollContents[i].gameObject);
            //}

            //scrollMore.CloseMe();

            //scrollContents.Clear();
            //content.localPosition = new Vector3(0, content.localPosition.y, content.localPosition.z);
            //UIManager.CloseUIWindow(this);
        });
    }
}
