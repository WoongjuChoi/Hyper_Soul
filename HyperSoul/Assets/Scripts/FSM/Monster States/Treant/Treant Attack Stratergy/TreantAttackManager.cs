using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantAttackManager
{
    private ITreantAttack _treantAttack;

    public void SetTreantAttack(ITreantAttack treantAttack)
    {
        this._treantAttack = treantAttack;
    }

    public void Attack(GameObject obj)
    {
        _treantAttack.Attack(obj);
    }
}
