using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private ObjectPool _sniperPool = new ObjectPool();

    private void OnEnable()
    {
        CurBulletCnt = 7;
        MaxBulletAmt = 7;
        _reloadTime = 2;
        _gunState = EGunState.Ready;
        ZoomRotationSpeed = new Vector2(0.05f, 0.05f);

        _sniperPool.Init(_bulletPrefab, 10);
    }

    public override void Fire()
    {
        if (false == photonView.IsMine || _gunState != EGunState.Ready || _canFire == false)
        {
            return;
        }

        if (CurBulletCnt > 0)
        {
            photonView.RPC("SniperFire", RpcTarget.All);
        }
    }
    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }

    [PunRPC]
    public void SniperFire()
    {
        SetMousePos();
        StartCoroutine(Shoot());
    }
    private IEnumerator Shoot()
    {
        --CurBulletCnt;

        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        GameObject bullet = _sniperPool.GetObj();
        bullet.transform.position = _bulletSpawnPos.position;
        bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        bullet.GetComponent<Bullet>().SetBulletReturnFunc(_sniperPool.ReturnObj);
        bullet.GetComponent<Bullet>().ProjectileOwnerID = _playerInfo.PhotonViewID;
        bullet.SetActive(true);

        Collider bulletCollider = bullet.GetComponent<BoxCollider>();
        if (false == PhotonNetwork.IsMasterClient)
        {
            bulletCollider.enabled = false;
        }
        else
        {
            bulletCollider.enabled = true;
        }

        _canFire = false;
        _playerAnimator.SetTrigger(PlayerAnimatorID.SINGLESHOT);

        _audioSource.PlayOneShot(ShotSound);
        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        MuzzleFlashEffect.SetActive(false);
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        _playerAnimator.SetTrigger(PlayerAnimatorID.RELOAD);

        yield return new WaitForSeconds(2.9f);

        _canFire = true;
    }
}
