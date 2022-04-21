using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantIdleState : BaseState<TreantInformation>
{
    private int _increaseHealing = 0;
    private bool _isDoneHealing = false;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Idle;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;
        _isDoneHealing = false;
        CreatureInformation.OriginVec = MonsterObject.transform.forward;
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        IncreaseHealing();
    }

    private void IncreaseHealing()
    {
        if (false == _isDoneHealing)
        {
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
