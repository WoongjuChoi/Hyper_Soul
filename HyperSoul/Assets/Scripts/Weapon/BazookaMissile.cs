using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : MonoBehaviour
{
    private PlayerInfo _misilleOwner = null;
    private Transform _target = null;
    private float _targetDistance = 0f;
    private Vector3 _launchPos;
    private AudioSource _missileLaunch;
    private int _attack = 30;

    [SerializeField]
    AudioClip _launchSound = null;
    [SerializeField]
    GameObject _missilePrefab = null;
    [SerializeField]
    GameObject _rocketParticleEffect = null;
    [SerializeField]
    GameObject _explosionEffect = null;
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
    public PlayerInfo MisilleOwner
    {
        set { _misilleOwner = value; }
    }

    public void ReceiveReturnMissileFunc(Action<GameObject> returnMissile)
    {
        _missileReturn = returnMissile;
    }

    void OnEnable()
    {
        _missileLaunch = gameObject.AddComponent<AudioSource>();
        _missileLaunch.PlayOneShot(_launchSound);

        _launchPos = transform.position;
        if (_target != null)
        {
            StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(_misilleOwner.transform.position, _target.transform.position);
        }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * _gravityForce);
        float _curDistMissileAndLaunchPos = Vector3.Distance(_launchPos, transform.position);

        if (_isHitted == false && _isLaunched == true)
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
                Explosion();
            }
        }
        else
        {
            _isLaunched = true;
            _rocketParticleEffect.SetActive(true);

            if(_curDistMissileAndLaunchPos > 70f)
            {
                Explosion();
                ReturnMissile();
            }
        }
    }
    void Update()
    {
       
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        Explosion();
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_misilleOwner, _attack, transform.position, transform.position.normalized);
        yield return new WaitForSeconds(1f);

        ReturnMissile();

    }

    private IEnumerator SoftLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        _rocketParticleEffect.SetActive(true);
        _isLaunched = true;
    }
    private void Explosion()
    {
        _isHitted = true;
        _explosionEffect.SetActive(true);
        _rocketParticleEffect.SetActive(false);
        _missilePrefab.SetActive(false);
    }

    private void ReturnMissile()
    {
        gameObject.SetActive(false);
        _missileReturn(this.gameObject);
    }
}
