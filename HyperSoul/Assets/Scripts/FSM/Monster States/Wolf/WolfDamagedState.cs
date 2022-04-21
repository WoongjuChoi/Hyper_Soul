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
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Damaged;
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
        CreatureInformation.TakeDamage(CreatureInformation.AttackerInfo.ProjectileOwnerID, CreatureInformation.Attacker.GetComponent<LivingEntity>().Attack, Vector3.zero, Vector3.zero);
        
        if (CreatureInformation.CurHp <= 0)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Die);

            return;
        }

        if (CreatureInformation.IsTargeting)
        {
            if (CreatureInformation.IsWithinAttackRange)
            {
                FiniteStateMachine.ChangeState(EMonsterStateIDs.Attack);

                return;
            }
            else
            {
                FiniteStateMachine.ChangeState(EMonsterStateIDs.Chase);

                return;
            }
        }

        RaycastHit hit;

        if (Physics.Raycast(_raycastOriginVec, _lookAtTargetVec.normalized, out hit, _lookAtTargetVec.magnitude, 1 << CreatureInformation.Target.layer))
        {
            if (CreatureInformation.Target.layer == hit.collider.gameObject.layer)
            {
                CreatureInformation.MonsterChaser.IsTarget = CreatureInformation.Target.transform;
                CreatureInformation.IsTargeting = true;

                if (CreatureInformation.IsWithinAttackRange)
                {
                    FiniteStateMachine.ChangeState(EMonsterStateIDs.Attack);

                    return;
                }
                else
                {
                    FiniteStateMachine.ChangeState(EMonsterStateIDs.Chase);

                    return;
                }
            }
            else
            {
                FiniteStateMachine.ChangeState(EMonsterStateIDs.Alert);

                return;
            }
        }
        else
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Alert);

            return;
        }
    }
}
