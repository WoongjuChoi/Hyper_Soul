using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChaseState : BaseState<WolfInformation>
{
    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Chase;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        base.CreatureInformation.MonsterChaser.IsActive = false;

        base.CreatureInformation.MonsterChaser.ResetPath();

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        if (base.CreatureInformation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (base.CreatureInformation.IsWithinAttackRange)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        float distance = (base.GameObject.transform.position - base.CreatureInformation.InitializePosition.position).magnitude;

        if (distance >= 20f)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        base.CreatureInformation.MonsterChaser.IsActive = true;
    }
}
