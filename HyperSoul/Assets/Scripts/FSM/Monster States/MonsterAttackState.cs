using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("MonsterAttackState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterAttackState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("MonsterAttackState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("MonsterAttackState UpdateState");
    }
}
