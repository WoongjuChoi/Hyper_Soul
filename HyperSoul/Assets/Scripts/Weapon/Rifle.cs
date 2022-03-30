using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private void OnEnable()
    {
        _curBulletCnt = 20;
        _maxBulletAmt = 20;
        _reloadTime = 2f;
        _gunState = EGunState.Ready;
    }
   
    public override void Fire()
    {
        if (_curBulletCnt > 0 && _canFire == true)
        {
            StartCoroutine(Shoot());
        }
    }
    public override void Zoom()
    {
        _zoomCam.SetActive(true);
        _playerCam._rotationSpeedX = _zoomRotationSpeed.x;
        _playerCam._rotationSpeedY = _zoomRotationSpeed.y;
    }
    private IEnumerator Shoot()
    {
        --_curBulletCnt;
        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        Instantiate(_bulletPrefab, _bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;

        yield return new WaitForSeconds(0.1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
    }
}
