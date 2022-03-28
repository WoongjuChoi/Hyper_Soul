using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private Transform _playerBody;

    [SerializeField]
    private Transform _cameraArm;

    [SerializeField]
    private Transform _cameraPos;

    [SerializeField]
    private float _rotationSpeedX = 0.5f;

    [SerializeField]
    private float _rotationSpeedY = 0.5f;

    [SerializeField]
    private Vector2 _normalRotationSpeed;

    [SerializeField]
    private Vector2 _zoomRotationSpeed;

    private float _limitMinX = -80f;
    private float _limitMaxX = 50f;
    public float _eulerAngleX { get; private set; }
    private float _eulerAngleY;

    private PlayerInfo _playerInfo;
    private PlayerInputs _input;
    private RaycastHit _hit;
    private float _rayDistance = 3f;
    private Vector3 _defaultCamPos;

    private void Awake()
    {
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _defaultCamPos = new Vector3(1.5f, 0.4f, -3.4f);
        _normalRotationSpeed = new Vector2(0.5f, 0.5f);
        _zoomRotationSpeed = new Vector2(0.2f, 0.2f);
    }
    
    private void Update()
    {
        if (_playerInfo.IsDead)
        {
            return;
        }
        
        if (_input.IsZoom)
        {
            _rotationSpeedX = _zoomRotationSpeed.x;
            _rotationSpeedY = _zoomRotationSpeed.y;
        }
        else
        {
            _rotationSpeedX = _normalRotationSpeed.x;
            _rotationSpeedY = _normalRotationSpeed.y;
        }

        MouseRotate(_input.MousePos.x, _input.MousePos.y);
        camCollisionFix();
    }

    public void MouseRotate(float mouseX, float mouseY)
    {
        _eulerAngleY += mouseX * _rotationSpeedY;
        // 마우스 아래로 내리면 음수인데 오브젝트의 x축이 +방향으로 회전해야 아래를 보기 때문에 -연산
        _eulerAngleX -= mouseY * _rotationSpeedX;

        _eulerAngleX = clampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        _cameraArm.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
        _playerBody.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }

    private float clampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private void camCollisionFix()
    {
        if (Physics.Raycast(_cameraArm.transform.position, (_cameraPos.position - _cameraArm.transform.position).normalized, out _hit, _rayDistance))
        {
            if (_hit.transform.gameObject.tag != TagParameterID.PLAYER)
            {
                _cameraPos.position = _hit.point;
            }
        }
        else
        {
            _cameraPos.localPosition = _defaultCamPos;
        }
    }
}
