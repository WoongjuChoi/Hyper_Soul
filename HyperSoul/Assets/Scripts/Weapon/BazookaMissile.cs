using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : Projectile
{
    private Transform _target = null;
    private float _targetDistance = 0f;
    private Vector3 _launchPos;
    private AudioSource _missileLaunch;


    [SerializeField]
    AudioClip _launchSound = null;

    public GameObject MissilePrefab = null;
    public GameObject RocketParticleEffect = null;
    public GameObject ExplosionEffect = null;

    [SerializeField]
    private float _maxSpeed = 0f;
    private float _curSpeed = 0f;

    [SerializeField]
    private float _gravityForce = 10f;

    private bool _isLaunched = false;
    private bool _isHit = false;

    public Transform Target
    {
        set { _target = value; }
    }

    Coroutine lunchCoroutine = null;
    Coroutine explosionCoroutine = null;

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

        if (_target != null)
        {
            lunchCoroutine = StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        }
    }
    private void FixedUpdate()
    {
        if (false == photonView.IsMine || true == _isHit)
        {
            return;
        }

        GetComponent<Rigidbody>().AddForce(Vector3.up * _gravityForce);
        float _curDistMissileAndLaunchPos = Vector3.Distance(_launchPos, transform.position);
        if (_isLaunched == true)
        {
            if (_curSpeed <= _maxSpeed)
            {
                _curSpeed += _maxSpeed * Time.deltaTime;
            }
            transform.position += transform.forward * _curSpeed * Time.deltaTime;
        }
        if (_target != null)
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
                RocketParticleEffect.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"{collider.gameObject.name}¿¡¼­ Æø¹ßÇÔ");
        Explosion(transform.position);
        photonView.RPC(nameof(Explosion), RpcTarget.Others, transform.position);
    }

    private IEnumerator SoftLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        lunchCoroutine = null;
        RocketParticleEffect.SetActive(true);
        _isLaunched = true;
    }

    [PunRPC]
    private void Explosion(Vector3 pos)
    {
        explosionCoroutine = StartCoroutine(ExplosionCorountine(pos));
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
                    col.gameObject.GetComponent<MonsterInformation>().TakeMonsterDamage(Attack);
                }
            }
        }
        yield return new WaitForSeconds(1.3f);

        if (photonView.IsMine)
        {
            ReturnMissile();
        }
        explosionCoroutine = null;
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

        MissilePrefab.SetActive(true);
        RocketParticleEffect.SetActive(false);
        ExplosionEffect.SetActive(false);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        _isHit = false;

        _projectileReturn(gameObject);
    }
}
