using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDieState : BaseState<WolfInformation>
{
    private bool _isDie = false;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Die;
    }

    public override void ExitState()
    {
        _isDie = false;
        MonsterObject.transform.rotation = Quaternion.identity;
    }

    public override void UpdateState()
    {
        if (false == _isDie)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        _isDie = true;

        yield return new WaitForSeconds(0.5f);

        PlayerInfo targetInfo = CreatureInformation.Target.GetComponent<PlayerInfo>();

        if (CreatureInformation.Level < CreatureInformation.MonsterMaxLevel)
        {
            ++CreatureInformation.Level;
        }
    }
}
