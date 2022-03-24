using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfIdleState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfIdleState OnEnter");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfIdleState OnExit");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfIdleState OnInitialize");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfIdleState OnUpdate");
    }
}
