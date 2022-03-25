using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfChaseState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfChaseState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfChaseState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfChaseState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfChaseState UpdateState");
    }
}
