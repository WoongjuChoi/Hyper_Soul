using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _elapsedTime = 0f;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Spawn;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_SPAWN);
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
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _monsterInfo.MonsterInvincibleTime)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);
        }
    }
}
