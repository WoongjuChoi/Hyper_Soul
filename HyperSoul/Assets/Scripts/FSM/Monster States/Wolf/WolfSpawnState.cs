using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawnState : BaseState<WolfInformation>
{
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Spawn;

        StartCoroutine(SpawnAnimator());
    }
    private IEnumerator SpawnAnimator()
    {
        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_SPAWN, true);

        yield return new WaitForSeconds(0.1f);

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_SPAWN, false);
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
