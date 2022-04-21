using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _startGameText;

    [SerializeField]
    private Text _startTimeText;

    [SerializeField]
    private Text _inGameTimeText;

    [SerializeField]
    private float _maxReadyTime = 0f;

    [SerializeField]
    private int _maxInGameTime = 0;

    private float _startTime = 0f;
    private float _currTime = 0f;

    private int _minutesNum = 0;
    private int _secondsNum = 0;

    private int _timerText = 0;

    private bool _isReadyDone = false;

    private void OnEnable()
    {
        _startGameText.text = "START GAME";

        // HACK : TimeManager가 이미 RPC를 통해 생성돼서 OnEnalble이 전부 들어오는데
        // 굳이 여기서 또 RPC를 통해 실행시켜야하는가 의문을 가져 각 클라이언트에서 실행하기로 했습니다.
        ActivateTexts(true, false, false);

        if (false == PhotonNetwork.IsMasterClient)
        {
            return;
        }

        ResetTimer();
        _minutesNum = _maxInGameTime / 60;
        _secondsNum = _maxInGameTime % 60;

        //photonView.RPC(nameof(TextsActive), RpcTarget.All, true, false, false);
    }

    [PunRPC]
    public void ActivateTexts(bool startTime, bool inGameTime, bool startText)
    {
        _startTimeText.gameObject.SetActive(startTime);
        _inGameTimeText.gameObject.SetActive(inGameTime);
        _startGameText.gameObject.SetActive(startText);
    }

    [PunRPC]
    public void ObjectActive(bool b)
    {
        gameObject.SetActive(b);
    }

    private void Update()
    {
        _startTimeText.text = $"{_timerText}";
        _inGameTimeText.text = $"{_minutesNum} : {_secondsNum}";

        if (GameManager.Instance.IsGameOver || false == PhotonNetwork.IsMasterClient)
        {
            return;
        }

        CheckReadyTimer();

        if (_isReadyDone)
        {
            CheckInGameTimer();
        }

    }

    private void CheckInGameTimer()
    {
        if (GameManager.Instance.IsReady)
        {
            return;
        }

        photonView.RPC(nameof(ActivateTexts), RpcTarget.All, false, true, false);

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
            _minutesNum = 0;
            _secondsNum = 0;

            photonView.RPC(nameof(SetGameOver), RpcTarget.All, true);
        }
    }

    private void CheckReadyTimer()
    {
        if (false == GameManager.Instance.IsReady)
        {
            return;
        }

        _currTime = Time.time - _startTime;
        _timerText = (int)(_maxReadyTime - _currTime);

        if (_timerText <= 0)
        {
            photonView.RPC(nameof(SetIsReady), RpcTarget.All, false);
            photonView.RPC(nameof(ActivateTexts), RpcTarget.All, false, false, true);

            ShowStartGameText();
            ResetTimer();
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
        GameManager.Instance.IsReady = isReady;
    }

    [PunRPC]
    private void SetGameOver(bool gameOver)
    {
        GameManager.Instance.IsGameOver = gameOver;
        GameManager.Instance.StopRespawnPlayer();
        GameManager.Instance.StopRespawnMonster();

        _gameOverText.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("GoResultScene", 3f);
        }
    }

    private void GoResultScene()
    {
        PhotonNetwork.LoadLevel("ResultScene");
    }

    private void ShowStartGameText()
    {
        StartCoroutine(ShowStartText());
    }

    private IEnumerator ShowStartText()
    {
        yield return new WaitForSeconds(1f);
        photonView.RPC(nameof(ActivateTexts), RpcTarget.All, false, false, false);
        _isReadyDone = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient && stream.IsWriting)
        {
            stream.SendNext(_timerText);
            stream.SendNext(_minutesNum);
            stream.SendNext(_secondsNum);
        }
        else if (stream.IsReading)
        {
            _timerText = (int)stream.ReceiveNext();
            _minutesNum = (int)stream.ReceiveNext();
            _secondsNum = (int)stream.ReceiveNext();
        }
    }
}
