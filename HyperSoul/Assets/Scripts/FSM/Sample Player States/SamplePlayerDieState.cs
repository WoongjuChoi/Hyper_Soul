using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerDieState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void OnEnter()
    {
        Debug.Log("SamplePlayerDieState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerDieState OnExit");
    }

    public void OnInitialize(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerDieState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerDieState OnUpdate");
    }
}
