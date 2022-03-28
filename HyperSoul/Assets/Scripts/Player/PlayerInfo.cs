using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float Attack { get; private set; }
    public int Hp { get; set; }

    public bool IsDead { get; set; }

    private void OnEnable()
    {
        Attack = 1f;
        Hp = 5;
        IsDead = false;
    }
}
