using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : BaseState<MonsterInfomations>
{
    public override void EnterState()
    {
        base.CreatureInfomation = base.GameObject.GetComponent<MonsterInfomations>();

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Chase;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        base.CreatureInfomation.MonsterChaser.IsActive = false;

        base.CreatureInfomation.MonsterChaser.ResetPath();

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        if (base.CreatureInfomation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (base.CreatureInfomation.IsWithinAttackRange)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        float distance = (base.GameObject.transform.position - base.CreatureInfomation.InitializePosition.position).magnitude;

        if (distance >= 10f)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        base.CreatureInfomation.MonsterChaser.IsActive = true;
    }
}
