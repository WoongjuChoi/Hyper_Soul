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
    private bool _isHitted = false;

    private event Action<GameObject> _missileReturn;

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

        if (false == _isHitted && _isLaunched == true)
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
            //transform.forward = dir;
            if (_targetDistance + 10f < _curDistMissileAndLaunchPos)
            {
                Explosion();
            }
        }
        else
        {
            _isLaunched = true;
            RocketParticleEffect.SetActive(true);

            if (_curDistMissileAndLaunchPos > 70f)
            {
                Explosion();
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
        photonView.RPC("Explosion", RpcTarget.Others);
    }

    private IEnumerator SoftLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        lunchCoroutine = null;
        RocketParticleEffect.SetActive(true);
        _isLaunched = true;
    }

    [PunRPC]
    private void Explosion()
    {
        explosionCoroutine = StartCoroutine(ExplosionCorountine());
    }
    private IEnumerator ExplosionCorountine()
    {
        _isHitted = true;
        _isLaunched = false;
        _curSpeed = 0f;
        ExplosionEffect.SetActive(true);
        RocketParticleEffect.SetActive(false);
        MissilePrefab.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        ReturnMissile();
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
        _isHitted = false;
        gameObject.SetActive(false);
        _missileReturn(this.gameObject);
    }

    public void ReceiveReturnMissileFunc(Action<GameObject> returnMissile)
    {
        _missileReturn = returnMissile;
    }

}
