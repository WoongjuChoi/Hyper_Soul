using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantIdleState : BaseState<TreantInformation>
{
    private int _increaseHealing = 0;

    private bool _isDoneHealing = false;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Idle;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;

        _isDoneHealing = false;

        CreatureInformation.OriginVec = GameObject.transform.forward;
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        IncreaseHealing();
    }

    private void IncreaseHealing()
    {
        if (false == _isDoneHealing)
        {
            // 현재 체력이 최대 체력이 아니라면 서서히 증가
            if (CreatureInformation.CurHp >= CreatureInformation.MaxHp)
            {
                CreatureInformation.CurHp = CreatureInformation.MaxHp;

                _increaseHealing = 0;

                CreatureInformation.SetMonsterHp(CreatureInformation.CurHp);

                _isDoneHealing = true;
            }
            else
            {
                float increaseHealing = Random.Range(0f, 1f);

                _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

                CreatureInformation.CurHp += _increaseHealing;

                CreatureInformation.SetMonsterHp(CreatureInformation.CurHp);
            }
        }
    }
}
