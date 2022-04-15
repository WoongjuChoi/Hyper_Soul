using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfInformation : MonsterInformation
{
    [SerializeField]
    private GameObject _attackCollider = null;

    [SerializeField]
    private GameObject _startPoint = null;

    private WolfAlertState _monsterAlertState = null;
    private WolfAttackState _monsterAttackState = null;
    private WolfChaseState _monsterChaseState = null;
    private WolfDamagedState _monsterDamagedState = null;
    private WolfDieState _monsterDieState = null;
    private WolfIdleState _monsterIdleState = null;
    private WolfReturnPositionState _monsterReturnPositionState = null;
    private WolfSpawnState _monsterSpawnState = null;

    public GameObject AttackCollider { get { return _attackCollider; } }
    public GameObject StartPoint { get { return _startPoint; } }

    public override void Awake()
    {
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        _monsterType = MonsterType.Wolf;
        Level = 1;

        _attackCollider.SetActive(false);

        _monsterAlertState = GetComponent<WolfAlertState>();
        _monsterAttackState = GetComponent<WolfAttackState>();
        _monsterChaseState = GetComponent<WolfChaseState>();
        _monsterDamagedState = GetComponent<WolfDamagedState>();
        _monsterDieState = GetComponent<WolfDieState>();
        _monsterIdleState = GetComponent<WolfIdleState>();
        _monsterReturnPositionState = GetComponent<WolfReturnPositionState>();
        _monsterSpawnState = GetComponent<WolfSpawnState>();

        _monsterFSM.AddState(EStateIDs.Alert, _monsterAlertState);
        _monsterFSM.AddState(EStateIDs.Attack, _monsterAttackState);
        _monsterFSM.AddState(EStateIDs.Chase, _monsterChaseState);
        _monsterFSM.AddState(EStateIDs.Damaged, _monsterDamagedState);
        _monsterFSM.AddState(EStateIDs.Die, _monsterDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _monsterIdleState);
        _monsterFSM.AddState(EStateIDs.ReturnPosition, _monsterReturnPositionState);
        _monsterFSM.AddState(EStateIDs.Spawn, _monsterSpawnState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SampleObjectParameterID.LAYER_SAMPLE_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (SampleObjectParameterID.LAYER_SAMPLE_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SampleObjectParameterID.LAYER_SAMPLE_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = false;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (false == _isDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Alert == _monsterCurrentState || EStateIDs.Attack == _monsterCurrentState || EStateIDs.Chase == _monsterCurrentState || EStateIDs.Idle == _monsterCurrentState)
            {
                _collisionVec = gameObject.transform.position;

                _isDamaged = true;

                _attackerInfo = collision.gameObject.GetComponent<Projectile>();

                _target = PhotonView.Find(_attackerInfo.ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;

                Vector3 targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);

                _lookAtTargetVec = targetPosition - _collisionVec;
            }
        }

        if (SampleObjectParameterID.LAYER_SAMPLE_PLAYER == collision.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }
}
