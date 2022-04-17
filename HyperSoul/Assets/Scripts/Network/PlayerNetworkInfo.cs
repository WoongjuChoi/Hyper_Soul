using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serialization<T>
{
    public List<T> List;
    public Serialization(List<T> list)
    {
        List = list;
    }
}

[System.Serializable]
public class PlayerNetworkInfo
{
    public PlayerNetworkInfo()
    {

    }
}
