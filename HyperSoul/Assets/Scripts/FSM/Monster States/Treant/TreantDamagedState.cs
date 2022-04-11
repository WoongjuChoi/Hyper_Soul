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
        // ������ �ް� (���� �ʿ�)
        CreatureInformation.MonsterCurrentHP -= 20;

        // HP <= 0 �̸� Die ����
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
