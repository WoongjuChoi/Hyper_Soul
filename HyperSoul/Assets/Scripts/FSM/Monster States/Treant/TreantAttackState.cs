using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantAttackState : BaseState<TreantInformation>
{
    private TreantAttackManager _treantAttackManager = new TreantAttackManager();

    private TreantRootAttack _treantRootAttack = new TreantRootAttack();
    private TreantStompAttack _treantStompAttack = new TreantStompAttack();

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);
        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        base.CreatureInformation.StompAttackArea.SetActive(false);
    }

    public override void UpdateState()
    {
        if (base.CreatureInformation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (false == base.CreatureInformation.ExistInSight)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.RotatePosition);

            return;
        }

        SetAttackPattern();

        _treantAttackManager.Attack(base.GameObject);
    }

    private void SetAttackPattern()
    {
        if (base.CreatureInformation.DistanceMonsterToTarget < 10f)
        {
            _treantAttackManager.SetTreantAttack(_treantStompAttack);
        }
        else if (base.CreatureInformation.DistanceMonsterToTarget < 50f)
        {
            _treantAttackManager.SetTreantAttack(_treantRootAttack);
        }
    }
}
