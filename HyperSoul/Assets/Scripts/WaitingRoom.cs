using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : MonoBehaviour
{
    public int CurPlayerType { get; private set; }
    public bool IsReady { get; private set; }
    public Text PlayerName;

    private const int PLAYERCHARACTOR_COUNT = 3;

    [SerializeField]
    private GameObject _riflePlayer;
    [SerializeField]
    private GameObject _bazookaPlayer;
    [SerializeField]
    private GameObject _sniperPlayer;
    [SerializeField]
    private GameObject _readyButton;

    private List<GameObject> _playerCharactor = new List<GameObject>();
    private Text _readyButtonText;
    private Image _readyButtonImage;


    private void Awake()
    {
        _playerCharactor.Add(_riflePlayer);
        _playerCharactor.Add(_bazookaPlayer);
        _playerCharactor.Add(_sniperPlayer);

        CurPlayerType = (int)EPlayerType.Rifle;
        _playerCharactor[CurPlayerType].SetActive(true);

        for (int i = 1; i < _playerCharactor.Count; ++i)
        {
            _playerCharactor[i].SetActive(false);
        }

        IsReady = false;

        _readyButtonText = _readyButton.GetComponentInChildren<Text>();
        _readyButtonImage = _readyButton.GetComponent<Image>();
    }

    private void OnEnable()
    {
        PlayerName.text = "Player 1";
    }

    public void RightClick()
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
        IsReady = !IsReady;
        Debug.Log(IsReady);

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
}