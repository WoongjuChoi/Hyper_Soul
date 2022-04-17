using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField]
    private float _speed = 100f;

    private Rigidbody _bulletRigidbody;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(ReceiveInfo), RpcTarget.MasterClient, ProjectileOwnerID, Attack);
        }

        _bulletRigidbody.velocity = Vector3.zero;
        _bulletRigidbody.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            _projectileReturn(gameObject);
        }
    }
}
