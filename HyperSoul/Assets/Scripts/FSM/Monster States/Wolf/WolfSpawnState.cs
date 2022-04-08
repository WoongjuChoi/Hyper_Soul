using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawnState : BaseState<WolfInformation>
{
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Spawn;

        base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_SPAWN);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= base.CreatureInformation.MonsterInvincibleTime)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }
    }
}
