using UnityEngine;
using UnityEngine.InputSystem;

public class SamplePlayerInput : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 0f;

    [SerializeField]
    private float _rotateSpeed = 180f;

    private PlayerInputs _playerInputs = null;

    private Rigidbody _rigidbody = null;

    private bool _isFire = false;

    private float _rotateHorizontal = 0f;

    public bool IsFire { get { return _isFire; } set { _isFire = value; } }

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 moveVec = new Vector3(_playerInputs.MoveVec.x, 0f, _playerInputs.MoveVec.y).normalized;

        _rigidbody.MovePosition(transform.position + _moveSpeed * Time.deltaTime * moveVec);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isFire = true;
        }

        float turnSpeed = _playerInputs.MousePos.x * _rotateSpeed * Time.deltaTime;

        gameObject.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0f, turnSpeed, 0f);

        Debug.Log(turnSpeed);
    }
}
