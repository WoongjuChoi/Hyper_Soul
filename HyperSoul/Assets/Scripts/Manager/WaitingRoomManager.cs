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

    // 이 아래는 Master만 사용하는 변수들
    private bool[] _emptyRoomCheck = new bool[4];


    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        _roomPanal = GameObject.Find("RoomPanel");
        _roomNameText = GameObject.Find("RoomNameText").GetComponent<Text>();
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        _playerList = PhotonNetwork.CurrentRoom.Players;

        if (PhotonNetwork.IsMasterClient)
        {
            AddPlayer(PhotonNetwork.LocalPlayer.NickName);
        }

        for (int i = 0; i < _roomList.Length; ++i)
        {
            _roomList[i].SetCharactorSelectFunc(SelectCharactor);
            _roomList[i].SetReadyButtonFunc(SetReadyState);
        }
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
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void GameStart()
    {
        if (false == PhotonNetwork.IsMasterClient)
        {
            return;
        }

        for (int i = 0; i < _playerList.Count; ++i)
        {
            if (_roomList[i].IsReady == false) // 한 명이라도 레디를 안했다면 리턴
            {
                return;
            }
        }

        photonView.RPC(nameof(SendPlayerData), RpcTarget.AllBuffered);

        photonView.RPC(nameof(StartMainScene), RpcTarget.All);
    }

    [PunRPC]
    private void SendPlayerData()
    {
        for (int i = 0; i < _playerList.Count; ++i)
        {
            if (_roomList[i].PlayerName.text == PhotonNetwork.LocalPlayer.NickName) // 본인의 패널인 경우 데이터매니저에 정보 전달
            {
                DataManager.Instance.PlayerOrderIndex = i;
                DataManager.Instance.PlayerType = (EPlayerType)_roomList[i].CurPlayerType;
            }
        }
    }

    private void AddPlayer(string playerName)
    {
        for (int i = 0; i < _roomList.Length; ++i) // 방에 같은 이름의 플레이어가 있는지 확인
        {
            if (_roomList[i].PlayerName.text == playerName)
            {
                return;
            }
        }

        for (int i = 0; i < _emptyRoomCheck.Length; ++i) // 빈 칸이 있는지 체크
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
        _roomList[index].Index = index;
        _roomList[index].gameObject.SetActive(isActive);
        _roomList[index].PlayerName.text = name;
    }

    private void SelectCharactor(int index, string dir)
    {
        switch (dir)
        {
            case "Right":
                photonView.RPC("RightButton", RpcTarget.AllBuffered, index);
                break;
            case "Left":
                photonView.RPC("LeftButton", RpcTarget.AllBuffered, index);
                break;
            default:
                break;
        }
    }

    private void SetReadyState(int index)
    {
        photonView.RPC("ReadyButton", RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    private void RightButton(int index)
    {
        _roomList[index].Right();
    }

    [PunRPC]
    private void LeftButton(int index)
    {
        _roomList[index].Left();
    }

    [PunRPC]
    private void ReadyButton(int index)
    {
        _roomList[index].SetReadyState();
    }

    [PunRPC]
    private void StartMainScene()
    {
        if (true == PhotonNetwork.AutomaticallySyncScene)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel("MainScene");
        }

    }
}
