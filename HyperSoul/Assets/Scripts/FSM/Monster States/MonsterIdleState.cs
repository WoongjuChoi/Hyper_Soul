using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterIdleState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterIdleState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterIdleState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterIdleState UpdateState");
    }
}
