using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDamagedState : BaseState<TreantInformation>
{
    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Damaged;
    }

    public override void ExitState()
    {
        CreatureInformation.IsDamaged = false;
    }

    public override void UpdateState()
    {
        CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.Attacker.GetComponent<LivingEntity>().Attack, Vector3.zero, Vector3.zero);

        if (CreatureInformation.CurHp <= 0)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Die);

            return;
        }

        if (CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Attack);

            return;
        }
        else
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.RotatePosition);

            return;
        }
    }
}
