using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    static private GameManager _instance;

    static public GameManager Instance
    {
        get { Init(); return _instance; }
    }

    public Transform PlayerCamRotationTransform { get; set; }

    private bool[] _isSpawned = new bool[5];
    private GameObject _player;
    private GameObject _spawnPosBase;

    Transform[] _spawnPoint;

    void Start()
    {
        Init();
        PhotonNetwork.IsMessageQueueRunning = true;
        SpawnPlayer();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void SpawnPlayer()
    {
        _spawnPosBase = GameObject.Find("SpawnPosition");
        _spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
        int index = DataManager.Instance.PlayerOrderIndex + 1;

        switch (DataManager.Instance.PlayerType)
        {
            case EPlayerType.Rifle:
                _player = PhotonNetwork.Instantiate("RiflePlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Bazooka:
                _player = PhotonNetwork.Instantiate("BazookaPlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Snipers:
                _player = PhotonNetwork.Instantiate("SniperPlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
        }

    }

    public void RespawnPlayer()
    {
        int index = Random.Range(1, 5);

        while (_isSpawned[index])
        {
            index = Random.Range(1, 5);
        }

        _player.SetActive(true);

        _player.transform.position = _spawnPoint[index].position;

        photonView.RPC(nameof(Respawn), RpcTarget.Others);
    }

    [PunRPC]
    public void Respawn()
    {
        _player.SetActive(true);
    }

   
    static private void Init()
    {
        if (_instance == null)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager == null)
            {
                gameManager = new GameObject { name = "GameManager" };
                gameManager.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(gameManager);
            _instance = gameManager.GetComponent<GameManager>();
        }
    }
}