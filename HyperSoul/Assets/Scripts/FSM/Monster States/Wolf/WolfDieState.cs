using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDieState : BaseState<WolfInformation>
{
    private float _monsterDieTime = 2f;
    private float _elapsedTime = 0f;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Die;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DIE, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        base.GameObject.transform.rotation = Quaternion.identity;
    }

    public override void UpdateState()
    {
        DieWolf();
    }

    private void DieWolf()
    {
        if (base.CreatureInformation.IsDie)
        {
            return;
        }

        if (_elapsedTime >= _monsterDieTime)
        {
            base.CreatureInformation.IsDie = true;

            base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_DIE, false);

            base.GameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
