using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChaseState : BaseState<WolfInformation>
{
    [SerializeField]
    private AudioSource _chaseAudioSource;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Chase;
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_CHASE, true);
        _chaseAudioSource.Play();
    }

    public override void ExitState()
    {
        CreatureInformation.MonsterChaser.IsActive = false;
        CreatureInformation.MonsterChaser.ResetPath();
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_CHASE, false);
        _chaseAudioSource.Stop();
    }

    public override void UpdateState()
    {
        if (CreatureInformation.Target.GetComponent<LivingEntity>().IsDead)
        {
            CreatureInformation.IsTargeting = false;
            FiniteStateMachine.ChangeState(EMonsterStateIDs.ReturnPosition);

            return;
        }

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        if (CreatureInformation.IsWithinAttackRange)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Attack);

            return;
        }

        float distance = (MonsterObject.transform.position - CreatureInformation.InitializeTransform.position).magnitude;

        if (distance >= 20f)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.ReturnPosition);

            return;
        }

        CreatureInformation.MonsterChaser.IsActive = true;
    }
}
