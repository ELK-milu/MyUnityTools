/****************************************************
    文件：ABFrame.cs
	作者：Plusbe_hmr
    邮箱: 2698971533@qq.com
    日期：2022/11/25 11:37:16
	功能：Nothing
*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(FrameAnimator))]
public class ABFrame : MonoBehaviour 
{
    public string abName;
    public Texture2D zeroSprite;

    private FrameAnimator frameAnimator;

    private void Awake()
    {
        frameAnimator = GetComponent<FrameAnimator>();
    }

    private void OnEnable()
    {
        OnPlayFrame(abName);
    }

    private void OnPlayFrame(string abName)
    {
        frameAnimator.Frames = ResourceABManager.GetTexture2Ds(abName);
        frameAnimator.Restart();
    }

    private void OnDisable()
    {
        GetComponent<RawImage>().texture = zeroSprite;
    }
}
