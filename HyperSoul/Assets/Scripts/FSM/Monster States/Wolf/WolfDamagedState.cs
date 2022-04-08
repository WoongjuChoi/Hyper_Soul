﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class WolfDamagedState : BaseState<WolfInformation>
{
    private Vector3 _lookAtTargetVec = Vector3.zero;

    private Vector3 _raycastOriginVec = Vector3.zero;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Damaged;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, true);

        base.CreatureInformation.MonsterAttackRangeCollider.enabled = false;

        _raycastOriginVec = base.CreatureInformation.CollisionVec;

        _lookAtTargetVec = base.CreatureInformation.LookAtTargetVec;
    }

    public override void ExitState()
    {
        base.CreatureInformation.IsDamaged = false;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DAMAGED, false);

        base.CreatureInformation.MonsterAttackRangeCollider.enabled = true;
    }

    public override void UpdateState()
    {
        // 데미지 받고 (수정 필요)
        base.CreatureInformation.MonsterCurrentHP -= 20;

        // HP <= 0 이면 Die 상태
        if (base.CreatureInformation.MonsterCurrentHP <= 0)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Die);

            return;
        }

        // 적을 한번 타겟팅 하면 이후 갱신 X
        // (활동 범위를 벗어나 되돌아 가거나 죽었을 때 타겟팅 해제)
        if (base.CreatureInformation.IsTargeting)
        {
            if (base.CreatureInformation.IsWithinAttackRange)
            {
                base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

                return;
            }
            else
            {
                base.FiniteStateMachine.ChangeState(EStateIDs.Chase);

                return;
            }
        }

        // 레이케스트 쐈을 때
            // 적이 있을때
                // 공격 범위 안이면 Attck 상태
                // else Chase 상태
            // 적이 없다면 Alert 상태
        // 레이케스트 아무것도 안맞았다면 Alert 상태

        RaycastHit hit;

        if (Physics.Raycast(_raycastOriginVec, _lookAtTargetVec, out hit, 1000f))
        {
            //// 디버깅용
            //Debug.Log($"_monsterInfo.Target.layer : {_monsterInfo.Target.layer}\nhit.collider.gameObject.layer :{hit.collider.gameObject.layer}");

            if (base.CreatureInformation.Target.layer == hit.collider.gameObject.layer)
            {
                base.CreatureInformation.MonsterChaser.IsTarget = base.CreatureInformation.Target.transform;

                base.CreatureInformation.IsTargeting = true;

                if (base.CreatureInformation.IsWithinAttackRange)
                {
                    base.FiniteStateMachine.ChangeState(EStateIDs.Attack);

                    return;
                }
                else
                {
                    base.FiniteStateMachine.ChangeState(EStateIDs.Chase);

                    return;
                }
            }
            else
            {
                base.FiniteStateMachine.ChangeState(EStateIDs.Alert);

                return;
            }
        }
        else
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Alert);

            return;
        }
    }
}