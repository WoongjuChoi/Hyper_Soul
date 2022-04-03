using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : LivingEntity
{
    PlayerData _data;
    private string _nickName;
    public string NickName
    {
        get { return _nickName; }
    }

    private void OnEnable()
    {
        Attack = _data.Attack;
        MaxHp = _data.MaxHp;
        CurHp = MaxHp;
        IsDead = false;

    }

    private void OnCollisionEnter(Collider collision)
    {
        if (collision.gameObject.tag == TagParameterID.BULLET)
        {
            //TakeDamage()
        }

    }
    [PunRPC]
    public override void TakeDamage(GameObject attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead == false)
        {
            base.TakeDamage(attacker, damageAmt, hitPoint, hitNormal);
        }
    }
}
