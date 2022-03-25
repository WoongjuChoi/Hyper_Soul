using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterAlertState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterAlertState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterAlertState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterAlertState UpdateState");
    }
}
