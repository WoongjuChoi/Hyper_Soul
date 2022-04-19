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

    // æ¿ µø±‚»≠ ∫Œ∫–
    [SerializeField]
    private GameObject _loadingPanel;
    private List<PlayerInfo> _playerInfoList;
    public bool IsStart = false;
    public bool IsGameover = false;

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Init();
        _chatManager = GameObject.Find("ChatManager").gameObject.GetComponent<ChatManager>();
    }

    IEnumerator Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "loadScene", true } });
        yield return Loading();

        if (true == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
        }
    }

    IEnumerator Loading()
    {
        while (false == AllPlayerCheck("loadScene"))
        {
            yield return null;
        }

        SpawnPlayer();

        while (false == AllPlayerCheck("loadPlayer"))
        {
            yield return null;
        }
    }

    [PunRPC]
    private void StartGame()
    {
        IsStart = true;
        _loadingPanel.SetActive(false);

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

    public void RespawnPlayer()
    {
        StartCoroutine(CoroutineRespawn());
    }

    private IEnumerator CoroutineRespawn()
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