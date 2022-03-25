using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfDieState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfDieState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfDieState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfDieState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfDieState UpdateState");
    }
}
