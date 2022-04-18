using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField]
    private float _moveSpeed = 20.0f;
    [SerializeField]
    private float _jumpForce = 10f;
    [SerializeField]
    private Transform _cameraArm;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private GameObject _walkingSound;
    [SerializeField]
    protected GameObject _jumpSound;

    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;
    private PlayerCam _playerCam;
    private PlayerInputs _input;
    private PlayerInfo _playerInfo;

    private bool _isJump = false;
    private float _aim = 0.5f;

    private Vector3 _storeFirePosition = Vector3.zero;  // (22.04.01) 슈팅했을 때의 플레이어의 위치값을 저장하는 변수 추가
    
    public Vector3 StoreFirePosition { get { return _storeFirePosition; } } // (22.04.01) 슈팅했을 때의 플레이어의 위치값을 저장하는 변수의 프로퍼티 추가

    private bool isLoaded = false;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerCam = GetComponent<PlayerCam>();
        _input = GetComponent<PlayerInputs>();
        _playerInfo = GetComponent<PlayerInfo>();
        _walkingSound.SetActive(false);

        
    }

    private void Update()
    {
        if (/*false == GameManager.TimeManager.StartGame || */_playerInfo.IsDead)
        {
            _walkingSound.SetActive(false);
            _jumpSound.SetActive(false);

            return;
        }

        MoveAnimation();
        JumpAnimation();
        MoveSound();
        JumpSound();
        Reload();
        Fire();
    }

    private void FixedUpdate()
    {
        if (/*false == GameManager.TimeManager.StartGame || */_playerInfo.IsDead)
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
    }

    private void Move()
    {
        float dtMoveSpeed = _moveSpeed * Time.deltaTime;
        // 캐릭터의 로컬 전방을 알기 위한 Vector3 변수
        Vector3 lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
        Vector3 moveVector = lookForward * _input.MoveVec.y + lookRight * _input.MoveVec.x;
        _playerRigidbody.MovePosition(_playerRigidbody.position + moveVector * dtMoveSpeed);
    }

    private void MoveSound()
    {
        if (_input.MoveVec != Vector2.zero && _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimatorID.MOVEMENT))
        {
            _walkingSound.SetActive(true);
        }
        else
        {
            _walkingSound.SetActive(false);
        }
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
    private void JumpSound()
    {
        if (_playerAnimator.GetBool(PlayerAnimatorID.ISJUMP) && photonView.IsMine)
        {
            _jumpSound.SetActive(true);
        }
        else
        {
            _jumpSound.SetActive(false);
        }
    }
    private void Reload()
    {
        if (_input.IsReload && _weapon.HasReloaded())
        {
            _playerAnimator.SetTrigger(PlayerAnimatorID.RELOAD);
            _input.IsReload = false;

            // 결과 씬 디버깅용 코드
            //if (PhotonNetwork.IsMasterClient && isLoaded == false)
            //{
            //    GoResultScene();
            //}

            _playerInfo.LevelUpdate();
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

    // 결과 씬 디버깅용
    private void GoResultScene()
    {
        isLoaded = true;
        PhotonNetwork.LoadLevel("ResultScene");
    }

    // 점프 애니메이션 처리를 위한 트리거 콜라이더 처리
    private void OnTriggerStay(Collider other)
    {
        if (false == _playerInfo.IsDead)
        {
            _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, false);
            _isJump = false;
            _input.IsJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (false == _playerInfo.IsDead)
        {
            _playerAnimator.SetTrigger(PlayerAnimatorID.FALLING);
            _isJump = true;
            _playerAnimator.SetBool(PlayerAnimatorID.ISJUMP, true);
        }
    }
}
