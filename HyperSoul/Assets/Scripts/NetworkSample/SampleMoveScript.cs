using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("�׽�Ʈ���� ��ũ��Ʈ�� ��������")]
public class SampleMoveScript : MonoBehaviour
{
    float maxDist = 50f;
    bool leftOrRight = false;

    public float moveSpeed = 20f;

    private const int AMMO_COLLIDER = 11;
    private const int MONSTER_ATTACK_COLLIDER = 13;

    void Update()
    {
        if(leftOrRight == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if(transform.position.x >= 330f)
            {
                leftOrRight = true;
            }
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            if (transform.position.x <= 130f)
            {
                leftOrRight = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();

        if (AMMO_COLLIDER == collider.gameObject.layer)
        {
            Debug.Log($"�ǰݴ���\n Attacker : {projectile.ProjectileOwnerID}" +
           $"\n Damage : {projectile.Attack}");

        }

        //LivingEntity livingEntity = collider.gameObject.GetComponentInParent<LivingEntity>();

        //if (MONSTER_ATTACK_COLLIDER == collider.gameObject.layer)
        //{
        //    Debug.Log($"�ǰݴ���\nAttacker : {livingEntity.gameObject.name}" +
        //            $"\nDamage : {livingEntity.Attack}" +
        //            $"\nHP : {CurHp}");

        //}

    }
}
