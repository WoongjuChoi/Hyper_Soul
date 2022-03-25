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
    private float _rotCamXAxisSpeed = 5f;

    [SerializeField]
    private float _rotCamYAxisSpeed = 3f;

    private float _limitMinX = -80f;
    private float _limitMaxX = 50f;
    private float _eulerAngleX;
    private float _eulerAngleY;

    private PlayerInput _playerInput;
    private Animator _playerAnimator;

    private RaycastHit _hit;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerInput = GetComponent<PlayerInput>();
        _playerAnimator = _playerBody.GetComponent<Animator>();
    }

    private void Update()
    {
        MouseRotate(_playerInput.MouseX, _playerInput.MouseY);
    }

    public void MouseRotate(float mouseX, float mouseY)
    {
        _eulerAngleY += mouseX * _rotCamYAxisSpeed;
        // 마우스 아래로 내리면 음수인데 오브젝트의 x축이 +방향으로 회전해야 아래를 보기 때문에 -연산
        _eulerAngleX -= mouseY * _rotCamXAxisSpeed;

        _eulerAngleX = ClampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        _cameraArm.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
        _playerBody.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
