using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //[Header("DisconnectPanel")]
    //public GameObject DisconnectPanel;
    //[Header("RoomPanel")]
    //public GameObject RoomPanel;
    public InputField NicknameInput;

    [SerializeField]
    private Text _connetInfoText;
    [SerializeField]
    private GameObject _baseSpawnPos;

    private const string GAME_VERSION = "0.01";

    private bool[] _isSpawned = new bool[4];

    PhotonView _photonView;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.GameVersion = GAME_VERSION;
        _photonView = photonView;
        Connect();
    }

    public void Connect()
    {
        //PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();
        _connetInfoText.text = "Connecting to Master Server";

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
        _connetInfoText.text = "Online";
    }
    //void ShowPanel(GameObject CurPanel)
    //{
    //    DisconnectPanel.SetActive(false);
    //    RoomPanel.SetActive(false);
    //    CurPanel.SetActive(true);
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
        _connetInfoText.text = "Offline : Disconnected to Master Server\n Reconnecting...";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log($"Disconnect! Cause : {cause}");
    }
    public override void OnJoinedRoom()
    {
        //ShowPanel(RoomPanel);
        SpawnPlayer();     
    }

    PlayerNetwork FindPlayer()
    {
        foreach(GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(Player.GetPhotonView().IsMine)
            {
                return Player.GetComponent<PlayerNetwork>(); 
            }
        }
        return null;
    }

    public void Click()
    {
        PlayerNetwork Player = FindPlayer();
    }

    private void SpawnPlayer()
    {
        Transform[] spawnPoint = _baseSpawnPos.GetComponentsInChildren<Transform>();
        int index = Random.Range(0, 4);
        while(_isSpawned[index])
        {
            index = Random.Range(0, 4);
        }
        PhotonNetwork.Instantiate("Player", spawnPoint[index].position, Quaternion.identity);

        _photonView.RPC("SpawnPosCheck", RpcTarget.All, index);
    }

    private void SpawnPosCheck(int spawnPosNum)
    {
        _isSpawned[spawnPosNum] = true;

        // µð¹ö±ë¿ë
        for(int i = 0; i < _isSpawned.Length; ++i)
        {
            Debug.Log($"{i} : {_isSpawned[i]}");
        }
    }
}
