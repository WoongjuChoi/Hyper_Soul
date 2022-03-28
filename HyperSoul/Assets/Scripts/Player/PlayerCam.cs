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
    private float _rotCamXAxisSpeed = 0.5f;

    [SerializeField]
    private float _rotCamYAxisSpeed = 0.5f;

    private float _limitMinX = -80f;
    private float _limitMaxX = 50f;
    public float _eulerAngleX { get; private set; }
    private float _eulerAngleY;

    private PlayerInputs _input;
    private RaycastHit _hit;
    private float _rayDistance = 3f;
    private Vector3 _defaultCamPos;

    private void Awake()
    {
        _input = GetComponent<PlayerInputs>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _defaultCamPos = new Vector3(1.5f, 0.4f, -3.4f);
    }
    
    private void Update()
    {
        MouseRotate(_input.MousePos.x, _input.MousePos.y);
        
        camCollisionFix();
    }

    public void MouseRotate(float mouseX, float mouseY)
    {
        _eulerAngleY += mouseX * _rotCamYAxisSpeed;
        // ���콺 �Ʒ��� ������ �����ε� ������Ʈ�� x���� +�������� ȸ���ؾ� �Ʒ��� ���� ������ -����
        _eulerAngleX -= mouseY * _rotCamXAxisSpeed;

        _eulerAngleX = clampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        _cameraArm.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
        _playerBody.rotation = Quaternion.Euler(0, _eulerAngleY, 0);

        Debug.Log("_eulerAngleX : " + _eulerAngleX);
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
