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

    private bool _outOfSight = false;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        _animator.SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);
        _animator.SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        _treantRootAttack.StopAttack();
        _treantStompAttack.StopAttack();

        _outOfSight = false;
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

        if (false == CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }

        SetAttackPattern();

        if (_outOfSight)
        {
            FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

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
        else if (CreatureInformation.DistanceMonsterToTarget < 30f)
        {
            _treantStompAttack.StopAttack();

            _treantAttack = _treantRootAttack;
        }
        else
        {
            _outOfSight = true;
        }
    }
}
