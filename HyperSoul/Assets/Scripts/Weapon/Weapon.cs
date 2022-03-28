using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviourPunCallbacks
{
    public int _curBulletCnt = 0;
    public int _maxBulletAmt = 0;
    protected float _reloadTime = 0;

    protected EGunState _gunState;

    [PunRPC]
    public virtual void Fire() { }
    public virtual void Zoom() { }

    public bool HasReloaded()
    {
        if (_gunState == EGunState.Reloading || _curBulletCnt >= _maxBulletAmt)
        {
            return false;
        }
        Debug.Log(_reloadTime);
        StartCoroutine(Reload());
        return true;
    }
    private IEnumerator Reload()
    {
        // 사운드는 derivied class에서 구현
        _gunState = EGunState.Reloading;

        yield return new WaitForSeconds(_reloadTime);

        _curBulletCnt = _maxBulletAmt;

        _gunState = EGunState.Ready;
    }
}
