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


    DataManager _dataManager;

    TimeManager _timeManager;


    public static DataManager DataManager { get { return Instance._dataManager; } }

    public static TimeManager TimeManager { get { return Instance._timeManager; } }

    public Transform PlayerCamRotationTransform { get; set; }

    //[SerializeField]
    //private InputField _chatMsg;
    //[SerializeField]
    //private Text[] _chatList;

    private bool[] _isSpawned = new bool[5];
    private GameObject _player;
    private GameObject _spawnPosBase;

    Transform[] _spawnPoint;
    private bool _isMainScene;

    private bool _isRespawn = false;

    private void Awake()
    {
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
    }

    void Start()
    {
        Init();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        PhotonNetwork.IsMessageQueueRunning = true;
        _isMainScene = false;
    }

    void Update()
    {
        if ("MainScene" == SceneManager.GetActiveScene().name)
        {
            if (false == _isMainScene)
            {
                _isMainScene = true;
                _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
                SpawnPlayer();
            }
            else if (_player.GetComponent<LivingEntity>().IsDead && false == _isRespawn)
            {
                _isRespawn = true;

                Invoke(nameof(RespawnPlayer), 5f);
            }
        }
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }


    public void GameStartButton()
    {
        photonView.RPC("StartMainScene", RpcTarget.All);


    }

    private void SpawnPlayer()
    {
        _spawnPosBase = GameObject.Find("SpawnPosition");
        _spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
        int index = _dataManager.PlayerIndex + 1;

        switch (_dataManager.PlayerType)
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

        _isRespawn = false;

        photonView.RPC(nameof(Respawn), RpcTarget.Others);
    }

    [PunRPC]
    public void Respawn()
    {
        _player.SetActive(true);
    }

    [PunRPC]
    private void StartMainScene()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }

    //private void SendChatMessage()
    //{
    //    string msg = $"[{PhotonNetwork.LocalPlayer.NickName}] \n {_chatMsg.text}";
    //    photonView.RPC("Chat", RpcTarget.OthersBuffered, msg);
    //    Chat(msg);
    //}
    //public void SendDieMessage(LivingEntity attacker, LivingEntity victim)
    //{
    //    string msg = "";
    //    PlayerInfo attackerInfo = attacker.GetComponent<PlayerInfo>();
    //    PlayerInfo victimPlayerInfo = victim.GetComponent<PlayerInfo>();
    //    // MonsterInfo victimMonsterInfo =  victim.GetComponent<MonsterInfo>(); 만들기
    //    if (victimPlayerInfo != null)
    //    {
    //        msg = $"{attackerInfo.NickName}이(가) {victimPlayerInfo.NickName}을(를) 처치";
    //    }
    //    //else if(victimMonsterInfo != null)
    //    //{
    //    //    msg = $"{attackerInfo.NickName}이(가) {victimMonsterInfo.NickName}을(를) 처치";
    //    //}

    //    photonView.RPC("Chat", RpcTarget.OthersBuffered, msg);
    //    Chat(msg);
    //}

    //[PunRPC]
    //private void Chat(string msg)
    //{
    //    bool _input = false;

    //    for (int i = 0; i < _chatList.Length; ++i)
    //    {
    //        if (_chatList[i].text == "")
    //        {
    //            if (i != 0)
    //            {
    //                for (int j = i; j >= 1; --j)
    //                {
    //                    _chatList[j].text = _chatList[j - 1].text;
    //                }
    //            }
    //            _input = true;
    //            _chatList[0].text = msg;
    //            break;
    //        }
    //    }
    //    if (_input == false)
    //    {
    //        for (int i = _chatList.Length - 1; i >= 1; --i)
    //        {
    //            _chatList[i].text = _chatList[i - 1].text;
    //        }
    //        _chatList[0].text = msg;
    //    }
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(_isSpawned);
    //    }
    //    else
    //    {
    //        _isSpawned = (bool[])stream.ReceiveNext();
    //    }
    //}
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