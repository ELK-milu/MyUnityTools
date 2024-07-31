/*
*┌────────────────────────────────────────────────┐
*│　描    述：ScrollRectBase                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0   
*│  UnityVersion：#UNITYVERSION#                                            
*│　创建时间：#DATE#                        
*└────────────────────────────────────────────────┘
*/

using DG.Tweening;
using Plusbe.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollRectBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPointDown;

    public int itemWidth = 714;
    public Transform container;

    public GameObject goScrollPoint;
    public Transform containerPoint;

    //public Sprite spPointSelect;
    //public Sprite spPointUnSelect;

    private float lastDownX; //鼠标按下的位置
    private float lastUpdateX; //上一帧的位置

    private Vector3 lastDownContainerPosition;

    private int currLoc;
    private int totalLoc;

    private List<ButtonBase> btnPoints;

    public Vector3 centerPosition;

    public int pointSpaceW = 100; //半个的距离

    public bool autoPlay=false;

    private float autoPlayTime = 8;
    private float lastPlayTime;

    private Vector3 endPosition;
    public float delayTime = 0.3f;

    private bool isInit;

    private void Update()
    {
        if (!isInit || !gameObject.activeSelf) return;

        if (isPointDown)
        {
            lastUpdateX = Input.mousePosition.x;

            container.localPosition = lastDownContainerPosition + new Vector3(lastUpdateX-lastDownX, 0, 0);
        }

        if (Time.time - lastPlayTime>autoPlayTime)
        {
            lastPlayTime = Time.time;
            ShowNext();
        }
    }

    /// <summary>
    /// 状态初始化
    /// </summary>
    /// <param name="cLoc"></param>
    /// <param name="tLoc"></param>
    public void OnInit(int cLoc,int tLoc)
    {
        isInit = true;

        currLoc = cLoc;
        totalLoc = tLoc;

        btnPoints = new List<ButtonBase>();

        for (int i = 0; i < totalLoc; i++)
        {
            GameObject point = Instantiate(goScrollPoint, containerPoint);
            point.transform.localPosition = centerPosition + new Vector3( - pointSpaceW * (totalLoc-1)+pointSpaceW*i*2, 0, 0);
            point.GetComponent<ButtonBase>().Init(i,OnClickPoint);
            btnPoints.Add(point.GetComponent<ButtonBase>());
        }

        endPosition = this.transform.localPosition;
    }

    private void OnClickPoint(ButtonBase obj)
    {
        ShowContent(obj.index, true);
        UpdateHandleTime();
    }

    private void UpdateHandleTime()
    {
        lastPlayTime = Time.time + 10; //比默认时间多10秒
        autoPlay = false;
    }

    public void OnOpen(int toLoc = 0)
    {

        AnimationHelper.DoMoveIn(this.transform, 0, -1200, 0, endPosition, 0.3f, delayTime);

        ShowContent(toLoc);

        lastPlayTime = Time.time;
        autoPlay = true;
    }

    public void OnClose()
    {

    }

    private void ShowNext()
    {
        currLoc++;
        if (currLoc > totalLoc - 1) currLoc = 0;
        ShowContent(currLoc, true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ScrollRectBase OnPointerDown");

        
        UpdateHandleTime();

        isPointDown = true;
        lastDownX = lastUpdateX = Input.mousePosition.x;

        lastDownContainerPosition = container.localPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("ScrollRectBase OnPointerUp");

        isPointDown = false;

        int startLoc = currLoc;

        int moveType = 0; //0不动  1往右 -1往左
        if (lastUpdateX - lastDownX > 10)
        {
            moveType = -1;
        }
        else if (lastUpdateX - lastDownX < -10)
        {
            moveType = 1;
        }

        float itemNum = -container.localPosition.x / itemWidth;
        int itemIndex = Mathf.RoundToInt(itemNum); // 准备展示的页数

        if (moveType == 1)
        {
            if (itemIndex == currLoc)
                currLoc++;
            else currLoc = itemIndex;
        }
        else if (moveType == -1)
        {
            if (itemIndex == currLoc)
                currLoc--;
            else currLoc = itemIndex;
        }
        else
        {
            currLoc = itemIndex;
        }

        if (currLoc <= 0) currLoc = 0;
        if (currLoc >= totalLoc - 1) currLoc = totalLoc - 1;

        ShowContent(currLoc, true);

        Debug.Log(container.localPosition + ">>currrLoc:" + currLoc + ">>startLoc:" + startLoc + ">>itemIndex:" + itemIndex + ">>moveType:" + moveType + ">>offX:" + (lastDownX - lastUpdateX) + ">>lastDownX:" + lastDownX + ">>lastUpdateX:" + lastUpdateX);
    }

    private void ShowContent(int cIndex, bool doAniamtion = false)
    {

        currLoc = cIndex;

        if (doAniamtion)
        {
            container.DOLocalMoveX(-cIndex * itemWidth, 0.3f);
        }
        else
        {
            container.localPosition = new Vector3(-itemWidth * cIndex, 0, 0);
        }

        //for (int i = 0; i < littlePoints.Length; i++)
        //{
        //    if (littlePoints[i].index == cIndex)
        //    {
        //        littlePoints[i].ShowSelect();
        //    }
        //    else
        //    {
        //        littlePoints[i].ShowUnSelect();
        //    }
        //}

        UpdateBtnPointStatus();
    }

    /// <summary>
    /// 更新点击按钮状态
    /// </summary>
    private void UpdateBtnPointStatus()
    {
        for (int i = 0; i < btnPoints.Count; i++)
        {
            if (currLoc == i)
            {
                btnPoints[i].ShowSelect();
            }
            else
            {
                btnPoints[i].ShowUnselect();
            }
        }
    }
}
