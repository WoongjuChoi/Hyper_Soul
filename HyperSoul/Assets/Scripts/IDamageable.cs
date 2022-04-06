using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal);
}
