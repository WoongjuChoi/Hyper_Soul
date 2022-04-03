using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(GameObject attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal);
}
