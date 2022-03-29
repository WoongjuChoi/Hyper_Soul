using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _monster = null;

    [SerializeField]
    private float _spawnTime = 0f;

    private bool _isSpawned = false;

    private float _elapsedTime = 0f;
    private float _initializeDirection = 0f;

    public float InitializeDirection { get { return _initializeDirection; } }

    private void Start()
    {
        _elapsedTime = _spawnTime;

        _initializeDirection = 145f;
    }

    private void Update()
    {
        if (_monster.GetComponent<MonsterInfomations>().IsDie)
        {
            _isSpawned = false;
        }

        if (false == _isSpawned)
        {
            if (_elapsedTime >= _spawnTime)
            {
                _monster.SetActive(true);

                _isSpawned = true;

                return;
            }

            _elapsedTime += Time.deltaTime;
        }
        else
        {
            _elapsedTime = 0f;
        }
    }
}
