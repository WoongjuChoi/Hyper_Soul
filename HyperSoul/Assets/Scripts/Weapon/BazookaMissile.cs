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

    public void Init()
    {
        _curSpeed = 0f;

    }

    void OnEnable()
    {
        //Debug.Log($"Ȱ��ȭ�� �����ȣ {photonView.ViewID} \n {ProjectileOwnerID}�� Ȱ��ȭ");
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(ReceiveInfo), RpcTarget.Others, ProjectileOwnerID, Attack);
        }

        _missileLaunch = gameObject.AddComponent<AudioSource>();
        _missileLaunch.PlayOneShot(_launchSound);
        _launchPos = transform.position;

        if (_target != null)
        {
            lunchCoroutine = StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * _gravityForce);
        float _curDistMissileAndLaunchPos = Vector3.Distance(_launchPos, transform.position);

        if (false == _isHit && _isLaunched == true)
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
            if (_targetDistance + 10f < _curDistMissileAndLaunchPos)
            {
                Explosion(transform.position);
                photonView.RPC("Explosion", RpcTarget.Others, transform.position);
            }
        }
        else
        {
            _isLaunched = true;
            RocketParticleEffect.SetActive(true);

            if (_curDistMissileAndLaunchPos > 200f)
            {
                Explosion(transform.position);
                photonView.RPC("Explosion", RpcTarget.Others, transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Explosion(transform.position);
        GetComponent<PhotonView>().RPC("Explosion", RpcTarget.Others, transform.position);
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
        ExplosionEffect.SetActive(true);
        RocketParticleEffect.SetActive(false);
        MissilePrefab.SetActive(false);

        yield return new WaitForSeconds(1.3f);

        if(photonView.IsMine)
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

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        _isHit = false;

        _projectileReturn(gameObject);
    }
}
