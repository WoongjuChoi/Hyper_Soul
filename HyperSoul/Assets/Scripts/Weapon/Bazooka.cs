using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Bazooka : Weapon, IPunObservable
{
    PlayerInfo _bazookaOwner;
    
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
        _bazookaOwner = GetComponentInParent<PlayerInfo>();

        _curBulletCnt = 100;
        _maxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;

        for(int i = 0; i < 20; ++i)
        {
            _missilePool.Init(_missilePrefab);
        }
    }

    public override void Fire()
    {
        // 발사 가능한지 여부 체크 후, 가능하다면 RayCast후 맞는 처리 실시
        if (false == photonView.IsMine || _curBulletCnt <= 0 || false == _canFire)
        {
            return;
        }

        Vector3 pos = Vector3.zero; // 카메라의 위치가 들어가야 함
        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;

        SetMousePos();

        photonView.RPC("MissileFire", RpcTarget.All, null);
    }

    public override void Zoom()
    {
        _zoomCam.SetActive(true);
        _playerCam._rotationSpeedX = _zoomRotationSpeed.x;
        _playerCam._rotationSpeedY = _zoomRotationSpeed.y;
    }
    [PunRPC]
    public void MissileFire()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        GameObject _bazookaMissile = _missilePool.GetObj();
        --_curBulletCnt;

        Vector3 aimDir = (_mousePos - _missileSpawnPos.position).normalized;
        Debug.Log($"{_mousePos.x}, {_mousePos.y}, {_mousePos.x}");
        _bazookaMissile.GetComponent<BazookaMissile>().Target = AimTarget()?.transform;
        _bazookaMissile.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        _bazookaMissile.transform.position = _missileSpawnPos.position;
        _bazookaMissile.GetComponent<Rigidbody>().velocity = _bazookaMissile.transform.forward * 7f + _bazookaMissile.transform.up * 7f;
        _bazookaMissile.GetComponent<BazookaMissile>().ReceiveReturnMissileFunc(ReturnMissile);
        _bazookaMissile.GetComponent<BazookaMissile>().ProjectileOwner = _bazookaOwner;
        _bazookaMissile.SetActive(true);

        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);
        _canFire = false;

        yield return new WaitForSeconds(1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        
    }

    private GameObject AimTarget()
    {
        if(!photonView.IsMine)
        {
            return null;
        }
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

    private void ReturnMissile(GameObject missile)
    {
        _missilePool.ReturnObj(missile);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_curBulletCnt);
            stream.SendNext(_gunState);
        }
        else
        {
            _curBulletCnt = (int)stream.ReceiveNext();
            _gunState = (EGunState)stream.ReceiveNext();

        }
    }

}
