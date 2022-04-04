using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : LivingEntity
{
    int hp = 5;

    [SerializeField]
    private PlayerInfo playerInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            --CurHp;
            Debug.Log("Enemy Hp : " + CurHp);
            if (CurHp <= 0)
            {
                playerInfo.CurExp += 20;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (false == collision.gameObject.GetComponent<LivingEntity>().IsDead)
        {
            collision.gameObject.GetComponent<LivingEntity>()?.TakeDamage(this, 1, collision.contacts[0].point, collision.contacts[0].normal);
        }
    }
}
