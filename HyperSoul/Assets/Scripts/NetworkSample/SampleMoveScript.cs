using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("�׽�Ʈ���� ��ũ��Ʈ�� ��������")]
public class SampleMoveScript : MonoBehaviour
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
        Debug.Log($"�ǰݴ���\n Attacker : {collision.gameObject.GetComponent<Projectile>().ProjectileOwnerID}" +
            $"\n Damage : {collision.gameObject.GetComponent<Projectile>().Attack}");
    }
}
