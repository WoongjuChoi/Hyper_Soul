using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : MonoBehaviourPun
{
    [SerializeField]
    private GameObject _riflePlayer;
    [SerializeField]
    private GameObject _bazookaPlayer;
    [SerializeField]
    private GameObject _sniperPlayer;
    [SerializeField]
    private GameObject _readyButton;

    public Text PlayerName;

    private List<GameObject> _playerCharactor = new List<GameObject>();
    private Text _readyButtonText;
    private Image _readyButtonImage;
    private event Action<int, string> _charactorSelectFunc;
    private event Action<int> _readyFunc;

    public int CurPlayerType { get; private set; }
    public int Index { get; set; }
    public bool IsReady { get; private set; }

    private const int PLAYERCHARACTOR_COUNT = 3;

    private void Awake()
    {
        _playerCharactor.Add(_riflePlayer);
        _playerCharactor.Add(_bazookaPlayer);
        _playerCharactor.Add(_sniperPlayer);

        _playerCharactor[CurPlayerType].SetActive(true);

        _readyButtonText = _readyButton.GetComponentInChildren<Text>();
        _readyButtonImage = _readyButton.GetComponent<Image>();
    }

    private void OnEnable()
    {
        CurPlayerType = (int)EPlayerType.Rifle;

        for (int i = 1; i < _playerCharactor.Count; ++i)
        {
            _playerCharactor[i].SetActive(false);
        }

        IsReady = false;
    }

    public void RightClick()
    {
        if (PlayerName.text != PhotonNetwork.LocalPlayer.NickName)
        {
            return;
        }

        _charactorSelectFunc(Index, "Right");
    }

    public void Right()
    {
        _playerCharactor[CurPlayerType].SetActive(false);
        ++CurPlayerType;

        if (CurPlayerType > PLAYERCHARACTOR_COUNT - 1)
        {
            CurPlayerType = 0;
        }

        _playerCharactor[CurPlayerType].SetActive(true);
    }

    public void LeftClick()
    {
        if (PlayerName.text != PhotonNetwork.LocalPlayer.NickName)
        {
            return;
        }

        _charactorSelectFunc(Index, "Left");
    }

    public void Left()
    {
        _playerCharactor[CurPlayerType].SetActive(false);
        --CurPlayerType;

        if (CurPlayerType < 0)
        {
            CurPlayerType = PLAYERCHARACTOR_COUNT - 1;
        }

        _playerCharactor[CurPlayerType].SetActive(true);
    }

    public void ReadyClick()
    {
        if (PlayerName.text != PhotonNetwork.LocalPlayer.NickName)
        {
            return;
        }

        _readyFunc(Index);
    }

    public void SetReadyState()
    {
        IsReady = !IsReady;

        if (IsReady)
        {
            _readyButtonImage.color = new Color(80, 255, 0, 255);
            _readyButtonText.color = new Color(80, 255, 0, 255);
        }
        else
        {
            _readyButtonImage.color = new Color(255, 255, 255, 255);
            _readyButtonText.color = new Color(255, 255, 255, 255);
        }
    }

    public void SetCharactorSelectFunc(Action<int, string> func)
    {
        _charactorSelectFunc = func;
    }

    public void SetReadyButtonFunc(Action<int> func)
    {
        _readyFunc = func;
    }
}
