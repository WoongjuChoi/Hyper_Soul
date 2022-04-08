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
    public int Attack { get; set; }
    public bool IsDead { get; set; }

    // 새로 추가된 변수들
    [SerializeField]
    protected GameObject _hitSound;
    [SerializeField]
    protected GameObject _deathSound;
    [SerializeField]
    protected GameObject _hitImage;
    [SerializeField]
    protected Canvas _hpCanvas;

    protected Animator _animator;
    protected bool _isHitting = false;

   
    public int Exp { get; set; }
    public string CharacterName { get; set; } // 바주카인지 라이플인지 이런거 넣기
    public int Level { get; set; }
    public CharacterType Type { get; set; }

    protected DataManager _dataManager;

    private void Awake()
    {
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        if(photonView.IsMine)
        {
            _hpBar.gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        _hpBar.value = (float)CurHp / MaxHp;
        _hpCanvas.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.gameObject.GetComponent<Projectile>() != null)
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().ProjectileOwner, collision.gameObject.GetComponent<Projectile>().Attack,
                collision.transform.position, collision.transform.position.normalized);
        }
        
    }

    [PunRPC]
    public void Respawn()
    {
        IsDead = false;
        _deathSound.SetActive(false);
        gameObject.SetActive(true);
    }

    public virtual void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CurHp -= damageAmt;
            Hit();
            photonView.RPC("UpdateHP", RpcTarget.Others, CurHp);
            photonView.RPC("OnDamage", RpcTarget.Others, attacker, damageAmt, hitPoint, hitNormal);


        }
    }

    [PunRPC]
    protected void OnDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead)
        {
            return;
        }

        if (CurHp <= 0 && IsDead == false)
        {
            Die(attacker);
        }
        else if (false == _isHitting)
        {
            photonView.RPC("Hit", RpcTarget.Others, null);
        }
    }

    [PunRPC]
    public void UpdateHP(int newHp)
    {
        CurHp = newHp;
    }

    [PunRPC]
    private void Hit()
    {
        StartCoroutine(HitCorountine());
    }

    // Hit시 애니메이션과 사운드, 이펙트 처리
    public IEnumerator HitCorountine()
    {
        _isHitting = true;
        _animator.SetBool(CommonAnimatorID.HIT, true);
        if(photonView.IsMine)
        {
            _hitImage.SetActive(true);
        }
        _hitSound.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        _isHitting = false;
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _animator.SetBool(CommonAnimatorID.HIT, false);
    }

    public void Die(LivingEntity attacker)
    {
        GameManager.Instance.SendDieMessage(this, attacker);
        _deathSound.SetActive(true);
        
        if (false == IsDead)
        {
            IsDead = true;
            _animator.SetTrigger(CommonAnimatorID.DIE);
        }
    }

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

}