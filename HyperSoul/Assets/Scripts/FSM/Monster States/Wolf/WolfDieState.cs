using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDieState : BaseState<WolfInformation>
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

        GameObject.transform.rotation = Quaternion.identity;
    }

    public override void UpdateState()
    {
        DieWolf();
    }

    private void DieWolf()
    {
        if (CreatureInformation.IsDie)
        {
            return;
        }

        if (_elapsedTime >= _monsterDieTime)
        {
            CreatureInformation.IsDie = true;

            GameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
