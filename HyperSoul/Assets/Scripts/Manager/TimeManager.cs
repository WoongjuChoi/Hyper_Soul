using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private Text _GameOverText = null;

    [SerializeField]
    private Text _startGameText = null;

    [SerializeField]
    private Text _startTimeText = null;

    [SerializeField]
    private Text _inGameTimeText = null;

    [SerializeField]
    private float _maxReadyTime = 0f;

    [SerializeField]
    private int _maxInGameTime = 0;

    private float _startTime = 0f;
    private float _currTime = 0f;

    private int _minutesNum = 0;
    private int _secondsNum = 0;

    private int _timerText = 0;

    private bool _isReady = true;

    public bool StartGame { get; private set; }

    private void Start()
    {
        ResetTimer();

        _isReady = true;
        StartGame = false;

        _minutesNum = _maxInGameTime / 60;
        _secondsNum = _maxInGameTime % 60;

        _startGameText.text = "START GAME";
    }

    private void Update()
    {
        CheckReadyTimer();

        CheckInGameTimer();

        ShowGameOverText();
    }

    private void CheckInGameTimer()
    {
        if (false == StartGame)
        {
            return;
        }

        _inGameTimeText.text = $"{_minutesNum} : {_secondsNum}";

        if (PhotonNetwork.IsMasterClient)
        {
            _currTime = Time.time - _startTime;

            if (_currTime >= 1f)
            {
                --_secondsNum;

                ResetTimer();
            }

            if (_secondsNum < 0)
            {
                _secondsNum = 59;

                --_minutesNum;
            }

            if (_minutesNum < 0)
            {
                StartGame = false;
                //photonView.RPC("SetStartGame", RpcTarget.All, false);
            }
        }
    }

    private void CheckReadyTimer()
    {
        if (false == _isReady)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _currTime = Time.time - _startTime;

            _timerText = (int)(_maxReadyTime - _currTime);
        }

        _startTimeText.text = $"{_timerText}";

        if (0 == _timerText)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _isReady = false;
                //photonView.RPC("SetIsReady", RpcTarget.All, false);
            }

            _startTimeText.gameObject.SetActive(false);

            ShowStartGameText();

            ResetTimer();

            if (PhotonNetwork.IsMasterClient)
            {
                StartGame = true;
                //photonView.RPC("SetStartGame", RpcTarget.All, true);
            }

            _inGameTimeText.gameObject.SetActive(true);
        }
    }

    private void ResetTimer()
    {
        _startTime = Time.time;
        _currTime = Time.time;
    }

    [PunRPC]
    private void SetIsReady(bool isReady)
    {
        _isReady = isReady;
    }

    [PunRPC]
    private void SetStartGame(bool startGame)
    {
        StartGame = startGame;
    }

    private void ShowGameOverText()
    {
        if (_isReady || StartGame)
        {
            return;
        }

        _GameOverText.gameObject.SetActive(true);
    }

    private void ShowStartGameText()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(0.3f);

        _startGameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        _startGameText.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient && stream.IsWriting)
        {
            stream.SendNext(_timerText);
            stream.SendNext(_minutesNum);
            stream.SendNext(_minutesNum);
        }
        else if (stream.IsReading)
        {
            _timerText = (int)stream.ReceiveNext();
            _minutesNum = (int)stream.ReceiveNext();
            _minutesNum = (int)stream.ReceiveNext();
        }
    }
}
