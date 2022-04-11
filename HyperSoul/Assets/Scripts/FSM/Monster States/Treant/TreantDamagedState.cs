using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDamagedState : BaseState<TreantInformation>
{
    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Damaged;
    }

    public override void ExitState()
    {
        CreatureInformation.IsDamaged = false;
    }

    public override void UpdateState()
    {
        // 데미지 받고 (수정 필요)
        //CreatureInformation.MonsterCurrentHP -= 20;
        CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.Target.GetComponent<LivingEntity>().Attack, Vector3.zero, Vector3.zero);

        // 최종 결과
        //CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.AttackerInfo.Attack, Vector3.zero, Vector3.zero);

        // HP <= 0 이면 Die 상태
        if (CreatureInformation.CurHp <= 0)
        {
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
