using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    int hp = 5;

    [SerializeField]
    private PlayerInfo playerInfo;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            --hp;
            Debug.Log("Enemy Hp : " + hp);
            if (hp <= 0)
            {
                playerInfo.CurrExp += 10;
                Destroy(this);
            }
        }
    }
}
