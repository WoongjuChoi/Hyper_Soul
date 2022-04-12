using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[System.Obsolete("이것은 임시스크립트로 더 이상 사용되지 않음")]
public class NetworkManager : MonoBehaviourPunCallbacks
{
 
    [SerializeField]
    private GameObject _baseSpawnPos;

    

    private bool[] _isSpawned = new bool[4];

    PhotonView _photonView;

    private void Awake()
    {
        
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        _photonView = photonView;
        Connect();
    }

    public void Connect()
    {
        //PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }
    //void ShowPanel(GameObject CurPanel)
    //{
    //    DisconnectPanel.SetActive(false);
    //    RoomPanel.SetActive(false);
    //    CurPanel.SetActive(true);
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
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

        // 디버깅용
        for(int i = 0; i < _isSpawned.Length; ++i)
        {
            Debug.Log($"{i} : {_isSpawned[i]}");
        }
    }
}
