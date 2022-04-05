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
    protected bool _isHit = false;

    private void Awake()
    {
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
    }

    private void LateUpdate()
    {
        _hpBar.value = (float)CurHp / MaxHp;
        _hpCanvas.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;
    }

    [PunRPC]
    public void UpdateHP(int newHp)
    {
        CurHp = newHp;
    }

    [PunRPC]
    public void Respawn()
    {
        IsDead = false;
        _deathSound.SetActive(false);
        gameObject.SetActive(true);
    }

    [PunRPC]
    public virtual void TakeDamage(LivingEntity attacker, int damageAmt, Vector3 hitPoint, Vector3 hitNormal)
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    CurHp -= damageAmt;
        //    photonView.RPC("UpdateHP", RpcTarget.Others, CurHp, IsDead);
        //    photonView.RPC("TakeDamage", RpcTarget.Others, damageAmt, hitPoint, hitNormal);

        //    if (false == _isHit)
        //    {
        //        StartCoroutine(Hit());
        //    }
        //}

        // 디버그용 TakeDamage 코드
        CurHp -= damageAmt;

        if (CurHp <= 0 && IsDead == false)
        {
            Die(attacker);
        }
        else if(false == _isHit)
        {
            StartCoroutine(Hit());
        }
    }
    // Hit시 애니메이션과 사운드, 이펙트 처리
    public IEnumerator Hit()
    {
        _isHit = true;
        _animator.SetBool(CommonAnimatorID.HIT, true);
        _hitImage.SetActive(true);
        _hitSound.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        _isHit = false;
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _animator.SetBool(CommonAnimatorID.HIT, false);
    }

    public void Die(LivingEntity attacker)
    {
        // 포톤 없이 돌리기 위해서 잠시 주석처리함
        //GameManager.Instance.SendDieMessage(this, attacker);
        _deathSound.SetActive(true);
        
        if (false == IsDead)
        {
            IsDead = true;
            _animator.SetTrigger(CommonAnimatorID.DIE);
        }
    }
}