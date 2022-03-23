using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerAttackState : IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void OnEnter()
    {
        Debug.Log("SamplePlayerAttackState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerAttackState OnExit");
    }

    public void OnInitialize(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("SamplePlayerAttackState OnInitialize");
    }

    public void OnUpdate()
    {
        // µð¹ö±ë¿ë
        if (Input.GetKeyDown(KeyCode.I))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _finiteStateMachine.ChangeState(EStateIDs.Move);
        }

        Debug.Log("SamplePlayerAttackState OnUpdate");
    }
}
