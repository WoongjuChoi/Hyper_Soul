using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : BaseState<MonsterInfomations>
{
    private float _monsterDieTime = 2f;
    private float _elapsedTime = 0f;

    private const string IS_DIE = "isDie";

    public override void EnterState()
    {
        base.CreatureInfomation = base.GameObject.GetComponent<MonsterInfomations>();

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Die;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_DIE, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        base.GameObject.transform.rotation = Quaternion.identity;
    }

    public override void UpdateState()
    {
        if (-1f == _elapsedTime)
        {
            return;
        }

        MonsterDie();
    }

    private void MonsterDie()
    {
        if (_elapsedTime >= _monsterDieTime)
        {
            base.CreatureInfomation.IsDie = true;

            _elapsedTime = -1f;

            base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_DIE, false);

            base.GameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
