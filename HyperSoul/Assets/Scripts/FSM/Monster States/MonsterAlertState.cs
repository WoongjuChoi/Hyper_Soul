using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _changeIdleAnimationTime = 4f;
    private float _elapsedTime = 0f;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Alert;

        _gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, true);
    }

    public void ExitState()
    {
        _elapsedTime = 0;
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

        if (-1 == _elapsedTime)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);

            _gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, false);

            return;
        }

        ChangeIdleAnimation();
    }

    private void ChangeIdleAnimation()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeIdleAnimationTime)
        {
            _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_IDLE);

            _elapsedTime = -1f;
        }
    }
}
