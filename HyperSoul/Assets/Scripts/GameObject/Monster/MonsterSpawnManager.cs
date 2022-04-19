using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] _monsterSpawnTransform;

    private string _monsterName;

    public void SetMonsterPosition()
    {
        GameObject monster = null;

        for (int i = 0; i < _monsterSpawnTransform.Length; ++i)
        {
            if (i < 7)
            {
                _monsterName = "Green Treant";
            }
            else if (i <= 7 && i < 12)
            {
                _monsterName = "Blue Treant";
            }
            else if (12 <= i && i < 19)
            {
                _monsterName = "Purple Treant";
            }
            else if (19 <= i && i < 29)
            {
                _monsterName = "Brown Wolf";
            }
            else if (29 <= i && i < 38)
            {
                _monsterName = "Black Wolf";
            }
            else if (38 <= i && i < 47)
            {
                _monsterName = "White Wolf";
            }

            monster = PhotonNetwork.InstantiateRoomObject(_monsterName, _monsterSpawnTransform[i].position, _monsterSpawnTransform[i].rotation);
            monster.GetComponent<MonsterInformation>().InitializeTransform = _monsterSpawnTransform[i];
            monster.GetComponent<MonsterInformation>().MonsterSpawnDirection = _monsterSpawnTransform[i].eulerAngles.y;
        }
    }

    //[SerializeField]
    //private GameObject _monster = null;

    //[SerializeField]
    //private float _initializeDirection = 0f;

    //[SerializeField]
    //private float _spawnTime = 0f;

    //private bool _isSpawned = false;

    //private float _elapsedTime = 0f;

    //public float InitializeDirection { get { return _initializeDirection; } }

    //private void Awake()
    //{
    //    _elapsedTime = _spawnTime;
    //}

    //private void Update()
    //{
    //    if (_monster.GetComponent<LivingEntity>().IsDead)
    //    {
    //        _isSpawned = false;
    //    }

    //    if (false == _isSpawned)
    //    {
    //        if (_elapsedTime >= _spawnTime)
    //        {
    //            _monster.SetActive(true);

    //            _isSpawned = true;

    //            return;
    //        }

    //        _elapsedTime += Time.deltaTime;
    //    }
    //    else
    //    {
    //        _elapsedTime = 0f;
    //    }
    //}
}
