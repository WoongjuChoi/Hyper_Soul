using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class WolfDamagedState : BaseState<WolfInformation>
{
    [SerializeField]
    private AudioSource _hurtAudioSource;

    private Vector3 _lookAtTargetVec;

    private Vector3 _raycastOriginVec;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Damaged;

        CreatureInformation.MonsterAttackRangeCollider.enabled = false;

        _raycastOriginVec = CreatureInformation.CollisionVec;

        _lookAtTargetVec = CreatureInformation.LookAtTargetVec;

        _hurtAudioSource.Play();
    }

    public override void ExitState()
    {
        CreatureInformation.IsDamaged = false;

        CreatureInformation.MonsterAttackRangeCollider.enabled = true;
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

        // 적을 한번 타겟팅 하면 이후 갱신 X
        // (활동 범위를 벗어나 되돌아 가거나 죽었을 때 타겟팅 해제)
        if (CreatureInformation.IsTargeting)
        {
            if (CreatureInformation.IsWithinAttackRange)
            {
                FiniteStateMachine.ChangeState(EStateIDs.Attack);

                return;
            }
            else
            {
                FiniteStateMachine.ChangeState(EStateIDs.Chase);

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

        if (Physics.Raycast(_raycastOriginVec, _lookAtTargetVec.normalized, out hit, _lookAtTargetVec.magnitude, 1 << CreatureInformation.Target.layer))
        {
            //// 디버깅용
            //Debug.Log($"_monsterInfo.Target.layer : {_monsterInfo.Target.layer}\nhit.collider.gameObject.layer :{hit.collider.gameObject.layer}");

            if (CreatureInformation.Target.layer == hit.collider.gameObject.layer)
            {
                CreatureInformation.MonsterChaser.IsTarget = CreatureInformation.Target.transform;

                CreatureInformation.IsTargeting = true;

                if (CreatureInformation.IsWithinAttackRange)
                {
                    FiniteStateMachine.ChangeState(EStateIDs.Attack);

                    return;
                }
                else
                {
                    FiniteStateMachine.ChangeState(EStateIDs.Chase);

                    return;
                }
            }
            else
            {
                FiniteStateMachine.ChangeState(EStateIDs.Alert);

                return;
            }
        }
        else
        {
            FiniteStateMachine.ChangeState(EStateIDs.Alert);

            return;
        }
    }
}
