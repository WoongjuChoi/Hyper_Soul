using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDamagedState : BaseState<TreantInformation>
{
    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Damaged;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, true);
    }

    public override void ExitState()
    {
        base.CreatureInformation.IsDamaged = false;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, false);
    }

    public override void UpdateState()
    {
        // 데미지 받고 (수정 필요)
        base.CreatureInformation.MonsterCurrentHP -= 20;

        // HP <= 0 이면 Die 상태
        if (base.CreatureInformation.MonsterCurrentHP <= 0)
        {
            base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DIE, true);

            base.FiniteStateMachine.ChangeState(EStateIDs.Die);

            return;
        }

        if (base.CreatureInformation.ExistInSight)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }
        else
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }
    }
}
