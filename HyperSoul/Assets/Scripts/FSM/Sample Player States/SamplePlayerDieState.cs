using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerDieState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("SamplePlayerDieState OnEnter");
    }

    public void ExitState()
    {
        Debug.Log("SamplePlayerDieState OnExit");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerDieState OnInitialize");
    }

    public void UpdateState()
    {
        Debug.Log("SamplePlayerDieState OnUpdate");
    }
}
