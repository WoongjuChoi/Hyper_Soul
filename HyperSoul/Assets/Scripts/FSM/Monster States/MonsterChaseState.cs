using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Chase;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_CHASE);
    }

    public void ExitState()
    {
        _monsterInfo.MonsterChaser.IsActive = false;

        _monsterInfo.MonsterChaser.ResetPath();
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        if (_monsterInfo.IsDamaged)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (_monsterInfo.IsWithinAttackRange)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        _monsterInfo.MonsterChaser.IsActive = true;
    }
}
