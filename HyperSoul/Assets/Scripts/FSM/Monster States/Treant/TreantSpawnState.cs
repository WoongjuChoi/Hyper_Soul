using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSpawnState : BaseState<TreantInformation>
{
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Spawn;
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= base.CreatureInfomation.MonsterInvincibleTime)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }
    }
}
