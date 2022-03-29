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

    [SerializeField]
    private Transform _bulletSpawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;

    private Animator _playerAnimator;
    private PlayerCam _playerCam;
    private PlayerInputs _input;
    private Vector3 _mousePos;

    private bool _canFire = true;

    private void Awake()
    {
        _playerAnimator = _player.GetComponentInChildren<Animator>();
        _playerCam = _player.GetComponent<PlayerCam>();
        _input = _player.GetComponent<PlayerInputs>();

        _zoomRotationSpeed = new Vector2(0.2f, 0.2f);

        _zoomCam.SetActive(false);
    }

    private void Update()
    {
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

    private void OnEnable()
    {
        _curBulletCnt = 20;
        _maxBulletAmt = 20;
        _reloadTime = 2f;
        _gunState = EGunState.Ready;
    }
    public override void Fire() 
    {
        if (_curBulletCnt > 0 && _canFire == true)
        {
            StartCoroutine(Shoot());
        }
    }
    public override void Zoom() 
    {
        _zoomCam.SetActive(true);
        _playerCam._rotationSpeedX = _zoomRotationSpeed.x;
        _playerCam._rotationSpeedY = _zoomRotationSpeed.y;
    }
    private IEnumerator Shoot()
    {
        --_curBulletCnt;
        Vector3 aimDir = (_mousePos - _bulletSpawnPos.position).normalized;
        Instantiate(_bulletPrefab, _bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        _playerAnimator.SetBool(PlayerAnimatorID.ISFIRE, true);
        _canFire = false;
        Debug.Log("³²Àº ÃÑ¾Ë : " + _curBulletCnt);

        yield return new WaitForSeconds(0.1f);

        _canFire = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISFIRE, false);
    }

    private void SetMousePos()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            _mousePos = raycastHit.point;
        }
    }
}
