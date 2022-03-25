using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfSpawnState :IfiniteState
{
    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        Debug.Log("BlackWolfSpawnState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("BlackWolfSpawnState ExitState");
    }

    public void InitializeState(FiniteStateMachine fsm)
    {
        _finiteStateMachine = fsm;

        Debug.Log("BlackWolfSpawnState InitializeState");
    }

    public void UpdateState()
    {
        Debug.Log("BlackWolfSpawnState UpdateState");
    }
}
