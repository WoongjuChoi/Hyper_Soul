using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
    }

    public void ExitState()
    {
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
    }
}
