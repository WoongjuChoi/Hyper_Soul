using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterChaseState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterChaseState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterChaseState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterChaseState UpdateState");
    }
}
