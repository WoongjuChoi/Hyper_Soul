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

    private Vector3 _targetPos;

    private void OnEnable()
    {
        CurBulletCnt = 100;
        MaxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;

        _missilePool.Init(_missilePrefab, 20);
    }

    public override void Fire()
    {
        // 발사 가능한지 여부 체크 후, 가능하다면 RayCast후 맞는 처리 실시
        if (false == photonView.IsMine || _gunState != EGunState.Ready || _canFire == false)
        {
            return;
        }

        SetMousePos();
        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;
        GameObject target = AimTarget();
        int targetViewID = (target != null) ? target.GetComponent<PhotonView>().ViewID : 0;

        photonView.RPC("MissileFire", RpcTarget.All, aimDir, targetViewID);
    }


    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }

    [PunRPC]
    public void MissileFire(Vector3 aimDir, int targetViewID)
    {
        StartCoroutine(Shoot(aimDir, targetViewID));
    }


    private IEnumerator Shoot(Vector3 aimDir, int aimedTargetID)
    {
        --CurBulletCnt;


        GameObject _bazookaMissile = _missilePool.GetObj();

        _bazookaMissile.GetComponent<BazookaMissile>().Target = (aimedTargetID != 0) ? PhotonView.Find(aimedTargetID).gameObject.transform : null;
        _bazookaMissile.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        _bazookaMissile.transform.position = _missileSpawnPos.position;
        _bazookaMissile.GetComponent<Rigidbody>().velocity = _bazookaMissile.transform.forward * 7f + _bazookaMissile.transform.up * 7f;
        _bazookaMissile.GetComponent<BazookaMissile>().ReceiveReturnMissileFunc(ReturnMissile);
        _bazookaMissile.GetComponent<BazookaMissile>().ProjectileOwner = _playerInfo;
        _bazookaMissile.GetComponent<BazookaMissile>().Attack = _playerInfo.Attack;
        _bazookaMissile.SetActive(true);

        Collider[] bazookaColliders = _bazookaMissile.GetComponentsInChildren<Collider>();
        if(!PhotonNetwork.IsMasterClient)
        {
            foreach(Collider col in bazookaColliders)
            {
                col.enabled = false;
            }
        }
        else
        {
            foreach (Collider col in bazookaColliders)
            {
                col.enabled = true;
            }
        }

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
            if (target.transform.gameObject.layer == 3 || target.transform.gameObject.layer == 6)
            {
                Debug.Log($"Target is {target.transform.gameObject.layer}");
                return target.transform.gameObject;
            }
        }
        return null;
    }

    private void ReturnMissile(GameObject missile)
    {
        _missilePool.ReturnObj(missile);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurBulletCnt);
            stream.SendNext(_gunState);
        }
        else
        {
            CurBulletCnt = (int)stream.ReceiveNext();
            _gunState = (EGunState)stream.ReceiveNext();

        }
    }
}
