using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 8.0f;
    [SerializeField]
    private float _jumpForce = 10f;
    [SerializeField]
    private Transform _cameraArm;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private GameObject _hitImage;
    [SerializeField]
    private GameObject _walkingSound;
    [SerializeField]
    private GameObject _hitSound;
    [SerializeField]
    private GameObject _deathSound;

    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;
    private PlayerCam _playerCam;
    private PlayerInputs _input;
    private PlayerInfo _playerInfo;

    private bool _isJump = false;
    private bool _isHit = false;
    private float _aim = 0.5f;

    private Vector3 _storeFirePosition = Vector3.zero;  // (22.04.01) �������� ���� �÷��̾��� ��ġ���� �����ϴ� ���� �߰�
    
    public Vector3 StoreFirePosition { get { return _storeFirePosition; } } // (22.04.01) �������� ���� �÷��̾��� ��ġ���� �����ϴ� ������ ������Ƽ �߰�

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerCam = GetComponent<PlayerCam>();
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();
        _hitImage.SetActive(false);
        _walkingSound.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
    }

    private void Update()
    {
        if (_playerInfo.IsDead)
        {
            return;
        }

        MoveAnimation();
        JumpAnimation();
        AimAnimation();
        Reload();
        Fire();
    }

    private void FixedUpdate()
    {
        if (_playerInfo.IsDead)
        {
            return;
        }

        Move();
        Jump();
    }

    private void MoveAnimation()
    {
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _input.MoveVec.y);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _input.MoveVec.x);

        if (_input.MoveVec != Vector2.zero)
        {
            _walkingSound.SetActive(true);
        }
        else
        {
            _walkingSound.SetActive(false);
        }
    }

    private void Move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        // ĳ������ ���� ������ �˱� ���� Vector3 ����
        Vector3 lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
        Vector3 moveVector = lookForward * _input.MoveVec.y + lookRight * _input.MoveVec.x;
        _playerRigidbody.MovePosition(_playerRigidbody.position + moveVector * dtMoveSpeed);
    }

    private void JumpAnimation()
    {
        if (_isJump && _playerAnimator.GetBool(PlayerAnimatorID.ISJUMP) == false)
        {
            _playerAnimator.SetTrigger(PlayerAnimatorID.DOJUMP);
            _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, true);
        }
    }
    private void Jump()
    {
        if (_input.IsJump && _isJump == false)
        {
            _isJump = true;
            _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    private void Reload()
    {
        if (_input.IsReload && _weapon.HasReloaded())
        {
            _playerAnimator.SetTrigger(PlayerAnimatorID.RELOAD);
            _input.IsReload = false;
        }
    }
    private void Fire()
    {
        if (_input.IsShoot)
        {
            _storeFirePosition = gameObject.transform.position;
            _weapon.Fire();
        }
    }

    private void AimAnimation()
    {
        // 0 ~ 1 ������ ���� ��� ���� -80 ~ 50���� ������ �ִ� playerCam�� eulerAngleX�� ���� ����
        _aim = (_playerCam._eulerAngleX + 80f) / 130f;
        _playerAnimator.SetFloat(PlayerAnimatorID.AIM, _aim);
    }
    public IEnumerator Hit()
    {
        _isHit = true;
        _playerAnimator.SetTrigger(PlayerAnimatorID.HIT);
        _hitImage.SetActive(true);
        _hitSound.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        _isHit = false;
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
    }

    // ���ݴ����� ���� ó���ϴ� CollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == TagParameterID.BULLET)
        {
            --_playerInfo.CurrHp;
            Debug.Log("Player Hp : " + _playerInfo.CurrHp);

            if (_playerInfo.CurrHp <= 0)
            {
                _playerInfo.IsDead = true;
                _playerAnimator.SetTrigger(PlayerAnimatorID.DIE);
                _deathSound.SetActive(true);
                _walkingSound.SetActive(false);
            }
            else if (false == _isHit)
            {
                StartCoroutine(Hit());
            }
        }
    }

    // ���� �ִϸ��̼� ó���� ���� Ʈ���� �ݶ��̴� ó��
    private void OnTriggerStay(Collider other)
    {
         _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, false);
        _isJump = false;
        _input.IsJump = false;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerAnimator.SetTrigger(PlayerAnimatorID.FALLING);
        _isJump = true;
        _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, true);
    }
}
