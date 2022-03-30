using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Bazooka : Weapon
{
    [SerializeField]
    GameObject _shooter; // 수정예정
    [SerializeField]
    GameObject _missile = null;
    [SerializeField]
    Transform _missileSpawnPos = null;
    [SerializeField]
    GameObject _aimAngleRef;
    [SerializeField]
    private float _rayDist = 200f;
    [SerializeField]
    PlayerMovement _player;

    private Animator _bazookaAnim;

    private void Awake()
    {
        _player.MouseAction -= Trigger;
        _player.MouseAction += Trigger;
    }
    void Start()
    {
        _curBulletCnt = 100;
        _maxBulletAmt = 100;
        _reloadTime = 5;
        _gunState = EGunState.Ready;

        _bazookaAnim = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    _gunState = EGunState.Reloading;
        //    base.HasReloaded();
        //}
    }

    private void Trigger()
    {
        if (_curBulletCnt > 0 && _gunState == EGunState.Ready)
        {
            photonView.RPC("Fire", RpcTarget.MasterClient);
            Fire();
            if (_curBulletCnt <= 0)
            {
                _gunState = EGunState.Empty;
            }
        }

        _bazookaAnim.SetTrigger(PlayerAnimatorID.IS_SINGLE_SHOT);
    }

    [PunRPC]
    public override void Fire()
    {
        GameObject _bazookaMissile = Instantiate(_missile, _missileSpawnPos.position, _aimAngleRef.transform.rotation);
        _bazookaMissile.GetComponent<BazookaMissile>().Target = AimTarget()?.transform;
        _bazookaMissile.GetComponent<BazookaMissile>().MisilleOwner = this.gameObject;
        _bazookaMissile.transform.TransformDirection(_aimAngleRef.transform.forward);
        _bazookaMissile.GetComponent<Rigidbody>().velocity = new Vector3(0, _aimAngleRef.transform.localPosition.y * 3f, _aimAngleRef.transform.localPosition.z * 10f);
       
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
