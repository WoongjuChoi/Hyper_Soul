using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDamagedState : BaseState<TreantInformation>
{
    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Damaged;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, true);
    }

    public override void ExitState()
    {
        CreatureInformation.IsDamaged = false;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, false);
    }

    public override void UpdateState()
    {
        // 데미지 받고 (수정 필요)
        CreatureInformation.MonsterCurrentHP -= 20;

        // HP <= 0 이면 Die 상태
        if (CreatureInformation.MonsterCurrentHP <= 0)
        {
            GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DIE);

            FiniteStateMachine.ChangeState(EStateIDs.Die);

            return;
        }

        if (CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }
        else
        {
            FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }
    }
}
