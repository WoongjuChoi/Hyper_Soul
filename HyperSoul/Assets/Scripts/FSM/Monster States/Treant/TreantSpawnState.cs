using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSpawnState : BaseState<TreantInformation>
{
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Spawn;
        StartCoroutine(SpawnAnimator());
    }

    private IEnumerator SpawnAnimator()
    {
        MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_SPAWN, true);
        
        yield return new WaitForSeconds(0.1f);

        MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_SPAWN, false);
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
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Idle);

            return;
        }
    }
}
