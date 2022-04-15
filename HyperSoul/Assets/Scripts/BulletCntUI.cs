using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Obsolete("테스트씬용 스크립트로 삭제예정")]

public class BulletCntUI : MonoBehaviour
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
        _bullet.text = $"{_bazooka.CurBulletCnt}" + " / " + $"{_bazooka.MaxBulletAmt}";
        //_nickname.text = _networkManager.Nickname;
    }
}
