using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : Projectile
{
    [SerializeField]
    AudioClip _launchSound;
    [SerializeField]
    private float _maxSpeed = 0f;
    [SerializeField]
    private float _gravityForce = 10f;

    public GameObject MissilePrefab = null;
    public GameObject RocketParticleEffect = null;
    public GameObject ExplosionEffect = null;

    private AudioSource _missileLaunch;
    private Coroutine lunchCoroutine = null;
    private Coroutine explosionCoroutine = null;
    private Transform _target;
    private Vector3 _launchPos;
    private float _targetDistance = 0f;
    private float _curSpeed = 0f;
    private bool _isLaunched = false;
    private bool _isHit = false;

    public Transform Target
    {
        set { _target = value; }
    }

    private const int PLAYER_LAYER = 10;
    private const int MONSTER_LAYER = 12;

    public void Init()
    {
        _curSpeed = 0f;

    }

    void OnEnable()
    {
        _missileLaunch = gameObject.AddComponent<AudioSource>();
        _missileLaunch.PlayOneShot(_launchSound);

        if (false == photonView.IsMine)
        {
            return;
        }

        photonView.RPC(nameof(ReceiveInfo), RpcTarget.MasterClient, ProjectileOwnerID, Attack);
        _launchPos = transform.position;

        if (null != _target)
        {
            lunchCoroutine = StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        }
    }
    private void FixedUpdate()
    {
        if (false == photonView.IsMine || _isHit)
        {
            return;
        }

        GetComponent<Rigidbody>().AddForce(Vector3.up * _gravityForce);
        float _curDistMissileAndLaunchPos = Vector3.Distance(_launchPos, transform.position);
        if (_isLaunched)
        {
            if (_curSpeed <= _maxSpeed)
            {
                _curSpeed += _maxSpeed * Time.deltaTime;
            }
            transform.position += transform.forward * _curSpeed * Time.deltaTime;
        }
        if (null != _target)
        {
            Vector3 dir = (_target.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.1f);

            if (_targetDistance + 30f < _curDistMissileAndLaunchPos)
            {
                Explosion(transform.position);
                photonView.RPC(nameof(Explosion), RpcTarget.Others, transform.position);
            }
        }
        else
        {
            _isLaunched = true;
            if (false == _isHit)
            {
                photonView.RPC(nameof(ActivateRocketEffect), RpcTarget.All, true);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Explosion(transform.position);
        photonView.RPC(nameof(Explosion), RpcTarget.Others, transform.position);
    }

    private void ReturnMissile()
    {
        if (null != lunchCoroutine)
        {
            StopCoroutine(lunchCoroutine);
        }

        if (null != explosionCoroutine)
        {
            StopCoroutine(explosionCoroutine);
        }


        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        _isHit = false;

        _projectileReturn(gameObject);
    }

    private IEnumerator SoftLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        lunchCoroutine = null;
        photonView.RPC(nameof(ActivateRocketEffect), RpcTarget.All, true);
        _isLaunched = true;
    }

    private IEnumerator ExplosionCorountine(Vector3 pos)
    {
        transform.position = pos;
        _isHit = true;
        _isLaunched = false;
        _curSpeed = 0f;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        MissilePrefab.SetActive(false);
        RocketParticleEffect.SetActive(false);
        ExplosionEffect.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            Collider[] colliders = Physics.OverlapSphere(pos, 5f);
            foreach (Collider col in colliders)
            {
                if (PLAYER_LAYER == col.gameObject.layer)
                {
                    col.gameObject.GetComponent<PlayerInfo>().TakeDamage(ProjectileOwnerID, Attack, pos, pos.normalized);
                }
                else if (MONSTER_LAYER == col.gameObject.layer)
                {
                    col.gameObject.GetComponent<MonsterInformation>().MonsterDamage(photonView.ViewID);
                }
            }
        }
        yield return new WaitForSeconds(1.3f);

        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SetInit), RpcTarget.All);
            ReturnMissile();
        }
        explosionCoroutine = null;
    }

    [PunRPC]
    private void ActivateRocketEffect(bool onoff)
    {
        RocketParticleEffect.SetActive(onoff);
    }

    [PunRPC]
    private void Explosion(Vector3 pos)
    {
        explosionCoroutine = StartCoroutine(ExplosionCorountine(pos));
    }

    [PunRPC]
    private void SetInit()
    {
        MissilePrefab.SetActive(true);
        RocketParticleEffect.SetActive(false);
        ExplosionEffect.SetActive(false);
    }
}
