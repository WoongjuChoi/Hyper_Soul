using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterDieState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterDieState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterDieState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterDieState UpdateState");
    }
}
