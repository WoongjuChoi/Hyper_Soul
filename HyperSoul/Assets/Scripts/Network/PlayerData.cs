using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    public string PlayerName
    {
        get; set;
    }
    public int MaxHp
    {
        get; set;
    }
    public int Attack
    {
        get; set;
    }
    public int MaxExp
    {
        get; set;
    }
    public int Exp
    {
        get; set;
    }

}
