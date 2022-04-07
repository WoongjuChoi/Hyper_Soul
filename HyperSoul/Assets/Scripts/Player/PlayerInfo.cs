using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : LivingEntity
{
    public int CurExp { get; set; }
    private int MaxExp { get; set; }
    private int SkillAttack { get; set; }

    public string NickName { get; set; }

    public void OnEnable()
    {
        //if(!photonView.IsMine)
        //{
        //    return;
        //}
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        IsDead = false;
        //UpdateLevelUpInfo(CharacterName + Level);
    }

    [PunRPC]
    public override void UpdateLevelUpInfo(string info)
    {
        PlayerData playerData = _dataManager.FindPlayerData(info);
        MaxHp = playerData.MaxHp;
        CurHp = MaxHp;
        Attack = playerData.Attack;
        MaxExp = playerData.MaxExp;
        Exp = playerData.Exp;
        SkillAttack = playerData.SkillAttack;
        CurExp = 0;
    }

    private void LevelUp()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerParameter.BULLET)
        {
            Debug.Log($"{PhotonNetwork.NickName}에 충돌");
            LivingEntity attacker = collision.gameObject.GetComponent<LivingEntity>();
            Projectile bullet = collision.gameObject.GetComponent<Projectile>();
            TakeDamage(attacker, bullet.Attack, collision.gameObject.transform.position, collision.gameObject.transform.position.normalized);
        }

    }
    [PunRPC]
    public override void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("데미지 입음");
        if(IsDead == false)
        {
            base.TakeDamage(attacker, damageAmt, hitPoint, hitNormal);
        }
    }
}
