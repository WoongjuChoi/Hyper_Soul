using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfReturnPositionState : BaseState<WolfInformation>
{
    private float _distance = 0f;

    private int _increaseHealing = 0;

    private const string IS_WALK = "isWalk";

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.ReturnPosition;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        base.CreatureInformation.IsTargeting = false;

        base.CreatureInformation.MonsterChaser.ResetPath();

        base.CreatureInformation.MonsterChaser.IsActive = false;

        base.GameObject.transform.position = base.CreatureInformation.InitializePosition.position;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_WALK, false);

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        _distance = (base.GameObject.transform.position - base.CreatureInformation.InitializePosition.position).magnitude;

        if (_distance <= 2f)
        {
            base.CreatureInformation.GetComponentInChildren<Animator>().SetBool(IS_WALK, true);
        }
        
        if (_distance <= 0.5f)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
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

        base.CreatureInformation.MonsterChaser.IsTarget = base.CreatureInformation.InitializePosition;

        base.CreatureInformation.MonsterChaser.IsActive = true;
    }
}
