using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantAttackState : BaseState<TreantInformation>
{
    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (base.CreatureInformation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (false == base.CreatureInformation.ExistInSight)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }

        StartCoroutine(AttackMotion());
    }

    private IEnumerator AttackMotion()
    {
        while (base.CreatureInformation.ExistInSight)
        {
            base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_ATTACK);

            yield return new WaitForSeconds(5f);
        }
    }
}
