using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : LivingEntity
{
    [SerializeField]
    private PlayerInfo playerInfo;

    private void Start()
    {
        CurHp = 5;
        MaxHp = 5;
    }

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
