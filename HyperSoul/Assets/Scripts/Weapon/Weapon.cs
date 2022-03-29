using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviourPun, IPunObservable
{
    public int _curBulletCnt = 0;
    public int _maxBulletAmt = 0;
    protected float _reloadTime = 0;

    protected EGunState _gunState;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_curBulletCnt);
            stream.SendNext(_gunState);
        }
        else
        {
            _curBulletCnt = (int)stream.ReceiveNext();
            _gunState = (EGunState)stream.ReceiveNext();
        }
    }


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
        // ����� derivied class���� ����
        _gunState = EGunState.Reloading;

        yield return new WaitForSeconds(_reloadTime);

        _curBulletCnt = _maxBulletAmt;

        _gunState = EGunState.Ready;
    }

   
}
