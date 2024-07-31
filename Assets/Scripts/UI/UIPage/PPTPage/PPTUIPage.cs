using DG.Tweening;
using InspectorEx;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class PPTUIPage : UIPage
{
	public bool OnOpenAnimation; 
	public AnimationComponentManager OpenAnimComponentManager;
	public bool OnCloseAnimation;
	public AnimationComponentManager CloseAnimComponentManager;
	public RawImage ThisImage;
	protected Tweener CurrentOpenAnimTweener;
	protected Tweener CurrentCloseAnimTweener;
	public PPTUIPage(Action openMethod = null, Action closeMethod = null, Action updateMethod = null) : base(openMethod, closeMethod, updateMethod)
	{
		
	}

	private void OnValidate()
	{
		OpenAnimComponentManager.SetCurrentAnimComponent();
		CloseAnimComponentManager.SetCurrentAnimComponent();
	}
	

	override protected void Initiate()
	{
		base.Initiate();
		OnOpenHandler += OnOpenAnim;
		OnCloseHandler += OnCloseAnim;
		
	}
	
	
	// TODO:完善PPT的进场和出场动画设置，实现对DebugUIManager在播放动画完成后再切换的功能
	public virtual void OnOpenAnim()
	{
		if(!OnOpenAnimation) return;
		OnOpenAsyncCoroutine = StartCoroutine(OnOpenTween());
	}

	public virtual IEnumerator OnOpenTween()
	{
		CurrentOpenAnimTweener = OpenAnimComponentManager.DoAnim();
		yield return CurrentOpenAnimTweener.WaitForCompletion();

		CurrentOpenAnimTweener = null;
	}
	public virtual void OnCloseAnim()
	{
		if(!OnCloseAnimation) return;
		OnCloseAsyncCoroutine = StartCoroutine(OnCloseTween());
	}
	
	public virtual IEnumerator OnCloseTween()
	{
		CurrentCloseAnimTweener = CloseAnimComponentManager.DoAnim();	
		yield return CurrentCloseAnimTweener.WaitForCompletion();

		CurrentCloseAnimTweener = null;
		gameObject.SetActive(false);
	}


	public override void ResetState()
	{
		base.ResetState();
		OpenAnimComponentManager.ResetState();
		CloseAnimComponentManager.ResetState();
	}
}
