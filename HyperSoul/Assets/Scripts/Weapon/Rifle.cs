using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private ObjectPool _bulletPool = new ObjectPool();

    private void OnEnable()
    {
        CurBulletCnt = 20;
        MaxBulletAmt = 20;
        _reloadTime = 2;
        _gunState = EGunState.Ready;

        _bulletPool.Init(_bulletPrefab, 40);
    }

    public override void Fire()
    {
        if (false == photonView.IsMine || _gunState != EGunState.Ready || _canFire == false)
        {
            return;
        }

        if (CurBulletCnt > 0)
        {
            photonView.RPC("BulletFire", RpcTarget.All);
        }
    }
    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }

    [PunRPC]
    public void BulletFire()
    {
        SetMousePos();
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        --CurBulletCnt;

        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        GameObject bullet = _bulletPool.GetObj();
        bullet.transform.position = _bulletSpawnPos.position;
        bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        bullet.GetComponent<Bullet>().SetBulletReturnFunc(_bulletPool.ReturnObj);
        bullet.GetComponent<Bullet>().ProjectileOwner = _playerInfo;
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
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);

        _audioSource.clip = ShotSound;
        _audioSource.Play();

        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        MuzzleFlashEffect.SetActive(false);
    }
}
