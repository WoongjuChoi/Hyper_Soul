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
    public int Exp { get; set; }
    public float Attack { get; set; }
    public bool IsDead { get; set; }
    public string CharacterName { get; set; }
    public int Level { get; set; }
    public CharacterType Type { get; set; }

    protected DataManager _dataManager;


    public virtual void UpdateLevelUpInfo(string info)
    {
        switch (Type)
        {
            case CharacterType.Monster:
                MonsterData monsterData = _dataManager.FindMonsterData(info);
                MaxHp = monsterData.MaxHp;
                CurHp = MaxHp;
                Attack = monsterData.Attack;
                Exp = monsterData.Exp;
                break;
        }
    }

    [PunRPC]
    public void UpdateHP(int newHp, bool isDead)
    {
        CurHp = newHp;
        IsDead = isDead;
    }
    [PunRPC]
    public virtual void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            CurHp -= damageAmt;
            photonView.RPC("UpdateHP", RpcTarget.Others, CurHp, IsDead);
            photonView.RPC("TakeDamage", RpcTarget.Others, damageAmt, hitPoint, hitNormal);
        }

        if(CurHp <= 0 && IsDead == false)
        {
            Die(attacker);
        }
    }

    private void Die(LivingEntity attacker)
    {
        GameManager.Instance.SendDieMessage(this, attacker);
        IsDead = true;
    }


}
