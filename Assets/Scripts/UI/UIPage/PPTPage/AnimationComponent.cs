using DG.Tweening;
using InspectorEx;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public abstract class BaseAnimComponent
{
	public int LoopTime;
	public float Duration;
	public float Delay;
	public Ease Ease;
	public LoopType LoopType;
	public bool IsFrom;
	public abstract Tweener DoAnim();
	public abstract void ResetState();
}
[Serializable]
public class FadeAnimComponent : BaseAnimComponent
{
	public FadeType FadeType;
	[PropertyActive("FadeType",CompareType.Contains,FadeType.RawImage)]
	public RawImage RawImage;
	[PropertyActive("FadeType",CompareType.Contains,FadeType.Image)]
	public Image Image;
	[PropertyActive("FadeType",CompareType.Contains,FadeType.CanvasGroup)]
	public CanvasGroup CanvasGroup;
	[PropertyActive("FadeType",CompareType.Contains,FadeType.TextMeshProUGUI)]
	public TextMeshProUGUI TextMeshProUGUI;
	[PropertyActive("FadeType",CompareType.Contains,FadeType.Text)]
	public Text Text;
	public float Alpha;

	public override Tweener DoAnim()
	{
		Tweener tween = null;
		switch (FadeType)
		{
			case  FadeType.RawImage:
				tween = RawImage.DOFade(Alpha, Duration);
				break;
			case  FadeType.Image:
				tween = Image.DOFade(Alpha, Duration);
				break;
			case  FadeType.CanvasGroup:
				tween = CanvasGroup.DOFade(Alpha, Duration);
				break;
			case  FadeType.TextMeshProUGUI:
				tween = TextMeshProUGUI.DOFade(Alpha, Duration);
				break;
		}
		tween.SetEase(Ease).SetLoops(LoopTime, LoopType).SetDelay(Delay);
		if (IsFrom)
		{
			tween.From();
		}
		return tween;
	}
	public override void ResetState()
	{
		switch (FadeType)
		{
			case FadeType.RawImage:
				RawImage.color = new Color(RawImage.color.r,RawImage.color.g, RawImage.color.b, 1f);
				break;
			case FadeType.Image:
				Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1f);
				break;
			case FadeType.CanvasGroup:
				CanvasGroup.alpha = 1f;
				break;
			case FadeType.TextMeshProUGUI:
				TextMeshProUGUI.color = new Color(TextMeshProUGUI.color.r, TextMeshProUGUI.color.g, TextMeshProUGUI.color.b, 1f);
				break;
			case FadeType.Text:
				Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1f);
				break;
		}
	}
	//TODO : Blend类型返回多Tweener或Sequence
}

[Serializable]
public class MoveAnimComponent : BaseAnimComponent
{
	public bool IsLocal;
	public Transform Transform;
	public Vector3 Destination;
	public override Tweener DoAnim()
	{
		Tweener tween = null;
		if (IsLocal)
		{
			tween = Transform.DOMove(Destination, Duration);
		}
		else
		{
			tween = Transform.DOLocalMove(Destination, Duration);
		}
		tween.SetEase(Ease).SetLoops(LoopTime, LoopType).SetDelay(Delay);
		if (IsFrom)
		{
			tween.From();
		}
		return tween;
	}

	public override void ResetState()
	{
		Transform.rotation = new Quaternion(0, 0, 0, 0);
	}
}

[Serializable]
public class ScaleAnimComponent : BaseAnimComponent
{
	public Transform Transform;
	public Vector3 Sclaer;
	public override Tweener DoAnim()
	{
		Tweener tween = Transform.DOScale(Sclaer, Duration).SetEase(Ease).SetLoops(LoopTime, LoopType).SetDelay(Delay);
		if (IsFrom)
		{
			tween.From();
		}
		return tween;
	}

	public override void ResetState()
	{
		Transform.localScale = Vector3.one;
	}
}
[Serializable]
public class RotationAnimComponent : BaseAnimComponent
{
	public bool IsLocal;
	public Transform Transform;
	public Vector3 Rotation;
	public override Tweener DoAnim()
	{
		Tweener tween = null;
		if (IsLocal)
		{
			tween = Transform.DORotate(Rotation, Duration);
		}
		else
		{
			tween = Transform.DOLocalRotate(Rotation, Duration);
		}
		tween.SetEase(Ease).SetLoops(LoopTime, LoopType).SetDelay(Delay);
		if (IsFrom)
		{
			tween.From();
		}
		return tween;
	}
	public override void ResetState()
	{
		Transform.localPosition = Vector3.zero;
	}
}
[Serializable]
public class AnimationComponentManager
{
	public AnimationType AnimationType;
	[Tooltip("播放前是否重置为初始状态")]
	public bool IsReset;
	[NonSerialized]
	public BaseAnimComponent CurrentAnimComponent;
	[PropertyActive("AnimationType",CompareType.Equal,AnimationType.Move)]
	public MoveAnimComponent MoveAnimComponent;
	
	[PropertyActive("AnimationType",CompareType.Equal,AnimationType.Fade)]
	public FadeAnimComponent FadeAnimComponent;
	
	[PropertyActive("AnimationType",CompareType.Equal,AnimationType.Scale)]
	public ScaleAnimComponent ScaleAnimComponent;
	
	[PropertyActive("AnimationType",CompareType.Equal,AnimationType.Rotate)]
	public RotationAnimComponent RotationAnimComponent;
	public UnityEvent OnStart;
	public UnityEvent OnComplete;

	public Tweener DoAnim()
	{
		var tweener = CurrentAnimComponent.DoAnim();
		tweener.OnStart(() => { OnStart.Invoke();});
		tweener.OnComplete(() => { OnComplete.Invoke();});
		tweener.Restart();
		return tweener;
	}

	public void SetCurrentAnimComponent()
	{
		switch (AnimationType)
		{
			case   AnimationType.Move:
				CurrentAnimComponent = MoveAnimComponent;
				break;
			case   AnimationType.Fade:
				CurrentAnimComponent = FadeAnimComponent;
				break;
			case   AnimationType.Scale:
				CurrentAnimComponent = ScaleAnimComponent;
				break;
			case   AnimationType.Rotate:
				CurrentAnimComponent = RotationAnimComponent;
				break;
			default:
				break;
		}
	}
	
	public void ResetState()
	{
		if (!IsReset)
		{
			return;
		}
		CurrentAnimComponent.ResetState();
	}

}
