using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    PhotonView _photonView;

    [SerializeField]
    private float _moveSpeed = 8.0f;

    private PlayerInput _playerInput;
    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;
    private PlayerCam _playerCam;

    [SerializeField]
    private float _jumpForce = 10f;

    [SerializeField]
    private Transform _cameraArm;

    private bool _isJump = false;

    private void Awake()
    {
        _photonView = photonView;

        _playerInput = GetComponent<PlayerInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(!_photonView.IsMine)
        {
            return;
        }
        moveAnimation();
        jumpAnimation();
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        move();
        jump();
    }

    private void moveAnimation()
    {
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _playerInput.VerticalMoveInput);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _playerInput.HorizontalMoveInput);
    }

    private void move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
        Vector3 moveVector = lookForward * _playerInput.VerticalMoveInput + lookRight * _playerInput.HorizontalMoveInput;
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
