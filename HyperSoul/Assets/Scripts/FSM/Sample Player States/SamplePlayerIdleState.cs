using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerIdleState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    

    public void EnterState()
    {
        Debug.Log("SamplePlayerIdleState OnEnter");
    }

    public void ExitState()
    {
        Debug.Log("SamplePlayerIdleState OnExit");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerIdleState OnInitialize");
    }

    public void UpdateState()
    {
        // 디버깅용
        if (Input.GetKeyDown(KeyCode.P))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Move);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Attack);
        }

        Debug.Log("SamplePlayerIdleState OnUpdate");
    }
}