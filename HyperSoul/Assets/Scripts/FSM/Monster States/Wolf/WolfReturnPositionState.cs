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
        CreatureInformation.MonsterCurrentState = EStateIDs.ReturnPosition;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public override void ExitState()
    {
        CreatureInformation.IsTargeting = false;

        CreatureInformation.MonsterChaser.ResetPath();

        CreatureInformation.MonsterChaser.IsActive = false;

        GameObject.transform.position = CreatureInformation.InitializePosition.position;

        GameObject.GetComponentInChildren<Animator>().SetBool(IS_WALK, false);

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public override void UpdateState()
    {
        _distance = (GameObject.transform.position - CreatureInformation.InitializePosition.position).magnitude;

        if (_distance <= 2f)
        {
            CreatureInformation.GetComponentInChildren<Animator>().SetBool(IS_WALK, true);
        }
        
        if (_distance <= 0.5f)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        // 현재 체력이 최대 체력이 아니라면 서서히 증가
        if (CreatureInformation.CurHp >= CreatureInformation.MaxHp)
        {
            CreatureInformation.CurHp = CreatureInformation.MaxHp;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            CreatureInformation.CurHp += _increaseHealing;
        }

        CreatureInformation.MonsterChaser.IsTarget = CreatureInformation.InitializePosition;

        CreatureInformation.MonsterChaser.IsActive = true;
    }
}
