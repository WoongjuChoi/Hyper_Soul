using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantAttackState : BaseState<TreantInformation>
{
    [SerializeField]
    private TreantRootAttack _treantRootAttack = null;
    [SerializeField]
    private TreantStompAttack _treantStompAttack = null;
    [SerializeField]
    private Animator _animator = null;

    private ITreantAttack _treantAttack;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        _animator.SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);
        _animator.SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        _treantRootAttack.StopAttack();
        _treantStompAttack.StopAttack();
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        if (false == CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.RotatePosition);

            return;
        }

        SetAttackPattern();

        if (CreatureInformation.Target.GetComponent<LivingEntity>().IsDead || CreatureInformation.OutOfSight)
        {
            CreatureInformation.IsTargeting = false;
            FiniteStateMachine.ChangeState(EMonsterStateIDs.ReturnPosition);

            return;
        }

        _treantAttack.Attack();
    }

    private void SetAttackPattern()
    {
        if (CreatureInformation.DistanceMonsterToTarget < 10f)
        {
            _treantRootAttack.StopAttack();
            _treantAttack = _treantStompAttack;
        }
        else if (CreatureInformation.DistanceMonsterToTarget < CreatureInformation.DistanceFromTarget)
        {
            _treantStompAttack.StopAttack();
            _treantAttack = _treantRootAttack;
        }
    }
}
