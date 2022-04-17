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

    public string NickName { get; set; } // �α��� �� �г��� ���� ��
    public int MaxHp { get; set; }
    public int Attack { get; set; }
    public bool IsDead { get; set; }
    public int Score { get; set; }

    public int CurHp { get; set; }
    public int CurExp { get; set; }
    public int CurScore { get; set; }

    // ���� �߰��� ������
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

    [SerializeField]
    protected Canvas _profileCanvas;
    [SerializeField]
    protected Text _levelText;

    public virtual void Awake() { }

    private void LateUpdate()
    {
        _hpBarOverhead.value = (float)CurHp / MaxHp;
        _hpBarOverheadCanvas.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;
    }

    public virtual void OnCollisionEnter(Collision collision) { }

    public void TakeMonsterDamage(int damageAmt)
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
                Die();
                photonView.RPC("Die", RpcTarget.Others);
            }
            else if (false == _isHitting)
            {
                Hit();
                photonView.RPC("Hit", RpcTarget.Others, null);
            }
            photonView.RPC("UpdateHp", RpcTarget.Others, CurHp);
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

            if (CurHp <= 0 && IsDead == false)
            {
                CurHp = 0;
                //GameManager.Instance.SendDieMessage(PhotonView.Find(attackerID).GetComponent<LivingEntity>(), this);
                Die();
                photonView.RPC("Die", RpcTarget.Others);

                Debug.Log($"Score : {Score}");

                GiveScore(attackerID, Score);
                GiveExp(attackerID, Exp);
            }
            else if (false == _isHitting)
            {
                Hit();
                photonView.RPC("Hit", RpcTarget.Others);
            }
            photonView.RPC("UpdateHp", RpcTarget.Others, CurHp);
            //photonView.RPC("TakeDamage", RpcTarget.Others, attackerID, damageAmt, hitPoint, hitNormal);
        }
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

    // Hit�� �ִϸ��̼ǰ� ����, ����Ʈ ó��
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

    [PunRPC]
    public virtual void Die()
    {
        _deathSound.SetActive(true);

        IsDead = true;
        if (photonView.IsMine)
        {
            _animator.SetTrigger(CommonAnimatorID.DIE);

            Invoke(nameof(Respawn), 1.5f);
        }
    }

    public virtual void Respawn()
    {
        gameObject.SetActive(false);
    }

    public virtual void UpdateLevelUpInfo(string info)
    {
        switch (Type)
        {
            case CharacterType.Monster:
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
        if (photonView.IsMine)
        {
            LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

            attacker.CurScore += score;
        }
    }

    public void GiveExp(int attackerID, int expAmt)
    {
        if (photonView.IsMine)
        {
            LivingEntity attacker = PhotonView.Find(attackerID).GetComponent<LivingEntity>();

            attacker.CurExp += expAmt;
        }
    }
}