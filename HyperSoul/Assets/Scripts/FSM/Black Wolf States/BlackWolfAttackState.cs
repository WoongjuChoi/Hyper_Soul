using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfAttackState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfAttackState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfAttackState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfAttackState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfAttackState UpdateState");
    }
}
