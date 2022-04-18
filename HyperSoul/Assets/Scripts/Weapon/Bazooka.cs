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

    private Vector3 _targetPos;

    private Coroutine _shootCotountine;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _objectPool.Init("BazookaMissile", 3);
        }
    }

    private void OnEnable()
    {
        MaxBulletAmt = DataManager.Instance.FindPlayerData("Bazooka" + _playerInfo.Level.ToString()).MaxBullet;
        CurBulletCnt = MaxBulletAmt;
        _reloadTime = 5;
        _gunState = EGunState.Ready;
    }

    public override void Fire()
    {
        if (false == canFire())
        {
            return;
        }

        SetMousePos();
        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;
        GameObject target = AimTarget();
        int targetViewID = -1;
        if (target != null)
        { 
            targetViewID = target.GetComponent<PhotonView>().ViewID;
        }

        if (CurBulletCnt > 0)
        {
            photonView.RPC(nameof(MissileFire), RpcTarget.All, aimDir, targetViewID);
        }
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
        if (false == photonView.IsMine)
        {
            return;
        }
        _shootCotountine = StartCoroutine(Shoot(aimDir, targetViewID));
    }


    private IEnumerator Shoot(Vector3 aimDir, int aimedTargetID)
    {

        --CurBulletCnt;

        BazookaMissile bazookaMissile = _objectPool.GetObj("BazookaMissile").GetComponent<BazookaMissile>();

        bazookaMissile.MissilePrefab.SetActive(true);
        bazookaMissile.RocketParticleEffect.SetActive(false);
        bazookaMissile.ExplosionEffect.SetActive(false);

        if (aimedTargetID == -1)
        {
            bazookaMissile.Target = null;
        }
        else
        {
            bazookaMissile.Target = PhotonView.Find(aimedTargetID).gameObject.transform;
        }

        bazookaMissile.transform.position = _missileSpawnPos.position;
        bazookaMissile.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        bazookaMissile.GetComponent<Rigidbody>().velocity = bazookaMissile.transform.forward * 7f + bazookaMissile.transform.up * 7f;
        bazookaMissile.ProjectileOwnerID = _playerInfo.PhotonViewID;
        bazookaMissile.Attack = _playerInfo.Attack;
        bazookaMissile.GetComponent<PoolObject>().SetActiveObj(true);
        bazookaMissile.ReceiveReturnProjectileFunc(ReturnProjectile);
        bazookaMissile.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, true);



        if (true == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RemoveCollider), RpcTarget.OthersBuffered, bazookaMissile.GetComponent<PhotonView>().ViewID);
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
            if (target.transform.gameObject.layer == 10 || target.transform.gameObject.layer == 12)
            {
                Debug.Log($"Target is {target.transform.gameObject.layer}");
                return target.transform.gameObject;
            }
        }
        return null;
    }

    
}
