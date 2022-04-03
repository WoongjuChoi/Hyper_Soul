using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, PlayerData> _playerDataDictionary = new Dictionary<string, PlayerData>();
    private Dictionary<string, MonsterData> _monsterDataDictionary = new Dictionary<string, MonsterData>();

    const string PlayerDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:F";
    const string MonsterDataURL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A2:E&gid=1983254392";

    private void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("DataManager"));
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
}

