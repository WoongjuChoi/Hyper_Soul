using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantInformation : MonsterInformation
{
    [SerializeField]
    private GameObject _stompAttackArea = null;

    [SerializeField]
    private float _viewAngle = 0f;

    [SerializeField]
    private float _rotateSpeed = 0f;

    private TreantAttackState _treantAttackState = null;
    private TreantDamagedState _treantDamagedState = null;
    private TreantDieState _treantDieState = null;
    private TreantIdleState _treantIdleState = null;
    private TreantReturnPositionState _treantReturnPositionState = null;
    private TreantRotatePositionState _treantRotatePositionState = null;
    private TreantSpawnState _treantSpawnState = null;

    private Vector3 _vecMonsterToTarget = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;

    private bool _existInSight = false;

    private const float DOT_120_DEGREE = -0.5f;

    public GameObject StompAttackArea { get { return _stompAttackArea; } }
    public Vector3 VectorMonsterToTarget { get { return _vecMonsterToTarget; } }
    public Vector3 OriginVec { get; set; }
    public bool ExistInSight { get { return _existInSight; } }
    public float DistanceMonsterToTarget { get; private set; }
    public float RotateSpeed { get { return _rotateSpeed; } }

    public override void Awake()
    {
        //_hitImage.SetActive(false);
        //_hitSound.SetActive(false);
        //_deathSound.SetActive(false);
        //_animator = GetComponentInChildren<Animator>();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        _monsterType = MonsterType.Tree;
        Level = 1;

        _treantAttackState = GetComponent<TreantAttackState>();
        _treantDamagedState = GetComponent<TreantDamagedState>();
        _treantDieState = GetComponent<TreantDieState>();
        _treantIdleState = GetComponent<TreantIdleState>();
        _treantReturnPositionState = GetComponent<TreantReturnPositionState>();
        _treantRotatePositionState = GetComponent<TreantRotatePositionState>();
        _treantSpawnState = GetComponent<TreantSpawnState>();

        _monsterFSM.AddState(EStateIDs.Attack, _treantAttackState);
        _monsterFSM.AddState(EStateIDs.Damaged, _treantDamagedState);
        _monsterFSM.AddState(EStateIDs.Die, _treantDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _treantIdleState);
        _monsterFSM.AddState(EStateIDs.ReturnPosition, _treantReturnPositionState);
        _monsterFSM.AddState(EStateIDs.RotatePosition, _treantRotatePositionState);
        _monsterFSM.AddState(EStateIDs.Spawn, _treantSpawnState);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (false == _isDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Attack == _monsterCurrentState || EStateIDs.Idle == _monsterCurrentState || EStateIDs.RotatePosition == _monsterCurrentState)
            {
                _collisionVec = collision.gameObject.transform.position;

                _isDamaged = true;

                if (false == _isTargeting)
                {
                    _target = PhotonView.Find(collision.gameObject.GetComponent<Projectile>().ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;

                    _isTargeting = true;
                }
            }
        }
    }

    private void Update()
    {
        if (_isTargeting)
        {
            _targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);

            ExistInTreantSight();

            Vector3 gameObjecPosition = new Vector3(gameObject.transform.position.x, _targetPosition.y, gameObject.transform.position.z);

            DistanceMonsterToTarget = (_targetPosition - gameObjecPosition).magnitude;
        }

        // µð¹ö±ë¿ë
        Debug.DrawRay(_collisionVec, _lookAtTargetVec * 1000f, Color.red);
        Debug.DrawRay(_monsterRayPoint.position, gameObject.transform.forward * 1000f, Color.black);

        //Debug.Log($"_monsterCurrentHP : {_monsterCurrentHP}");
        //Debug.Log($"gameObject.transform.eulerAngles: {gameObject.transform.eulerAngles}");
    }

    private void ExistInTreantSight()
    {
        _lookAtTargetVec = _targetPosition - _collisionVec;

        Vector3 monsterSightPosition = new Vector3(_monsterRayPoint.position.x, _targetPosition.y, _monsterRayPoint.position.z);

        _vecMonsterToTarget = (_targetPosition - monsterSightPosition).normalized;

        float dotMonsterToTarget = Vector3.Dot(gameObject.transform.forward, _vecMonsterToTarget);

        if (dotMonsterToTarget > Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
        {
            _existInSight = true;
        }
        else
        {
            _existInSight = false;
        }
    }
}
