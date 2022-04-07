using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private float _moveSpeed = 8.0f;
<<<<<<< Updated upstream
=======

    [SerializeField]
    private float _jumpForce = 10f;

    [SerializeField]
    private Transform _cameraArm;

    [SerializeField]
    private Weapon _weapon;
>>>>>>> Stashed changes

    private PlayerInput _playerInput;
    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;

<<<<<<< Updated upstream
    void Awake()
=======
    private bool _isJump = false;
    private bool _isHit = false;
    private bool _isShoot = false;
    private float _aim = 0.5f;


    public bool IsShoot
    {
        set { _isShoot = value; }
    }

    private void Awake()
>>>>>>> Stashed changes
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
<<<<<<< Updated upstream
=======
        _playerCam = GetComponent<PlayerCam>();
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();
>>>>>>> Stashed changes
    }

    private void Update()
    {
<<<<<<< Updated upstream
        moveAnimation();
=======
        if (_playerInfo.IsDead || photonView.IsMine == false)
        {
            return;
        }
        MoveAnimation();
        JumpAnimation();
        AimAnimation();
        Reload();
        Fire();
>>>>>>> Stashed changes
    }

    void FixedUpdate()
    {
<<<<<<< Updated upstream
        move();
=======
        if (_playerInfo.IsDead || photonView.IsMine == false)
        {
            return;
        }
        Move();
        Jump();
>>>>>>> Stashed changes
    }

    private void moveAnimation()
    {
<<<<<<< Updated upstream
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _playerInput.VerticalMoveInput);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _playerInput.HorizontalMoveInput);
=======
        _playerAnimator.SetFloat(PlayerAnimatorID.VERTICAL, _input.MoveVec.y);
        _playerAnimator.SetFloat(PlayerAnimatorID.HORIZONTAL, _input.MoveVec.x);
>>>>>>> Stashed changes
    }

    private void move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 moveVector = new Vector3(_playerInput.HorizontalMoveInput * dtMoveSpeed, 0, _playerInput.VerticalMoveInput * dtMoveSpeed);

<<<<<<< Updated upstream
        _playerRigidbody.MovePosition(_playerRigidbody.position + moveVector);
=======
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
        if (_input.IsShoot )
        {
            _weapon.Fire();
        }
    }

    private void AimAnimation()
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

    // 공격당했을 때를 처리하는 CollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == TagParameterID.BULLET)
        {

            //--_playerInfo.Hp;
            //Debug.Log("Player Hp : " + _playerInfo.Hp);

            //if (_playerInfo.Hp <= 0)
            //{
            //    _playerInfo.IsDead = true;
            //    _playerAnimator.SetTrigger(PlayerAnimatorID.DIE);
            //}
            //else if (false == _isHit)
            //{
            //    StartCoroutine(Hit());
            //}
        }
    }

    // 점프 애니메이션 처리를 위한 트리거 콜라이더 처리
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
>>>>>>> Stashed changes
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
