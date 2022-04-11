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
        // ������ �ް� (���� �ʿ�)
        //CreatureInformation.MonsterCurrentHP -= 20;
        CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.Target.GetComponent<LivingEntity>().Attack, Vector3.zero, Vector3.zero);

        // ���� ���
        //CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.AttackerInfo.Attack, Vector3.zero, Vector3.zero);

        // HP <= 0 �̸� Die ����
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
