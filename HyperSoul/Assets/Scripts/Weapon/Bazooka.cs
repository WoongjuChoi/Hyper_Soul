using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Bazooka : Weapon
{
    [SerializeField]
    Transform _missileSpawnPos;

    [SerializeField]
    GameObject _missilePrefab;
   
    [SerializeField]
    GameObject _aimAngleRef;

    [SerializeField]
    private float _rayDist = 200f;

    private void OnEnable()
    {
        CurBulletCnt = 1;
        MaxBulletAmt = 1;
        _reloadTime = 2;
        _reloadSpeed = 0.01f;
        _gunState = EGunState.Ready;
    }

    [PunRPC]
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
        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;
        GameObject _bazookaMissile = Instantiate(_missilePrefab, _missileSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        _bazookaMissile.GetComponent<BazookaMissile>().Target = AimTarget()?.transform;
        _bazookaMissile.GetComponent<BazookaMissile>().MisilleOwner = this.gameObject;
        _bazookaMissile.transform.TransformDirection(_aimAngleRef.transform.forward);
        _bazookaMissile.GetComponent<Rigidbody>().velocity = new Vector3(0, _aimAngleRef.transform.localPosition.y * 3f, _aimAngleRef.transform.localPosition.z * 10f);

        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;

        _audioSource.clip = ShotSound;
        _audioSource.Play();
        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        MuzzleFlashEffect.SetActive(false);
    }

    private GameObject AimTarget()
    {
        RaycastHit target;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out target, _rayDist))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            if(target.transform.gameObject.layer == 3 || target.transform.gameObject.layer == 6)
            {
                Debug.Log($"Target is {target.transform.gameObject.layer}");
                return target.transform.gameObject;
            }
        }
        return null;
    }
}
