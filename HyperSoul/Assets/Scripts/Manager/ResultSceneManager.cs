using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform _1stPosition;
    [SerializeField]
    private Transform _2ndPosition;
    [SerializeField]
    private Transform _3rdPosition;
    [SerializeField]
    private Transform _4thPosition;

    [SerializeField]
    private GameObject _riflePlayerPrefab;
    [SerializeField]
    private GameObject _bazookaPlayerPrefab;
    [SerializeField]
    private GameObject _sniperPlayerPrefab;

    private OtherPlayerInfos[] _playerInfos;
    private GameObject player;

    private void Awake()
    {
        ResultSceneInit();
    }

    private void ResultSceneInit()
    {
        int countOfPlayers = PhotonNetwork.CurrentRoom.Players.Count;
        _playerInfos = new OtherPlayerInfos[countOfPlayers];

        for (int i = 0; i < countOfPlayers; ++i) // 플레이어 수 만큼만 정보를 가져와 정렬하기위해 반복문으로 정보를 받아온다
        {
            _playerInfos[i].score = DataManager.Instance.PlayerInfos[i].score;
            _playerInfos[i].playerName = DataManager.Instance.PlayerInfos[i].playerName;
            _playerInfos[i].playerOrderIndex = DataManager.Instance.PlayerInfos[i].playerOrderIndex;
            _playerInfos[i].playerType = DataManager.Instance.PlayerInfos[i].playerType;
        }

        Array.Sort(_playerInfos);

        for (int i = 0; i < countOfPlayers; ++i)
        {
            InstantiatePlayerPrefab(i, _playerInfos[i]);
        }
    }

    private void InstantiatePlayerPrefab(int rank, OtherPlayerInfos info)
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        string AnimationTrigger = "Idle";

        switch(rank)
        {
            case 0:
                spawnPosition = _1stPosition.position;
                AnimationTrigger = "Win";
                break;
            case 1:
                spawnPosition = _2ndPosition.position;
                spawnRotation = Quaternion.Euler(0, 10, 0);
                break;
            case 2:
                spawnPosition = _3rdPosition.position;
                spawnRotation = Quaternion.Euler(0, -10, 0);
                break;
            case 3:
                spawnPosition = _4thPosition.position;
                AnimationTrigger = "Lose";
                break;
        }

        switch(info.playerType)
        {
            case EPlayerType.Rifle:
                player = Instantiate(_riflePlayerPrefab, spawnPosition, spawnRotation);
                break;
            case EPlayerType.Bazooka:
                player = Instantiate(_bazookaPlayerPrefab, spawnPosition, spawnRotation);
                break;
            case EPlayerType.Sniper:
                player = Instantiate(_sniperPlayerPrefab, spawnPosition, spawnRotation);
                break;
        }

        player.GetComponentInChildren<Animator>().SetTrigger(AnimationTrigger);
        player.GetComponentInChildren<Text>().text = info.playerName + "\n" + info.score;
    }
}
