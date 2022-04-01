using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : BaseState<MonsterInfomations>
{
    private float _changeIdleAnimationTime = 4f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInfomation = base.GameObject.GetComponent<MonsterInfomations>();

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Alert;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0;
    }

    public override void UpdateState()
    {
        if (base.CreatureInfomation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (-1 == _elapsedTime)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, false);

            return;
        }

        ChangeIdleAnimation();
    }

    private void ChangeIdleAnimation()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeIdleAnimationTime)
        {
            base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_IDLE);

            _elapsedTime = -1f;
        }
    }
}
