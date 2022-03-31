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

    private ObjectPool _missilePool = new ObjectPool();

    private void OnEnable()
    {
        _curBulletCnt = 100;
        _maxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;

        for(int i = 0; i < 100; ++i)
        {
            _missilePool.Init(_missilePrefab);
        }
    }

    [PunRPC]
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
        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;
        GameObject _bazookaMissile = _missilePool.GetObj();
        _bazookaMissile.SetActive(true);
        _bazookaMissile.transform.position = _missileSpawnPos.position;
        _bazookaMissile.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        _bazookaMissile.GetComponent<BazookaMissile>().Target = AimTarget()?.transform;
        _bazookaMissile.GetComponent<BazookaMissile>().MisilleOwner = this.gameObject;
        _bazookaMissile.transform.TransformDirection(_aimAngleRef.transform.forward);
        _bazookaMissile.GetComponent<Rigidbody>().velocity = new Vector3(0, _aimAngleRef.transform.localPosition.y * 3f, _aimAngleRef.transform.localPosition.z * 10f);

        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;

        yield return new WaitForSeconds(1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
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
