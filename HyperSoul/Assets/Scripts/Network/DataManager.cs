using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    public int PlayerIndex { get; set; }
    public EPlayerType PlayerType { get; set; }

    const string PlayerDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:H";
    const string MonsterDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:E&gid=1983254392";

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
            IsDataReady = true;
        }
        else
        {
            print("웹 응답 없음");
        }

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
                    _playerDataDictionary.Add(playerData.PlayerName, playerData);
                    break;
                case CharacterType.Monster:
                    MonsterData monsterData = new MonsterData();
                    monsterData.MonsterName = column[0] + (string)column[1];
                    monsterData.MaxHp = int.Parse(column[2]);
                    monsterData.Attack = int.Parse(column[3]);
                    monsterData.Exp = int.Parse(column[4]);
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
            GameObject _dataManager = GameObject.Find("DataManager");
            if (_dataManager == null)
            {
                _dataManager = new GameObject { name = "DataManager" };
                _dataManager.AddComponent<DataManager>();
            }
            DontDestroyOnLoad(_dataManager);
            _instance = _dataManager.GetComponent<DataManager>();
        }
    }
}

