using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantDieState : BaseState<TreantInformation>
{
    private float _monsterDieTime = 2f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Die;
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
        if (CreatureInformation.IsDie)
        {
            return;
        }

        if (_elapsedTime >= _monsterDieTime)
        {
            CreatureInformation.IsDie = true;

            ++CreatureInformation.Level;

            if (CreatureInformation.Level > CreatureInformation.MonsterMaxLevel)
            {
                CreatureInformation.Level = CreatureInformation.MonsterMaxLevel;
            }

            GameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
