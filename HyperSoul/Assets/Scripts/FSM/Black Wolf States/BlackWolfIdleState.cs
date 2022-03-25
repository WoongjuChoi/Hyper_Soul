using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfIdleState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfIdleState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfIdleState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfIdleState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfIdleState UpdateState");
    }
}
