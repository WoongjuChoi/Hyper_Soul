using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAlertState : BaseState<WolfInformation>
{
    private float _changeIdleAnimationTime = 4f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Alert;
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_ALERT, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0;
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_ALERT, false);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeIdleAnimationTime)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Idle);

            return;
        }
    }
}
