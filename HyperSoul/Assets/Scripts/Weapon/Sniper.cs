using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private ObjectPool _sniperPool = new ObjectPool();

    private Coroutine _shootCotountine;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _objectPool.Init("Bullet", 7);
        }
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            MaxBulletAmt = DataManager.Instance.FindPlayerData("Sniper" + _playerInfo.Level.ToString()).MaxBullet;
            CurBulletCnt = MaxBulletAmt;
        }
        _reloadTime = 2;
        _gunState = EGunState.Ready;
        ZoomRotationSpeed = new Vector2(0.05f, 0.05f);
    }

    public override void Fire()
    {
        if (false == photonView.IsMine || _gunState != EGunState.Ready || _canFire == false)
        {
            return;
        }

        if (CurBulletCnt > 0)
        {
            photonView.RPC(nameof(SniperFire), RpcTarget.All);
        }
    }
    public override void Zoom()
    {
        ZoomCam.SetActive(true);
        _playerCam._rotationSpeedX = ZoomRotationSpeed.x;
        _playerCam._rotationSpeedY = ZoomRotationSpeed.y;
    }

    [PunRPC]
    public void SniperFire()
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

        if (true == PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RemoveCollider), RpcTarget.OthersBuffered, bullet.GetComponent<PhotonView>().ViewID);
        }

        _canFire = false;
        _playerAnimator.SetTrigger(PlayerAnimatorID.SINGLESHOT);

        _audioSource.PlayOneShot(ShotSound);
        MuzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        MuzzleFlashEffect.SetActive(false);
        _playerAnimator.SetBool(PlayerAnimatorID.ISSHOOT, false);
        _playerAnimator.SetTrigger(PlayerAnimatorID.RELOAD);

        yield return new WaitForSeconds(2.9f);

        _canFire = true;
        _shootCotountine = null;
    }


}
