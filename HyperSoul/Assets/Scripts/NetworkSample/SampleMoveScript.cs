using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 삭제할 것
public class SampleMoveScript : MonoBehaviour, IDamageable
{
    float maxDist = 50f;
    bool leftOrRight = false;

    public float moveSpeed = 20f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(leftOrRight == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if(transform.position.x >= 50f)
            {
                leftOrRight = true;
            }
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            if (transform.position.x <= -50f)
            {
                leftOrRight = false;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Projectile bullet = collision.gameObject.GetComponent<Projectile>();
        Debug.Log($"피격당함\n Attacker : {bullet.ProjectileOwner} \n Damage : {bullet.Attack}\nCollision Layer : {collision.gameObject.layer}");

    }

    public void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("TakeDamage 실행");
    }
}
