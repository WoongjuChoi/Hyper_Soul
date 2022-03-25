using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfAlertState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfAlertState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfAlertState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfAlertState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfAlertState UpdateState");
    }
}
