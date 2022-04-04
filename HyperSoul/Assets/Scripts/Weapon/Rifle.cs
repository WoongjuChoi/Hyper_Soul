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
        CurBulletCnt = 20;
        MaxBulletAmt = 20;
        _reloadTime = 2;
        _gunState = EGunState.Ready;
    }

    public override void Fire()
    {
        if (CurBulletCnt > 0 && _canFire == true)
        {
            StartCoroutine(Shoot());
        }
    }
    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }
    private IEnumerator Shoot()
    {
        --CurBulletCnt;
        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        Instantiate(_bulletPrefab, _bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;
        _audioSource.clip = ShotSound;
        _audioSource.Play();
        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        MuzzleFlashEffect.SetActive(false);
    }
}
