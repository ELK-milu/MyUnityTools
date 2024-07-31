using Mono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour {


    public Image showView;
    public Sprite[] sprites = new Sprite[1];
    public string abFileName;

    public int midIndex = 0; //进场和循环切开序列索引

    private float lastUpdateTime;
    public int currActionIndex;

    private float frameTime = 0.04f;

    private bool isStopOnLast = false;

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gameObject.activeSelf)
        {
            if (sprites.Length <= 1)
                // sprites = ResourceABManager.GetSprites(abFileName);

            if (Time.time - lastUpdateTime > frameTime && sprites.Length>0)
            {
                lastUpdateTime = Time.time;
                int toFrame = ++currActionIndex % sprites.Length;
                if (isStopOnLast && currActionIndex >= sprites.Length)
                {
                    return;
                }

                if (midIndex != 0 && currActionIndex >= sprites.Length)
                {
                    toFrame = currActionIndex = midIndex;
                }

                showView.overrideSprite = sprites[toFrame];
            }
        }
    }

    public void InitFrame(float time)
    {
        frameTime = time;
    }

    public void InitShowLast(bool isLast)
    {
        isStopOnLast = isLast;
    }

    public void ShowLast()
    {
        if (sprites.Length > 0)
        {
            currActionIndex = sprites.Length - 1;
            showView.overrideSprite = sprites[currActionIndex];
        }
    }

    public void ResetStart()
    {
        currActionIndex = 0;
        //Debug.Log("restart:"+ currActionIndex);
        if(sprites.Length>0) showView.overrideSprite = sprites[0];
    }
}
