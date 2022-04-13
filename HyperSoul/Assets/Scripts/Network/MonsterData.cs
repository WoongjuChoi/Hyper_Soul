using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object/MonsterData", order = int.MaxValue)]
public class MonsterData //: ScriptableObject
{
    public string MonsterName
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
    public int Exp
    {
        get; set;
    }
    public int Score
    {
        get; set;
    }
}
