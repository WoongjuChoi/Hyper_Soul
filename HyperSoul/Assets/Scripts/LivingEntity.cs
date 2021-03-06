using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviourPun, IDamageable, IGiveExp, IGiveScore
{
    [SerializeField]
    protected Slider _hpBarOverhead;
    [SerializeField]
    protected GameObject _hitSound;
    [SerializeField]
    protected GameObject _deathSound;
    [SerializeField]
    protected GameObject _hitImage;
    [SerializeField]
    protected Canvas _hpBarOverheadCanvas;
    [SerializeField]
    protected Canvas _profileCanvas;
    [SerializeField]
    protected Text _levelText;

    protected Animator _animator;
    protected bool _isHitting = false;

    public Animator CreatureAnimator { get { return _animator; } }
    public int MaxLevel { get { return 5; } }

    public string NickName { get; set; }
    public int MaxHp { get; set; }
    public int Attack { get; set; }
    public bool IsDead { get; set; }
    public int Score { get; set; }
    public int CurHp { get; set; }
    public int CurExp { get; set; }
    public int CurScore { get; set; }
    public int Exp { get; set; }
    public int Level { get; set; }
    public ECharacterType Type { get; set; }

    public virtual void Awake() { }

    private void LateUpdate()
    {
        _hpBarOverhead.value = (float)CurHp / MaxHp;
        _hpBarOverheadCanvas.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;
    }

    public virtual void OnCollisionEnter(Collision collision) { }

    public virtual void OnTriggerEnter(Collider other) { }

    public void TakeMonsterDamage(int damageAmt)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (IsDead)
            {
                return;
            }
            CurHp -= damageAmt;
            photonView.RPC("UpdateHp", RpcTarget.Others, CurHp);

            if (CurHp <= 0 && false == IsDead)
            {
                CurHp = 0;
                Die();
                photonView.RPC("Die", RpcTarget.Others);
            }
            else if (false == _isHitting)
            {
                Hit();
                photonView.RPC("Hit", RpcTarget.Others, null);
            }
        }
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

            if (CurHp <= 0)
            {
                CurHp = 0;
            }

            photonView.RPC("UpdateHp", RpcTarget.Others, CurHp);

            if (CurHp <= 0 && false == IsDead && attackerID != photonView.ViewID)
            {
                GiveScore(attackerID, Score);
                GiveExp(attackerID, Exp);
            }
        }

        if (CurHp <= 0 && false == IsDead)
        {
            CurHp = 0;
            photonView.RPC(nameof(Die), RpcTarget.AllBuffered);

            GameManager.Chat.SendDieMessage(PhotonView.Find(attackerID).GetComponent<LivingEntity>(), this);
        }
        else if (false == _isHitting && IsDead)
        {
            Hit();
            photonView.RPC("Hit", RpcTarget.Others);
        }
    }

    // Hit?? ???????????? ??????, ?????? ????
    public IEnumerator HitCorountine()
    {
        _isHitting = true;
        _animator.SetBool(CommonAnimatorID.HIT, true);
        if (photonView.IsMine)
        {
            _hitImage.SetActive(true);
        }
        _hitSound.SetActive(true);
        yield return new WaitForSeconds(0.01f);

        _isHitting = false;
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _animator.SetBool(CommonAnimatorID.HIT, false);
    }

    private IEnumerator DieCoroutine()
    {
        _animator.SetBool(CommonAnimatorID.DIE, true);

        yield return new WaitForSeconds(0.1f);

        _animator.SetBool(CommonAnimatorID.DIE, false);
    }

    public virtual void Respawn() { }

    public virtual void UpdateLevelUpInfo(string info)
    {
        switch (Type)
        {
            case ECharacterType.Monster:
                MonsterData monsterData = DataManager.Instance.FindMonsterData(info);
                MaxHp = monsterData.MaxHp;
                CurHp = MaxHp;
                Attack = monsterData.Attack;
                Exp = monsterData.Exp;
                break;
        }
    }

    public void GiveScore(int attackerID, int score)
    {
        LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

        attacker.CurScore += score;

        int newScore = attacker.CurScore;

        photonView.RPC(nameof(UpdateScore), RpcTarget.Others, attackerID, newScore);
    }

    public void GiveExp(int attackerID, int expAmt)
    {
        LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

        attacker.CurExp += expAmt;

        int newExpAmt = attacker.CurExp;

        photonView.RPC(nameof(UpdateExp), RpcTarget.Others, attackerID, newExpAmt);
    }

    [PunRPC]
    public void UpdateHp(int hp)
    {
        CurHp = hp;
    }

    [PunRPC]
    public void Hit()
    {
        StartCoroutine(HitCorountine());
    }

    [PunRPC]
    public virtual void Die()
    {
        _deathSound.SetActive(true);

        IsDead = true;
        if (photonView.IsMine)
        {
            StartCoroutine(DieCoroutine());

            Respawn();
        }
    }

    [PunRPC]
    public void UpdateScore(int attackerID, int newScore)
    {
        LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

        attacker.CurScore = newScore;
    }

    [PunRPC]
    public void UpdateExp(int attackerID, int newExpAmt)
    {
        LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

        attacker.CurExp = newExpAmt;
    }
}