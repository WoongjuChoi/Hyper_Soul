using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChaseState : BaseState<WolfInformation>
{
    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Chase;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        CreatureInformation.MonsterChaser.IsActive = false;

        CreatureInformation.MonsterChaser.ResetPath();

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.Target.GetComponent<LivingEntity>().IsDead)
        {
            CreatureInformation.IsTargeting = false;

            FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (CreatureInformation.IsWithinAttackRange)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        float distance = (GameObject.transform.position - CreatureInformation.InitializePosition.position).magnitude;

        if (distance >= 20f)
        {
            FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        CreatureInformation.MonsterChaser.IsActive = true;
    }
}
