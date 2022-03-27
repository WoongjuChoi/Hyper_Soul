using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : MonoBehaviour
{
    private GameObject _misilleOwner = null;
    private Transform _target = null;
    private float _targetDistance = 0f;
    private Vector3 _launchPos;

    [SerializeField]
    GameObject _missilePrefab = null;
    [SerializeField]
    GameObject _rocketParticleEffect = null;
    [SerializeField]
    GameObject _explosionEffect = null;
    [SerializeField]
    private float _maxSpeed = 0f;
    private float _curSpeed = 0f;

    private bool _isLaunched = false;
    private bool _isHitted = false;

    public Transform Target
    {
        set { _target = value; }
    }
    public GameObject MisilleOwner
    {
        set { _misilleOwner = value; }
    }

    void Start()
    {
        _launchPos = transform.position;
        if (_target != null)
        {
            StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(_misilleOwner.transform.position, _target.transform.position);
            Debug.Log(_targetDistance);
        }
    }

    void Update()
    {
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

            float _curDistMissileAndLaunchPos = Vector3.Distance(_launchPos, transform.position);
            
            if (_targetDistance < _curDistMissileAndLaunchPos)
            {
                
                Explosion();
            }
        }
        else
        {
            _isLaunched = true;
            _rocketParticleEffect.SetActive(true);
        }
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ãæµ¹µÊ");
        Explosion();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        Destroy(this);
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
        _missilePrefab.SetActive(false);
    }
}
