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

    private enum EMonsterColors
    {
        // Treant Color
        Green = 0,
        Blue = 1,
        Purple = 2,
        // Wolf Color
        Black = 3,
        Brown = 4,
        White = 5,
    }

    private const string TREANT = "Treant";
    private const string WOLF = "Wolf";

    public void SetMonsterPosition()
    {
        GameObject monster = null;

        EMonsterColors monsterColor;

        for (int i = 0; i < _monsterSpawnTransform.Length; ++i)
        {
            if (0 == i % 2)
            {
                monsterColor = (EMonsterColors)UnityEngine.Random.Range(0, 3);
                _monsterName = monsterColor.ToString() + TREANT;
            }
            else
            {
                monsterColor = (EMonsterColors)UnityEngine.Random.Range(3, 6);
                _monsterName = monsterColor.ToString() + WOLF;
            }

            monster = PhotonNetwork.InstantiateRoomObject(_monsterName, _monsterSpawnTransform[i].position, _monsterSpawnTransform[i].rotation);
            monster.GetComponent<MonsterInformation>().InitializeTransform = _monsterSpawnTransform[i];
            monster.GetComponent<MonsterInformation>().MonsterSpawnDirection = _monsterSpawnTransform[i].eulerAngles.y;
        }
    }
}
