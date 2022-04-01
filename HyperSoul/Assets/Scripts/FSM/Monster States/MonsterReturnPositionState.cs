using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnPositionState : BaseState<MonsterInfomations>
{
    private float _distance = 0f;

    private int _increaseHealing = 0;

    private const string IS_WALK = "isWalk";

    public override void EnterState()
    {
        base.CreatureInfomation = base.GameObject.GetComponent<MonsterInfomations>();

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.ReturnPosition;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        base.CreatureInfomation.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_IDLE);

        base.CreatureInfomation.IsTargeting = false;

        base.CreatureInfomation.MonsterChaser.ResetPath();

        base.CreatureInfomation.MonsterChaser.IsActive = false;

        base.GameObject.transform.position = base.CreatureInfomation.InitializePosition.position;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_WALK, false);

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        _distance = (base.GameObject.transform.position - base.CreatureInfomation.InitializePosition.position).magnitude;

        if (_distance <= 2f)
        {
            base.CreatureInfomation.GetComponentInChildren<Animator>().SetBool(IS_WALK, true);
        }
        
        if (_distance <= 0.5f)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        // 현재 체력이 최대 체력이 아니라면 서서히 증가
        if (base.CreatureInfomation.MonsterCurrentHP >= base.CreatureInfomation.MonsterMaxHP)
        {
            base.CreatureInfomation.MonsterCurrentHP = base.CreatureInfomation.MonsterMaxHP;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            base.CreatureInfomation.MonsterCurrentHP += _increaseHealing;
        }

        base.CreatureInfomation.MonsterChaser.IsTarget = base.CreatureInfomation.InitializePosition;

        base.CreatureInfomation.MonsterChaser.IsActive = true;
    }
}
