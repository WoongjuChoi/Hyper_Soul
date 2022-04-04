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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            --hp;
            Debug.Log("Enemy Hp : " + hp);
            if (hp <= 0)
            {
                playerInfo.CurrExp += 20;
                Destroy(gameObject);
            }
        }
    }
}
