using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviourPun, IDamageable
{
    [SerializeField]
    protected Slider _hpBar;
    public int CurHp { get; set; }
    public int MaxHp { get; set; }
    public float Attack { get; set; }
    public bool IsDead { get; set; }

    [PunRPC]
    public void UpdateHP(int newHp, bool isDead)
    {
        CurHp = newHp;
        IsDead = isDead;
    }
    [PunRPC]
    public virtual void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CurHp -= damageAmt;
            photonView.RPC("UpdateHP", RpcTarget.Others, CurHp, IsDead);
            photonView.RPC("TakeDamage", RpcTarget.Others, damageAmt, hitPoint, hitNormal);
        }

        if (CurHp <= 0 && IsDead == false)
        {
            Die(attacker);
        }
    }

    public void Die(LivingEntity attacker)
    {
        GameManager.Instance.SendDieMessage(this, attacker);
        IsDead = true;
    }
}