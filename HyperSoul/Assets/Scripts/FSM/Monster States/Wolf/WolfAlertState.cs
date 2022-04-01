using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAlertState : BaseState<WolfInformation>
{
    private bool _playAnimation = false;

    private float _changeIdleAnimationTime = 4f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Alert;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0;

        _playAnimation = false;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ALERT, false);
    }

    public override void UpdateState()
    {
        if (base.CreatureInfomation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (_playAnimation)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

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

            _playAnimation = true;
        }
    }
}
