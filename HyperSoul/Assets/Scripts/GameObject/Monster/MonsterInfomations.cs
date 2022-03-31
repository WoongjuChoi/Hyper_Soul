using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfomations : MonoBehaviour
{
    [SerializeField]
    private FiniteStateMachine _monsterFSM = null;

    [SerializeField]
    private MonsterSpawnManager _monsterSpawnManager = null;

    [SerializeField]
    private Chaser _monsterChaser = null;

    [SerializeField]
    private Collider _attackRangeCollider = null;

    [SerializeField]
    private Transform _monsterRayPoint = null;

    [SerializeField]
    private float _monsterInvincibleTime = 0f;

    [SerializeField]
    private int _monsterMaxHP = 0;

    private MonsterAlertState _monsterAlertState = new MonsterAlertState();
    private MonsterAttackState _monsterAttackState = new MonsterAttackState();
    private MonsterChaseState _monsterChaseState = new MonsterChaseState();
    private MonsterDamagedState _monsterDamagedState = new MonsterDamagedState();
    private MonsterDieState _monsterDieState = new MonsterDieState();
    private MonsterIdleState _monsterIdleState = new MonsterIdleState();
    private MonsterReturnPositionState _monsterReturnPositionState = new MonsterReturnPositionState();
    private MonsterSpawnState _monsterSpawnState = new MonsterSpawnState();

    private GameObject _target = null;

    private Transform _initializePosition = null;

    private Vector3 _collisionVec = Vector3.zero;
    private Vector3 _lookAtTargetVec = Vector3.zero;

    private EStateIDs _monsterCurrentState = EStateIDs.None;

    private bool _isDamaged = false;
    private bool _isDie = false;
    private bool _isTargeting = false;
    private bool _isWithinAttackRange = false;

    private float _monsterSpawnDirection = 0f;

    private int _monsterCurrentHP = 0;

    public EStateIDs MonsterCurrentState { get { return _monsterCurrentState; } set { _monsterCurrentState = value; } }
    public bool IsDamaged { get { return _isDamaged; } set { _isDamaged = value; } }
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsTargeting { get { return _isTargeting; } set { _isTargeting = value; } }
    public int MonsterCurrentHP { get { return _monsterCurrentHP; } set { _monsterCurrentHP = value; } }

    public GameObject Target { get { return _target; } }
    public Chaser MonsterChaser { get { return _monsterChaser; } }
    public Collider MonsterAttackRangeCollider { get { return _attackRangeCollider; } }
    public Transform InitializePosition
    { get { return _initializePosition; } }
    public Transform MonsterRayPoint { get { return _monsterRayPoint; } }
    public Vector3 CollisionVec { get { return _collisionVec; } }
    public Vector3 LookAtTargetVec { get { return _lookAtTargetVec; } }
    public bool IsWithinAttackRange { get { return _isWithinAttackRange; } }
    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
    public float MonsterSpawnDirection { get { return _monsterSpawnDirection; } }
    public int MonsterMaxHP { get { return _monsterMaxHP; } }

    private void Awake()
    {
        _monsterFSM.AddState(EStateIDs.Alert, _monsterAlertState);
        _monsterFSM.AddState(EStateIDs.Attack, _monsterAttackState);
        _monsterFSM.AddState(EStateIDs.Chase, _monsterChaseState);
        _monsterFSM.AddState(EStateIDs.Damaged, _monsterDamagedState);
        _monsterFSM.AddState(EStateIDs.Die, _monsterDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _monsterIdleState);
        _monsterFSM.AddState(EStateIDs.ReturnPosition, _monsterReturnPositionState);
        _monsterFSM.AddState(EStateIDs.Spawn, _monsterSpawnState);
    }

    private void OnEnable()
    {
        _monsterFSM.ChangeState(EStateIDs.Spawn);

        _monsterCurrentHP = _monsterMaxHP;

        _initializePosition = _monsterSpawnManager.gameObject.transform;

        gameObject.transform.position = _initializePosition.position;

        _monsterSpawnDirection = _monsterSpawnManager.InitializeDirection;

        Quaternion monsterInitializeDirection = Quaternion.Euler(0f, _monsterSpawnDirection, 0f);

        gameObject.transform.rotation = monsterInitializeDirection;

        _isDie = false;
        _isTargeting = false;
        _isWithinAttackRange = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (false == _isDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Alert == _monsterCurrentState || EStateIDs.Attack == _monsterCurrentState || EStateIDs.Chase == _monsterCurrentState || EStateIDs.Idle == _monsterCurrentState)
            {
                _isDamaged = true;

                _target = collision.gameObject.GetComponent<BazookaMissile>().MisilleOwner;

                _collisionVec = collision.gameObject.transform.position;

                Vector3 targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);

                _lookAtTargetVec = targetPosition - _collisionVec;
            }
        }

        if (SampleObjectParameterID.LAYER_SAMPLE_PLAYER == collision.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }

    private void Update()
    {
        // 디버깅용(몬스터가 맞은 위치로부터 플레이어가 쏜 방향의 반대로 레이)
        Debug.DrawRay(_collisionVec, _lookAtTargetVec * 1000f, Color.red);
        Debug.DrawRay(_monsterRayPoint.position, gameObject.transform.forward * 1000f, Color.black);

        //Debug.Log($"_monsterCurrentHP : {_monsterCurrentHP}");
        //Debug.Log($"gameObject.transform.eulerAngles: {gameObject.transform.eulerAngles}");
    }
}
