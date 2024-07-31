using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class UIPage : MonoBehaviour
{
	public event Action OnOpenHandler;
	public event Action OnCloseHandler;
	public event Action OnUpdateHandler;
	public Coroutine OnOpenAsyncCoroutine;
	public Coroutine OnCloseAsyncCoroutine;
	private void Awake()
	{
		Initiate();
	}

	public UIPage(Action openMethod = null, Action closeMethod = null, Action updateMethod = null)
	{
		OnOpenHandler = openMethod;
		OnCloseHandler = closeMethod;
		OnUpdateHandler = updateMethod;
	}

	protected virtual void Initiate()
	{
		OnOpenHandler = () => { DebugUIManager.Instance.SetNowPage(this); };
	}

	public virtual IEnumerator OnOpen()
	{
		ResetState();
		OnOpenHandler?.Invoke();
		yield return null;
		if (OnOpenAsyncCoroutine != null)
		{
			yield return OnOpenAsyncCoroutine;
		}
		DebugUIManager.Instance.OnUpdateHandler += OnUpdate;
		OnOpenAsyncCoroutine = null;
	}

	public virtual void ResetState()
	{
		transform.localPosition = Vector3.zero;
	}

	public virtual IEnumerator OnClose()
	{
		OnCloseHandler?.Invoke();
		yield return null;
		if (OnCloseAsyncCoroutine != null)
		{
			yield return OnCloseAsyncCoroutine;
		}
		DebugUIManager.Instance.OnUpdateHandler -= OnUpdate;
		OnCloseAsyncCoroutine = null;
	}
	public virtual void OnUpdate()
	{
		OnUpdateHandler?.Invoke();
	}

	public virtual void OnFixedUpdate()
	{
		
	}
}