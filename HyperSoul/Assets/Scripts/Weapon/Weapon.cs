using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviourPun
{
    public Vector2 _zoomRotationSpeed;
    public GameObject _zoomCam;
    public GameObject _player;

    public int _curBulletCnt = 0;
    public int _maxBulletAmt = 0;

    protected float _reloadTime = 0;
    protected bool _canFire = true;
    protected int Damage = 0;

    protected Vector3 _mousePos;
    protected Animator _playerAnimator;
    protected PlayerCam _playerCam;
    protected PlayerInputs _input;
    protected EGunState _gunState;

    private void Awake()
    {
        _playerAnimator = _player.GetComponentInChildren<Animator>();
        _playerCam = _player.GetComponent<PlayerCam>();
        _input = _player.GetComponent<PlayerInputs>();

        _zoomRotationSpeed = new Vector2(0.2f, 0.2f);

        _zoomCam.SetActive(false);
    }
    protected void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        if (_input.IsZoom)
        {
            Zoom();
        }
        else
        {
            _zoomCam.SetActive(false);
        }

        SetMousePos();
    }

    public virtual void Fire() { }
    public virtual void Zoom() { }

    public bool HasReloaded()
    {
        if (_gunState == EGunState.Reloading || _curBulletCnt >= _maxBulletAmt)
        {
            return false;
        }
        Debug.Log(_reloadTime);
        StartCoroutine(Reload());
        return true;
    }
    private IEnumerator Reload()
    {
        // 사운드는 derivied class에서 구현
        _gunState = EGunState.Reloading;

        yield return new WaitForSeconds(_reloadTime);

        _curBulletCnt = _maxBulletAmt;

        _gunState = EGunState.Ready;
    }
    
    protected void SetMousePos()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            _mousePos = raycastHit.point;
        }
    }
}
