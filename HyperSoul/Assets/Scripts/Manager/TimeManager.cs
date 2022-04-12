using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    static public TimeManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<TimeManager>();
            }

            return _instance;
        }
    }

    static private TimeManager _instance = null;

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

    private bool _isReady = true;

    public bool StartGame { get; private set; }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

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
        }
    }

    private void CheckReadyTimer()
    {
        if (false == _isReady)
        {
            return;
        }

        _currTime = Time.time - _startTime;

        int timerText = (int)(_maxReadyTime - _currTime);

        _startTimeText.text = $"{timerText}";

        if (0 == timerText)
        {
            _isReady = false;

            _startTimeText.gameObject.SetActive(false);

            ShowStartGameText();

            ResetTimer();

            StartGame = true;

            _inGameTimeText.gameObject.SetActive(true);
        }
    }

    private void ResetTimer()
    {
        _startTime = Time.time;
        _currTime = Time.time;
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
}
