using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : MonoBehaviour
{
    private GameObject _misilleOwner = null;
    private Transform _target = null;
    private float _targetDistance = 0f;
    private Vector3 _launchPos;
    private AudioSource _missileLaunch;

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

    public Transform Target
    {
        set { _target = value; }
    }
    public GameObject MisilleOwner
    {
        get { return _misilleOwner; }
        set { _misilleOwner = value; }
    }

    void Start()
    {
        _missileLaunch = gameObject.AddComponent<AudioSource>();
        _missileLaunch.PlayOneShot(_launchSound);

        _launchPos = transform.position;
        if (_target != null)
        {
            StartCoroutine(SoftLaunch());
            _targetDistance = Vector3.Distance(_misilleOwner.transform.position, _target.transform.position);
            //Debug.Log(_targetDistance);
        }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * _gravityForce);

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

            if (_targetDistance + 10f < _curDistMissileAndLaunchPos)
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
    void Update()
    {
       
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌됨");
        Debug.Log(collision.gameObject.name);
        Explosion();
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
        // 오브젝트 풀링 구현되면 디스트로이 빼고 리턴으로
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
