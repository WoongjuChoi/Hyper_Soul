using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSpawnState : BaseState<TreantInformation>
{
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Spawn;

        GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_SPAWN);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= CreatureInformation.MonsterInvincibleTime)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }
    }
}
