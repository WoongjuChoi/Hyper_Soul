using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _waitingRoomPrefab;

    private GameObject _roomPanal;
    private Text _roomNameText;
    private Dictionary<int, Player> _playerList;
    private Dictionary<int, GameObject> _playerPanal = new Dictionary<int, GameObject>();

    private void Awake()
    {
        _roomPanal = GameObject.Find("RoomPanel");
        _roomNameText = GameObject.Find("RoomNameText").GetComponent<Text>();
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        _playerList = PhotonNetwork.CurrentRoom.Players;
        UpdatePlayerList();
    }

    private void Update()
    {
       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playerList = PhotonNetwork.CurrentRoom.Players;
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int playerPanalKey = 0;

        foreach(KeyValuePair<int, Player> player in _playerList)
        {
            if (player.Value == otherPlayer)
            {
                playerPanalKey = player.Key;
            }
        }

        _playerPanal.Remove(playerPanalKey);
        _playerList = PhotonNetwork.CurrentRoom.Players;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void GameStart()
    {
        photonView.RPC("StartMainScene", RpcTarget.All);
    }

    [PunRPC]

    private void StartMainScene()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }

    private void UpdatePlayerList()
    {
        foreach (var player in _playerList)
        {
            if (false == _playerPanal.ContainsKey(player.Key))
            {
                _playerPanal.Add(player.Key, Instantiate(_waitingRoomPrefab, _roomPanal.transform));
                _playerPanal[player.Key].GetComponent<WaitingRoom>().PlayerName.text = player.Value.NickName;
            }
        }
    }
}
