using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerMoveState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void OnEnter()
    {
        Debug.Log("SamplePlayerMoveState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerMoveState OnExit");
    }

    public void OnInitialize(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerMoveState OnInitialize");
    }

    public void OnUpdate()
    {
        // ������
        if (Input.GetKeyDown(KeyCode.I))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Attack);
        }

        Debug.Log("SamplePlayerMoveState OnUpdate");
    }
}
