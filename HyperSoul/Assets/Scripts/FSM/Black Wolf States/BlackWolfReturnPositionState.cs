using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfReturnPositionState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfReturnPositionState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfReturnPositionState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfReturnPositionState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfReturnPositionState UpdateState");
    }
}
