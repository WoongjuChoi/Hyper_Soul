using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    private string _roomName;
    private int _curPlayerCnt;
    private int _maxPlayer;

    private Text _roomInfo;

    private void Awake()
    {
        _roomInfo = GetComponentInChildren<Text>();
    }

    private void UpdateInfo()
    {
        _roomInfo.text = $"{_roomName}\n {_curPlayerCnt.ToString("0")} / {_maxPlayer.ToString()}";
    }
}
