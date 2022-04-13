using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmt);
    void TakeDamage(int attackerID, int damageAmt, Vector3 hitPoint, Vector3 hitNormal);
}
