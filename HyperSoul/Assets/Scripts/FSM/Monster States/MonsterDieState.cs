using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Die;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DIE);
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
