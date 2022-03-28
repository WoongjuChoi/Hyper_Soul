using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;
    private PlayerCam _playerCam;
    private PlayerInputs _input;
    private PlayerInfo _playerInfo;

    private bool _isJump = false;
    private bool _isHit = false;
    private float _aim = 0.5f;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerCam = GetComponent<PlayerCam>();
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (_playerInfo.IsDead)
        {
            return;
        }

        moveAnimation();
        jumpAnimation();
        aimAnimation();
        Reload();
        Fire();
        Debug.Log(_input.IsReload);
    }

    private void FixedUpdate()
    {
        if (_playerInfo.IsDead)
        {
            return;
        }

        move();
        jump();
    }

    private void moveAnimation()
    {
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _input.MoveVec.y);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _input.MoveVec.x);
    }
    private void move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        // 캐릭터의 로컬 전방을 알기 위한 Vector3 변수
        Vector3 lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
        Vector3 moveVector = lookForward * _input.MoveVec.y + lookRight * _input.MoveVec.x;
        _playerRigidbody.MovePosition(_playerRigidbody.position + moveVector * dtMoveSpeed);
    }

    private void jumpAnimation()
    {
        if (_isJump && _playerAnimator.GetBool(PlayerAnimatorID.ISJUMP) == false)
        {
            _playerAnimator.SetTrigger(PlayerAnimatorID.DOJUMP);
            _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, true);
        }
    }
    private void jump()
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
            _weapon.Fire();
        }
    }
    private void aimAnimation()
    {
        // 0 ~ 1 사이의 값을 얻기 위해 -80 ~ 50도의 제약이 있는 playerCam의 eulerAngleX의 값을 조정
        _aim = (_playerCam._eulerAngleX + 80f) / 130f;
        _playerAnimator.SetFloat(PlayerAnimatorID.AIM, _aim);
    }

    public IEnumerator Hit()
    {
        _isHit = true;
        _playerAnimator.SetTrigger(PlayerAnimatorID.HIT);
        yield return new WaitForSeconds(0.3f);

        _isHit = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagParameterID.BULLET)
        {
            --_playerInfo.Hp;
            Debug.Log("Player Hp : " + _playerInfo.Hp);

            if (_playerInfo.Hp <= 0)
            {
                _playerInfo.IsDead = true;
                _playerAnimator.SetTrigger(PlayerAnimatorID.DIE);
            }
            else if (false == _isHit)
            {
                StartCoroutine(Hit());
            }
        }
    }
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
