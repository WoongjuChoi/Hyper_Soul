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

    private DataManager _dataManager;

    static public DataManager Datamanager
    {
        get { return Instance._dataManager; }
    }

    public Transform PlayerCamRotationTransform { get; set; }

    //[SerializeField]
    //private InputField _chatMsg;
    //[SerializeField]
    //private Text[] _chatList;

    private bool[] _isSpawned = new bool[5];
    private GameObject _player;
    private GameObject _spawnPosBase;

    private bool _isMainScene;


    void Start()
    {
        Init();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        PhotonNetwork.IsMessageQueueRunning = true;
        _isMainScene = false;
    }

    void Update()
    {
        if("MainScene" == SceneManager.GetActiveScene().name && false ==_isMainScene)
        {
            _isMainScene = true;
            SpawnPlayer();
        }
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    //public void Respawn()
    //{
    //    Transform[] spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
    //}

    public void GameStartButton()
    {
        photonView.RPC("StartMainScene", RpcTarget.All);

        
    }

    private void SpawnPlayer()
    {
        _spawnPosBase = GameObject.Find("SpawnPosition");
        Transform[] spawnPoint = _spawnPosBase.GetComponentsInChildren<Transform>();
        int index = _dataManager.PlayerIndex;

        switch (_dataManager.PlayerType)
        {
            case EPlayerType.Rifle:
                _player = PhotonNetwork.Instantiate("RiflePlayer", spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Bazooka:
                _player = PhotonNetwork.Instantiate("BazookaPlayer", spawnPoint[index].position, Quaternion.identity);
                break;
            case EPlayerType.Snipers:
                _player = PhotonNetwork.Instantiate("SniperPlayer", spawnPoint[index].position, Quaternion.identity);
                break;
        }
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
    //    // MonsterInfo victimMonsterInfo =  victim.GetComponent<MonsterInfo>(); �����
    //    if (victimPlayerInfo != null)
    //    {
    //        msg = $"{attackerInfo.NickName}��(��) {victimPlayerInfo.NickName}��(��) óġ";
    //    }
    //    //else if(victimMonsterInfo != null)
    //    //{
    //    //    msg = $"{attackerInfo.NickName}��(��) {victimMonsterInfo.NickName}��(��) óġ";
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