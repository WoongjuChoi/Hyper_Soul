using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private bool _isZoom = false;

    private void OnEnable()
    {
        CurBulletCnt = 7;
        MaxBulletAmt = 7;
        _reloadTime = 2;
        _gunState = EGunState.Ready;
        ZoomRotationSpeed = new Vector2(0.05f, 0.05f);
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
        _canFire = false;
        
        _playerAnimator.SetTrigger(PlayerAnimatorID.SINGLESHOT);

        _audioSource.PlayOneShot(ShotSound);
        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        MuzzleFlashEffect.SetActive(false);

        yield return new WaitForSeconds(2.9f);

        _canFire = true;
    }
}
