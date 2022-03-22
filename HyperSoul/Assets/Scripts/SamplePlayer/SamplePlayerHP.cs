using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerHP : MonoBehaviour
{
    [SerializeField]
    private float _playerHP = 0f;

    [SerializeField]
    private float _damagedValue = 0f;

    private bool _isDamaged = false;
    private bool _isDie = false;

    public bool IsDamaged { get { return _isDamaged; } }
    public bool IsDie { get { return _isDie; } }

    private void Update()
    {
        if (_playerHP <= 0)
        {
            _isDie = true;
            return;
        }

        float bufferSamplePlayerHP = _playerHP;

        if (Input.GetKeyDown(KeyCode.X))
        {
            _playerHP -= _damagedValue;
        }

        if (bufferSamplePlayerHP != _playerHP)
        {
            _isDamaged = true;
        }
        else
        {
            _isDamaged = false;
        }
    }
}
