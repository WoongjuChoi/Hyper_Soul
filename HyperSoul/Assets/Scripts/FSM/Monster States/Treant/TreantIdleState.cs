using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantIdleState : BaseState<TreantInformation>
{
    private const float DOT_120_DEGREE = -0.5f;

    private int _increaseHealing = 0;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Idle;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;
    }

    public override void UpdateState()
    {
        if (base.CreatureInformation.IsDamaged)
        {
            Vector3 collisionVec = new Vector3(base.CreatureInformation.CollisionVec.x, base.GameObject.transform.position.y, base.CreatureInformation.CollisionVec.z);

            collisionVec = (collisionVec - base.GameObject.transform.position).normalized;

            float internalAngle = Vector3.Dot(base.GameObject.transform.forward, collisionVec);

            // 전방벡터와 데미지 입은 방향벡터와의 내각을 이용하여 
            if (internalAngle > DOT_120_DEGREE)
            {
                base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

                return;
            }
        }

        // 현재 체력이 최대 체력이 아니라면 서서히 증가
        if (base.CreatureInformation.MonsterCurrentHP >= base.CreatureInformation.MonsterMaxHP)
        {
            base.CreatureInformation.MonsterCurrentHP = base.CreatureInformation.MonsterMaxHP;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            base.CreatureInformation.MonsterCurrentHP += _increaseHealing;
        }
    }
}
