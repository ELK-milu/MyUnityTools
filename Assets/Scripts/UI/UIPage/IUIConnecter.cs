using System;

public interface IUIConnecter
{
	event Action OnOpenHandler;
	event Action OnCloseHandler;
	event Action OnUpdateHandler;
	void OnOpen();
	void OnClose();
	void OnUpdate();
}
