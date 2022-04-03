using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private string _playerName;
    public string PlayerName
    {
        get { return _playerName; }
    }

    [SerializeField]
    private int _attack;
    public int Attack
    {
        get { return _attack; }
    }

    [SerializeField]
    private int _maxHp;
    public int MaxHp
    {
        get { return _maxHp; }
    }
    [SerializeField]
    private int _exp;
    public int Exp
    {
        get { return _exp; }
    }

}
