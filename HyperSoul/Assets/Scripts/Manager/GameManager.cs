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

    private ChatManager _chatManager;
    public static ChatManager Chat { get { return Instance._chatManager; } }
    public Transform PlayerCamRotationTransform { get; set; }

    private bool[] _isSpawned = new bool[5];
    private GameObject _player;
    private GameObject _spawnPosBase;

    Transform[] _spawnPoint;

    // 씬 동기화 부분
    [SerializeField]
    private GameObject _loadingPanel;
    private List<PlayerInfo> _playerInfoList;
    public bool IsStart = false;

    public bool IsReady { get; set; }
    public bool IsGameOver { get; set; }

    private GameObject _timeManager;

    private GameObject _monsterSpawnManager;
    private Coroutine _playerRespawn;
    private Coroutine _monsterRespawn;

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Init();
        IsReady = true;
        IsGameOver = false;

        // MonsterSpawnManager 게임오브젝트 찾기    (22.04.19)
        if (PhotonNetwork.IsMasterClient)
        {
            _monsterSpawnManager = GameObject.Find("Monster Spawn Manager");
        }    

        _chatManager = GameObject.Find("ChatManager").gameObject.GetComponent<ChatManager>();
    }

    IEnumerator Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "loadScene", true } });
        yield return Loading();

        if (true == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.All);
        }
    }

    IEnumerator Loading()
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

        // Monster 위치시키는 함수 실행  (22.04.19)
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

    private bool AllPlayerCheck(string key)
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

        // MonsterInformation.cs 의 MonsterActive() 실행
        monsterInfo.GetComponent< PhotonView>().RPC("MonsterActive", RpcTarget.All, false);

        yield return new WaitForSeconds(1.5f);

        monsterInfo.GetComponent< PhotonView>().RPC("MonsterActive", RpcTarget.All, true);

        monster.transform.position = monsterInfo.InitializeTransform.position;

        monster.transform.rotation = Quaternion.Euler(0f, monsterInfo.MonsterSpawnDirection, 0f);
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

    // 싱글톤
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