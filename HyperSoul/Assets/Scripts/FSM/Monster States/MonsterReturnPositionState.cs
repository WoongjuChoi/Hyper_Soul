using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnPositionState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterReturnPositionState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterReturnPositionState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterReturnPositionState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterReturnPositionState UpdateState");
    }
}
