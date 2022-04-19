using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private Text _GameOverText;

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

    private void OnEnable()
    {
        ResetTimer();

        _minutesNum = _maxInGameTime / 60;
        _secondsNum = _maxInGameTime % 60;

        _startTimeText.text = $"{_timerText}";
        _inGameTimeText.text = $"{_minutesNum} : {_secondsNum}";
        _startGameText.text = "START GAME";

        _startTimeText.gameObject.SetActive(true);
        _inGameTimeText.gameObject.SetActive(false);
        _startGameText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void ObjectActive(bool b)
    {
        gameObject.SetActive(b);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        CheckReadyTimer();

        CheckInGameTimer();
    }

    private void CheckInGameTimer()
    {
        if (GameManager.Instance.IsReady)
        {
            return;
        }

        _inGameTimeText.text = $"{_minutesNum} : {_secondsNum}";

        //Debug.Log($"_minutesNum : {_minutesNum}\n" +
        //    $"_secondsNum : {_secondsNum}");

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

            photonView.RPC(nameof(SetGameOver), RpcTarget.AllViaServer, true);
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

        _startTimeText.text = $"{_timerText}";

        //Debug.Log($"_timerText : {_timerText}");

        if (0 == _timerText)
        {
            photonView.RPC(nameof(SetIsReady), RpcTarget.AllViaServer, false);

            _startTimeText.gameObject.SetActive(false);

            ShowStartGameText();

            ResetTimer();

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
        GameManager.Instance.IsReady = isReady;
    }

    [PunRPC]
    private void SetGameOver(bool gameOver)
    {
        GameManager.Instance.IsGameOver = gameOver;

        _GameOverText.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("GoResultScene", 3f);
        }
    }

    private void GoResultScene()
    {
        PhotonNetwork.LoadLevel("ResultScene");
    }

    [PunRPC]
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
