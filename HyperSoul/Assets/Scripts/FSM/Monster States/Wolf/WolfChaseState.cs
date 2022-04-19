using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChaseState : BaseState<WolfInformation>
{
    [SerializeField]
    private AudioSource _chaseAudioSource;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Chase;

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

            FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (CreatureInformation.IsWithinAttackRange)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        float distance = (GameObject.transform.position - CreatureInformation.InitializeTransform.position).magnitude;

        if (distance >= 20f)
        {
            FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        CreatureInformation.MonsterChaser.IsActive = true;
    }
}
