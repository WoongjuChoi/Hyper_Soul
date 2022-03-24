using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SamplePlayerDamagedState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("SamplePlayerDamagedState OnEnter");
    }

    public void ExitState()
    {
        Debug.Log("SamplePlayerDamagedState OnExit");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerDamagedState OnInitialize");
    }

    public void UpdateState()
    {
        Debug.Log("SamplePlayerDamagedState OnUpdate");
    }
}
