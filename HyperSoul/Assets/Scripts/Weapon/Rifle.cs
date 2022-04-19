using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;


    private Coroutine _shootCotountine;
    private void Start()
    {
        if (photonView.IsMine)
        {
            _objectPool.Init("Bullet", 30);
        }
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            //Debug.Log($"_playerInfo.Level : {_playerInfo.Level}");

            MaxBulletAmt = DataManager.Instance.FindPlayerData("Rifle" + _playerInfo.Level.ToString()).MaxBullet;

            CurBulletCnt = MaxBulletAmt;
        }
        _reloadTime = 2;
        _gunState = EGunState.Ready;
    }

    public override void Fire()
    {
        if (false == photonView.IsMine || _gunState != EGunState.Ready || _canFire == false)
        {
            return;
        }

        if (CurBulletCnt > 0)
        {
            photonView.RPC(nameof(BulletFire), RpcTarget.All);
        }

    }
    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }

    [PunRPC]
    public void BulletFire()
    {
        if (null != _shootCotountine)
        {
            StopCoroutine(_shootCotountine);
        }
        if (false == photonView.IsMine)
        {
            return;
        }
        SetMousePos();
        _shootCotountine = StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        --CurBulletCnt;

        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        GameObject bullet = _objectPool.GetObj("Bullet");
        bullet.transform.position = _bulletSpawnPos.position;
        bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        bullet.GetComponent<Bullet>().ReceiveReturnProjectileFunc(ReturnProjectile);
        bullet.GetComponent<Bullet>().ProjectileOwnerID = _playerInfo.PhotonViewID;
        bullet.GetComponent<Bullet>().Attack = _playerInfo.Attack;
        bullet.GetComponent<PoolObject>().photonView.RPC("SetActiveObj", RpcTarget.All, true);
        if(PhotonNetwork.IsMasterClient)
        {
            CreateCollider(bullet.GetComponent<PhotonView>().ViewID);
        }

        if (false == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(CreateCollider), RpcTarget.MasterClient, bullet.GetComponent<PhotonView>().ViewID);
        }


        _canFire = false;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, true);

        _audioSource.clip = ShotSound;
        _audioSource.Play();

        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        MuzzleFlashEffect.SetActive(false);
        _shootCotountine = null;
    }


}
