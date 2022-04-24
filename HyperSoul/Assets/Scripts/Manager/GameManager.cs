using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { Init(); return _instance; }
    }

    [SerializeField]
    private GameObject _loadingPanel;

    private List<PlayerInfo> _playerInfoList;

    private ChatManager _chatManager;
    private Coroutine _playerRespawn;
    private Coroutine _monsterRespawn;
    private GameObject _player;
    private GameObject _spawnPosBase;
    private GameObject _timeManager;
    private GameObject _monsterSpawnManager;

    private Transform[] _spawnPoint;

    private bool[] _isSpawned = new bool[5];

    public static ChatManager Chat { get { return Instance._chatManager; } }
    public Transform PlayerCamRotationTransform { get; set; }
    public bool IsStart { get; set; }
    public bool IsReady { get; set; }
    public bool IsGameOver { get; set; }

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Init();
        IsReady = true;
        IsGameOver = false;

        if (PhotonNetwork.IsMasterClient)
        {
            _monsterSpawnManager = GameObject.Find("Monster Spawn Manager");
        }    

        _chatManager = GameObject.Find("ChatManager").gameObject.GetComponent<ChatManager>();
    }

    private IEnumerator Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "loadScene", true } });
        yield return Loading();

        if (true == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.All);
        }
    }

    private IEnumerator Loading()
    {
        while (false == AllPlayerCheck("loadScene"))
        {
            yield return null;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _timeManager = PhotonNetwork.InstantiateRoomObject("TimeManager", Vector3.zero, Quaternion.identity);
            _timeManager.GetComponent<TimeManager>().photonView.RPC("ObjectActive", RpcTarget.All, false);
        }

        SpawnPlayer();

        while (false == AllPlayerCheck("loadPlayer"))
        {
            yield return null;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _monsterSpawnManager.GetComponent<MonsterSpawnManager>().SetMonsterPosition();
        }
    }

    [PunRPC]
    private void StartGame()
    {
        IsStart = true;
        _loadingPanel.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            _timeManager.GetComponent<TimeManager>().photonView.RPC("ObjectActive", RpcTarget.All, true);
        }
    }

    public bool AllPlayerCheck(string key)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            if (null == PhotonNetwork.PlayerList[i].CustomProperties[key])
            {
                return false;
            }
        }
        return true;
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void SpawnPlayer()
    {
        _spawnPosBase = GameObject.Find("SpawnPosition");
        _spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
        int index = DataManager.Instance.MyPlayerOrderIndex + 1;

        switch (DataManager.Instance.PlayerType)
        {
            case EPlayerType.Rifle:
                _player = PhotonNetwork.Instantiate("RiflePlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Bazooka:
                _player = PhotonNetwork.Instantiate("BazookaPlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Sniper:
                _player = PhotonNetwork.Instantiate("SniperPlayer", _spawnPoint[index].position, Quaternion.identity);
                break;
        }
    }

    public void RespawnMonster(GameObject monster)
    {
        MonsterInformation monsterInfo = monster.GetComponent<MonsterInformation>();

        _monsterRespawn = StartCoroutine(CoroutineRespawnMonster(monster, monsterInfo));
    }

    public void StopRespawnMonster()
    {
        if (null != _monsterRespawn)
        {
            StopCoroutine(_monsterRespawn);
        }
    }

    private IEnumerator CoroutineRespawnMonster(GameObject monster, MonsterInformation monsterInfo)
    {
        yield return new WaitForSeconds(1.5f);

        monster.transform.rotation = Quaternion.Euler(0f, monsterInfo.MonsterSpawnDirection, 0f);

        monster.transform.position = monsterInfo.InitializeTransform.position;

        // MonsterInformation.cs ¿« MonsterActive() Ω««‡
        monsterInfo.GetComponent< PhotonView>().RPC("MonsterActive", RpcTarget.All, false);

        yield return new WaitForSeconds(3.5f);

        monsterInfo.GetComponent< PhotonView>().RPC("MonsterActive", RpcTarget.All, true);
    }

    public void RespawnPlayer()
    {
        _playerRespawn = StartCoroutine(CoroutineRespawnPlayer());
    }

    public void StopRespawnPlayer()
    {
        if (null != _playerRespawn)
        {
            StopCoroutine(_playerRespawn);
        }
    }

    private IEnumerator CoroutineRespawnPlayer()
    {
        yield return new WaitForSeconds(1.5f);

        int index = Random.Range(1, 5);

        while (_isSpawned[index])
        {
            index = Random.Range(1, 5);
        }

        _player.GetComponent<PlayerInfo>().GetComponent<PhotonView>().RPC("PlayerActive", RpcTarget.All, false);

        yield return new WaitForSeconds(2.5f);

        _player.GetComponent<PlayerInfo>().GetComponent<PhotonView>().RPC("PlayerActive", RpcTarget.All, true);

        _player.transform.position = _spawnPoint[index].position;
    }

    // ΩÃ±€≈Ê
    private static void Init()
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