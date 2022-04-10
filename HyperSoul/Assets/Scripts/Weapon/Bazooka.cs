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

    private Coroutine _shootCotountine;

    private void OnEnable()
    {
        CurBulletCnt = 100;
        MaxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;

        _missilePool.Init(_missilePrefab, 2);

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
        int targetViewID = (target != null) ? target.GetComponent<PhotonView>().ViewID : -1;

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
        if(null != _shootCotountine)
        {
            StopCoroutine(_shootCotountine);
        }
        _shootCotountine = StartCoroutine(Shoot(aimDir, targetViewID));
    }


    private IEnumerator Shoot(Vector3 aimDir, int aimedTargetID)
    {
        --CurBulletCnt;

        BazookaMissile _bazookaMissile = _missilePool.GetObj().GetComponent<BazookaMissile>();

        _bazookaMissile.MissilePrefab.SetActive(true);
        _bazookaMissile.RocketParticleEffect.SetActive(false);
        _bazookaMissile.ExplosionEffect.SetActive(false);

        if(aimedTargetID == -1)
        {
            _bazookaMissile.Target = null;
        }
        else
        {
            _bazookaMissile.Target = PhotonView.Find(aimedTargetID).gameObject.transform;
        }

        _bazookaMissile.transform.position = _missileSpawnPos.position;
        _bazookaMissile.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        _bazookaMissile.GetComponent<Rigidbody>().velocity = _bazookaMissile.transform.forward * 7f + _bazookaMissile.transform.up * 7f;
        _bazookaMissile.ReceiveReturnMissileFunc(ReturnMissile);
        _bazookaMissile.ProjectileOwnerID = _playerInfo.PhotonViewID;
        _bazookaMissile.Attack = _playerInfo.Attack;
        Debug.DrawRay(_bazookaMissile.transform.position, aimDir, Color.red, 1f);
        _bazookaMissile.gameObject.SetActive(true);

        if(false == PhotonNetwork.IsMasterClient)
        {
            Collider[] bazookaColliders = _bazookaMissile.GetComponentsInChildren<Collider>();
            foreach(Collider col in bazookaColliders)
            {
                col.enabled = false;
            }
        }    

        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;

        yield return new WaitForSeconds(1f);
        _shootCotountine = null;
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
