using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDamagedState : BaseState<TreantInformation>
{
    private float _viewAngle = 60f;

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

        Vector3 monsterSightPosition = new Vector3(base.CreatureInformation.MonsterRayPoint.position.x, base.CreatureInformation.Target.transform.position.y, base.CreatureInformation.MonsterRayPoint.position.z);

        Vector3 vecMonsterToPlayer = (base.CreatureInformation.Target.transform.position - monsterSightPosition).normalized;

        float dotPlayerToMonster = Vector3.Dot(base.GameObject.transform.forward, vecMonsterToPlayer);

        float temp = Mathf.Cos(_viewAngle * Mathf.Deg2Rad);

        if (dotPlayerToMonster < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }
        else
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }
    }
}
