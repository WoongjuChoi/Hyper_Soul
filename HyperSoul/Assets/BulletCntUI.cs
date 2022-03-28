using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 임시 스크립트(삭제예정)
public class BulletCntUI : MonoBehaviourPun
{
    [SerializeField]
    Text _bullet;
    [SerializeField]
    Text _nickname;
    [SerializeField]
    Bazooka _bazooka;
    [SerializeField]
    NetworkManager _networkManager;

    void Update()
    {
        _bullet.text = $"{_bazooka._curBulletCnt}" + " / " + $"{_bazooka._maxBulletAmt}";
        //_nickname.text = _networkManager.Nickname;
    }
}
