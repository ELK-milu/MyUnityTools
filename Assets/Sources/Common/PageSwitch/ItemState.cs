using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemState : ASState
{
    [Space]
    public StateType stateName;
    [Space]
    public OnEnterEvent onEnterEvent;
    public OnExitEvent onExitEvent;

    public override void Enter(ASState from)
    {
        this.gameObject.SetActive(true);
        onEnterEvent?.Invoke();
    }

    public override void Exit(ASState to)
    {
        this.gameObject.SetActive(false);
        onExitEvent?.Invoke();
    }

    public override string GetName()
    {
        return stateName.ToString();
    }

    
    public void GoToNewState(StateType stateName)
    {
        stateManager.SwitchState(stateName);
    }

    public void GoToNewState(string stateName)
    {
        stateManager.SwitchState(stateName);
    }
}

[System.Serializable]
public class OnEnterEvent : UnityEvent { }

[System.Serializable]
public class OnExitEvent : UnityEvent { }