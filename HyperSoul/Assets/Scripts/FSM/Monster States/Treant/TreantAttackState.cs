using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantAttackState : BaseState<TreantInformation>
{
    [SerializeField]
    private TreantRootAttack _treantRootAttack = null;

    [SerializeField]
    private TreantStompAttack _treantStompAttack = null;

    private TreantAttackManager _treantAttackManager = new TreantAttackManager();

    private bool _outOfSight = false;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);
        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        _treantRootAttack.StopAttack();
        _treantStompAttack.StopAttack();

        base.CreatureInformation.StompAttackArea.SetActive(false);

        _outOfSight = false;
    }

    public override void UpdateState()
    {
        SetAttackPattern();

        if (base.CreatureInformation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }
        
        if (_outOfSight)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.ReturnPosition);

            return;
        }

        if (false == base.CreatureInformation.ExistInSight)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }

        _treantAttackManager.Attack();
    }

    private void SetAttackPattern()
    {
        if (base.CreatureInformation.DistanceMonsterToTarget < 10f)
        {
            _treantRootAttack.StopAttack();

            _treantAttackManager.SetTreantAttack(_treantStompAttack);
        }
        else if (base.CreatureInformation.DistanceMonsterToTarget < 30f)
        {
            _treantStompAttack.StopAttack();

            _treantAttackManager.SetTreantAttack(_treantRootAttack);
        }
        else
        {
            _outOfSight = true;
        }
    }
}
