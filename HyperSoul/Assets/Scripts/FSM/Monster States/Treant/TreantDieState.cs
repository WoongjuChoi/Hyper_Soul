using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDieState : BaseState<TreantInformation>
{
    private float _monsterDieTime = 2f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Die;
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;
    }

    public override void UpdateState()
    {
        DieTreant();
    }

    private void DieTreant()
    {
        if (base.CreatureInformation.IsDie)
        {
            return;
        }

        if (_elapsedTime >= _monsterDieTime)
        {
            base.CreatureInformation.IsDie = true;

            base.GameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
