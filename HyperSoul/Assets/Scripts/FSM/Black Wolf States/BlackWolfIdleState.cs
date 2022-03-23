using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfIdleState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void OnEnter()
    {
        Debug.Log("BlackWolfIdleState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("BlackWolfIdleState OnExit");
    }

    public void OnInitialize(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfIdleState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("BlackWolfIdleState OnUpdate");
    }
}
