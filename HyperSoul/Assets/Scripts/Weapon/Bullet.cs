using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;

    public float AttackValue = 1;

    private float _speed = 15f;

    private void OnEnable()
    {
        SetAttackValue(_playerInfo.Attack);
    }



    public void SetAttackValue(float attack)
    {
        AttackValue *= attack;
    }
}
