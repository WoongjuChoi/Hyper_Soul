using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SamplePlayerDamagedState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void OnEnter()
    {
        Debug.Log("SamplePlayerDamagedState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerDamagedState OnExit");
    }

    public void OnInitialize(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerDamagedState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerDamagedState OnUpdate");
    }
}
