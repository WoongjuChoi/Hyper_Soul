using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantIdleState : BaseState<TreantInformation>
{
    private int _increaseHealing = 0;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Idle;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;

        CreatureInformation.OriginVec = GameObject.transform.forward;
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        // ���� ü���� �ִ� ü���� �ƴ϶�� ������ ����
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
    }
}
