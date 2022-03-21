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

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveAnimation();
    }

    void FixedUpdate()
    {
        move();
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
}
