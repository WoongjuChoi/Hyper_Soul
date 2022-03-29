using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnPositionState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.ReturnPosition;
    }

    public void ExitState()
    {
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
    }
}
