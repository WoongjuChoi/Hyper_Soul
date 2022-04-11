using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviourPun, IDamageable
{
    [SerializeField]
    protected Slider _hpBarOverhead;

    public string NickName { get; set; } // 로그인 시 닉네임 넣을 것
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
    protected Canvas _hpBarOverheadCanvas;

    protected Animator _animator;
    protected bool _isHitting = false;

    public int Exp { get; set; }
    public int Level { get; set; }
    public int MaxLevel { get { return 5; } }
    public CharacterType Type { get; set; }

    protected DataManager _dataManager;

    public DataManager DataManager { get { return _dataManager; } }

    public virtual void Awake() { }

    private void LateUpdate()
    {
        _hpBarOverhead.value = (float)CurHp / MaxHp;
        _hpBarOverheadCanvas.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;
    }

    public virtual void OnCollisionEnter(Collision collision) { }

    [PunRPC]
    public void Respawn()
    {
        gameObject.SetActive(true);
    }

    public virtual void TakeDamage(int attackerID, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (IsDead)
            {
                return;
            }
            CurHp -= damageAmt;

            if (CurHp <= 0 && IsDead == false)
            {
                CurHp = 0;
                GameManager.Instance.SendDieMessage(PhotonView.Find(attackerID).GetComponent<LivingEntity>(), this);
                Die(attackerID);
                photonView.RPC("Die", RpcTarget.Others, attackerID);
            }
            else if (false == _isHitting)
            {
                Hit();
                photonView.RPC("Hit", RpcTarget.Others, null);
            }
            photonView.RPC("UpdateHp", RpcTarget.Others, CurHp);
            //photonView.RPC("TakeDamage", RpcTarget.Others, attackerID, damageAmt, hitPoint, hitNormal);
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
        if (photonView.IsMine)
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

    public virtual void Die(int attackerID)
    {
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