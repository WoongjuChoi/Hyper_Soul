using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRootAttack : ITreantAttack
{
    public void Attack(GameObject obj)
    {
        obj.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, true);
        obj.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);
    }
}
