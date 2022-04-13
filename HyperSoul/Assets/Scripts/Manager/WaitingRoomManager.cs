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
    [SerializeField]
    private WaitingRoom[] _roomList;

    private GameObject _roomPanal;
    private Text _roomNameText;
    private Dictionary<int, Player> _playerList;

    // �� �Ʒ��� Master�� ����ϴ� ������
    private bool[] _emptyRoomCheck = new bool[4];


    private void Awake()
    {
        _roomPanal = GameObject.Find("RoomPanel");
        _roomNameText = GameObject.Find("RoomNameText").GetComponent<Text>();
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        _playerList = PhotonNetwork.CurrentRoom.Players;

        if (PhotonNetwork.IsMasterClient)
        {
            AddPlayer(PhotonNetwork.LocalPlayer.NickName);
        }
    }

    private void Update()
    {
       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playerList = PhotonNetwork.CurrentRoom.Players;

        if (PhotonNetwork.IsMasterClient)
        {
            AddPlayer(newPlayer.NickName);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _playerList = PhotonNetwork.CurrentRoom.Players;

        if (PhotonNetwork.IsMasterClient)
        {
            RemovePlayer(otherPlayer.NickName);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LobbyScene");
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

    private void AddPlayer(string playerName)
    {
        for (int i = 0; i < _roomList.Length; ++i) // �濡 ���� �̸��� �÷��̾ �ִ��� Ȯ��
        {
            if (_roomList[i].PlayerName.text == playerName)
            {
                return;
            }
        }

        for (int i = 0; i < _emptyRoomCheck.Length; ++i) // �� ĭ�� �ִ��� üũ
        {
            if (_emptyRoomCheck[i] == false)
            {
                _emptyRoomCheck[i] = true;
                photonView.RPC("UpdateRoomInfo", RpcTarget.AllBuffered, i, playerName, true);

                return;
            }
        }
    }

    private void RemovePlayer(string playerName)
    {
        for (int i = 0; i < _roomList.Length; ++i)
        {
            if (_roomList[i].PlayerName.text == playerName)
            {
                _emptyRoomCheck[i] = false;

                photonView.RPC("UpdateRoomInfo", RpcTarget.AllBuffered, i, " ", false);
            }
        }
    }

    [PunRPC]
    private void UpdateRoomInfo(int index, string name, bool isActive)
    {
        _roomList[index].gameObject.SetActive(isActive);
        _roomList[index].PlayerName.text = name;
    }
}
