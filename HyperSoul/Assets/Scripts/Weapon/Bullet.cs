using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;

    [SerializeField]
    private float _speed = 100f;

    public float AttackValue = 1;

    private Rigidbody _bulletRigidbody;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //SetAttackValue(_playerInfo.Attack);
        _bulletRigidbody.velocity = transform.forward * _speed;
    }

    public void SetAttackValue(float attack)
    {
        AttackValue *= attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
