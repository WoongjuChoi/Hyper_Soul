using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 8.0f;

    private PlayerInput _playerInput;
    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;

    private float _jumpForce = 10f;
    private bool _isJump = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveAnimation();
        jump();
    }

    private void FixedUpdate()
    {
        move();
        jumpAnimation();
    }

    private void moveAnimation()
    {
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _playerInput.VerticalMoveInput);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _playerInput.HorizontalMoveInput);
    }

    private void move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 moveVector = new Vector3(_playerInput.HorizontalMoveInput * dtMoveSpeed, 0, _playerInput.VerticalMoveInput * dtMoveSpeed);

        _playerRigidbody.MovePosition(_playerRigidbody.position + moveVector);
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
        if (_playerInput.JumpInput && _isJump == false)
        {
            _isJump = true;
            _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, false);
        _isJump = false;
    }
}
