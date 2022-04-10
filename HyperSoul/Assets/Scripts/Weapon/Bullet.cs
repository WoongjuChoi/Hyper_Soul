using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField]
    private PlayerInfo _playerInfo;

    [SerializeField]
    private float _speed = 100f;

    public float AttackValue = 1;

    private Rigidbody _bulletRigidbody;
    private event Action<GameObject> _returnBullet;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _bulletRigidbody.velocity = Vector3.zero;
        _bulletRigidbody.velocity = transform.forward * _speed;
    }

    public void SetBulletReturnFunc(Action<GameObject> bulletReturnFunc)
    {
        _returnBullet = bulletReturnFunc;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        _returnBullet(this.gameObject);
    }
}
