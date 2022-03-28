using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private Vector2 _zoomRotationSpeed;

    [SerializeField]
    private GameObject _zoomCam;

    [SerializeField]
    private GameObject _player;

    private Animator _playerAnimator;
    private PlayerCam _playerCam;
    private PlayerInputs _input;

    private void Awake()
    {
        _playerAnimator = _player.GetComponent<Animator>();
        _playerCam = _player.GetComponent<PlayerCam>();
        _input = _player.GetComponent<PlayerInputs>();

        _zoomRotationSpeed = new Vector2(0.2f, 0.2f);

        _zoomCam.SetActive(false);
    }

    private void Update()
    {
        if (_input.IsZoom)
        {
            _zoomCam.SetActive(true);
            _playerCam._rotationSpeedX = _zoomRotationSpeed.x;
            _playerCam._rotationSpeedY = _zoomRotationSpeed.y;
        }
        else
        {
            _zoomCam.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _curBulletCnt = 20;
        _maxBulletAmt = 20;
        _reloadTime = 2f;
        _gunState = EGunState.Ready;
    }
    public override void Fire() 
    {
        --_curBulletCnt;
    }
    public override void Zoom() 
    {
    }
}
