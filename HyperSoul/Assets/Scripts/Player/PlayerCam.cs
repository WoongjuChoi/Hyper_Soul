using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class PlayerCam : MonoBehaviourPun
{
    [SerializeField]
    private Transform _playerBody;

    [SerializeField]
    private Transform _cameraArm;

    [SerializeField]
    private Transform _followCameraPos;

    public float _rotationSpeedX = 0.5f;
    public float _rotationSpeedY = 0.5f;

    [SerializeField]
    private Vector2 _normalRotationSpeed;

    private float _limitMinX = -80f;
    private float _limitMaxX = 50f;
    public float _eulerAngleX { get; private set; }
    private float _eulerAngleY;

    private PlayerInfo _playerInfo;
    private PlayerInputs _input;
    private RaycastHit _hit;
    private float _rayDistance = 3f;
    private Vector3 _defaultCamPos;
    private Animator _playerAnimator;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera[] followCam = _cameraArm.GetComponentsInChildren<CinemachineVirtualCamera>();
            foreach (CinemachineVirtualCamera cam in followCam)
            {
                if (cam.gameObject.name == "AimCamera")
                {
                    cam.Priority = 15;
                    cam.gameObject.SetActive(false);
                }
                else if (cam.gameObject.name == "PlayerFollowCamera")
                {
                    cam.Priority = 14;
                }
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            GameManager.Instance.PlayerCamRotationTransform = _cameraArm;
        }

        _defaultCamPos = new Vector3(1.5f, 0.4f, -3.4f);
        _normalRotationSpeed = new Vector2(0.5f, 0.5f);
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (false == photonView.IsMine)
        {
            return;
        }

        if (_playerInfo.IsDead)
        {
            return;
        }

        if (false == _input.IsZoom)
        {
            _rotationSpeedX = _normalRotationSpeed.x;
            _rotationSpeedY = _normalRotationSpeed.y;
        }

        MouseRotate(_input.MousePos.x, _input.MousePos.y);
        camCollisionFix();

        PlayerRotation();
    }

    public void MouseRotate(float mouseX, float mouseY)
    {
        _eulerAngleY += mouseX * _rotationSpeedY;
        // 마우스 아래로 내리면 음수인데 오브젝트의 x축이 +방향으로 회전해야 아래를 보기 때문에 -연산
        _eulerAngleX -= mouseY * _rotationSpeedX;

        _eulerAngleX = clampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        _cameraArm.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
    }

    private float clampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private void camCollisionFix()
    {
        if (Physics.Raycast(_cameraArm.transform.position, (_followCameraPos.position - _cameraArm.transform.position).normalized, out _hit, _rayDistance))
        {
            if (_hit.transform.gameObject.tag != TagParameterID.PLAYER)
            {
                _followCameraPos.position = _hit.point;
            }
        }
        else
        {
            _followCameraPos.localPosition = _defaultCamPos;
        }
    }

    private void PlayerRotation()
    {
        // 0 ~ 1 사이의 값을 얻기 위해 -80 ~ 50도의 제약이 있는 playerCam의 eulerAngleX의 값을 조정
        _playerAnimator.SetFloat(PlayerAnimatorID.AIM, (_eulerAngleX + 80f) / 130f);
        _playerBody.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(_playerBody.rotation);
    //        stream.SendNext(_playerAnimator);
    //    }
    //    else
    //    {
    //        _playerBody = (Transform)stream.ReceiveNext();
    //        _playerAnimator = (Animator)stream.ReceiveNext();
    //    }
    //}
}