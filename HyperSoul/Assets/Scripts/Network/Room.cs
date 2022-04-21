using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    private Text _roomInfo;

    private string _roomName;
    private int _curPlayerCnt;
    private int _maxPlayer;

    public string RoomName
    {
        get { return _roomName; }
        set { _roomName = value; }
    }
    public int CurPlayer
    {
        get { return _curPlayerCnt; }
        set { _curPlayerCnt = value; }
    }
    public int MaxPlayer
    {
        get { return _maxPlayer; }
        set { _maxPlayer = value; }
    }

    private void Awake()
    {
        _roomInfo = GetComponentInChildren<Text>();
    }

    public void UpdateInfo()
    {
        _roomInfo.text = $"{_roomName}\n {_curPlayerCnt.ToString("0")} / {_maxPlayer.ToString()}";
    }
}
