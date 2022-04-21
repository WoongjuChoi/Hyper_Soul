using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct OtherPlayerInfos : IComparable
{
    public string playerName;
    public int score;
    public int playerOrderIndex;
    public EPlayerType playerType;

    public int CompareTo(object obj)
    {
        OtherPlayerInfos infos = (OtherPlayerInfos)obj;

        if (score > infos.score)
        {
            return -1;
        }
        else if (score < infos.score)
        {
            return 1;
        }
        else
        {
            if (playerOrderIndex >= infos.playerOrderIndex)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}

public class DataManager : MonoBehaviour
{
    static private DataManager _instance;

    static public DataManager Instance
    {
        get { Init(); return _instance; }
    }


    private Dictionary<string, PlayerData> _playerDataDictionary = new Dictionary<string, PlayerData>();
    private Dictionary<string, MonsterData> _monsterDataDictionary = new Dictionary<string, MonsterData>();

    public bool IsDataReady { get; private set; }

    public event System.Action DataReady = null;
    public int MyPlayerOrderIndex { get; set; }
    public EPlayerType PlayerType { get; set;}
    public OtherPlayerInfos[] PlayerInfos = new OtherPlayerInfos[4];

    const string PlayerDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:J";
    const string MonsterDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:F&gid=1983254392";
    const string ScoreDataURL = "https://script.google.com/macros/s/AKfycbwdWYKDZhGhA_JI5MZOZq9gFT2886BV7ZShN1pWLqoAX59Zxi9Y3Khegeh0KSD9mFq5/exec";

    private void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("DataManager"));
        IsDataReady = false;
    }

    private IEnumerator Start()
    {
        UnityWebRequest playerDB = UnityWebRequest.Get(PlayerDataURL);
        UnityWebRequest monsterDB = UnityWebRequest.Get(MonsterDataURL);

        yield return playerDB.SendWebRequest();
        yield return monsterDB.SendWebRequest();
        
        if (playerDB.isDone && monsterDB.isDone)
        {
            SetData(CharacterType.Player, playerDB.downloadHandler.text);
            SetData(CharacterType.Monster, monsterDB.downloadHandler.text);
            DataReady.Invoke();
            IsDataReady = true;
        }
        else
        {
            print("웹 응답 없음");
        }
    }

    public void PostScore()
    {
        WWWForm scoreForm = new WWWForm();
        for (int i = 0; i < 4; ++i)
        {
            scoreForm.AddField("NickName" + i.ToString(), "NickNameValue");
            scoreForm.AddField("Score" + i.ToString(), "ScoreValue");
            scoreForm.AddField("TopScoreNickName", "TopScoreNickNameValue");
            scoreForm.AddField("TopScore", "TopScoreValue");
        }
        
        StartCoroutine(SendScore(scoreForm));
    }

    private IEnumerator SendScore(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScoreDataURL, form))
        {
            yield return www.SendWebRequest();

            if(false == www.isDone)
            {
                print("스코어웹의 응답이 없음");
            }
        }

        UnityWebRequest scoreDB = UnityWebRequest.Post(ScoreDataURL, "");
        yield return scoreDB.SendWebRequest();
    }

    private void SetData(CharacterType characterType, string tsvFile)
    {
        string[] row = tsvFile.Split('\n');
        for (int i = 0; i < row.Length; ++i)
        {
            string[] column = row[i].Split('\t');
            switch (characterType)
            {
                case CharacterType.Player:
                    PlayerData playerData = new PlayerData();
                    playerData.PlayerName = column[0] + (string)column[1];
                    playerData.MaxHp = int.Parse(column[2]);
                    playerData.Attack = int.Parse(column[3]);
                    playerData.MaxExp = int.Parse(column[4]);
                    playerData.Exp = int.Parse(column[5]);
                    playerData.SkillAttack = int.Parse(column[6]);
                    playerData.MaxBullet = int.Parse(column[7]);
                    playerData.Score = int.Parse(column[8]);
                    playerData.MoveSpeed = int.Parse(column[9]);
                    _playerDataDictionary.Add(playerData.PlayerName, playerData);
                    break;
                case CharacterType.Monster:
                    MonsterData monsterData = new MonsterData();
                    monsterData.MonsterName = column[0] + (string)column[1];
                    monsterData.MaxHp = int.Parse(column[2]);
                    monsterData.Attack = int.Parse(column[3]);
                    monsterData.Exp = int.Parse(column[4]);
                    monsterData.Score = int.Parse(column[5]);
                    _monsterDataDictionary.Add(monsterData.MonsterName, monsterData);
                    break;
                default:
                    break;
            }
        }
    }

    public PlayerData FindPlayerData(string name)
    {
        PlayerData data;
        _playerDataDictionary.TryGetValue(name, out data);
        return data;
    }

    public MonsterData FindMonsterData(string name)
    {
        MonsterData data;
        _monsterDataDictionary.TryGetValue(name, out data);
        return data;
    }

    static private void Init()
    {
        if (_instance == null)
        {
            GameObject dataManager = GameObject.Find("DataManager");
            if (dataManager == null)
            {
                dataManager = new GameObject { name = "DataManager" };
                dataManager.AddComponent<DataManager>();
            }
            DontDestroyOnLoad(dataManager);
            _instance = dataManager.GetComponent<DataManager>();
        }
    }
}

