using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{
    [SerializeField]
    GameObject _shooter;
    [SerializeField]
    GameObject _missile = null;
    [SerializeField]
    Transform _missileSpawnPos = null;
    [SerializeField]
    GameObject _aimAngleRef;
    
    
    private float _rayDist = 200f;

    void Start()
    {
        _curBulletCnt = 100;
        _maxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;
    }                                                                                                                                                                                                                                                                                                                                                                                                                                               

    void Update()
    {
        // 삭제 예정
        if (Input.GetMouseButtonDown(0) && _curBulletCnt > 0 && _gunState == EGunState.Ready)
        {
            Fire();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            base.HasReloaded();
        }
    }

    public override void Fire()
    {
        // 플레이어가 이동하면 탄 회전이 안된채로 나감
        GameObject _bazookaMissile = Instantiate(_missile, _missileSpawnPos.position, _aimAngleRef.transform.rotation);
        _bazookaMissile.GetComponent<BazookaMissile>().Target = AimTarget()?.transform;
        _bazookaMissile.GetComponent<BazookaMissile>().MisilleOwner = this.gameObject;
        _bazookaMissile.transform.rotation = _aimAngleRef.transform.rotation;
        _bazookaMissile.GetComponent<Rigidbody>().velocity = _shooter.transform.position * 10f;

        --_curBulletCnt;        
    }

    public override void Zoom()
    {
        
    }

    private GameObject AimTarget()
    {
        RaycastHit target;
        if(Physics.Raycast(_aimAngleRef.transform.position, _aimAngleRef.transform.forward, out target, _rayDist))
        {
            Debug.DrawRay(_aimAngleRef.transform.position, _aimAngleRef.transform.forward * 100f, Color.red, 1f);
            if(target.transform.gameObject.layer == 3 || target.transform.gameObject.layer == 6)
            {
                Debug.Log($"Target is {target.transform.gameObject.layer}");
                return target.transform.gameObject;
            }
        }
        return null;
    }
}
