using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterInformation : LivingEntity
{
    [SerializeField]
    protected FiniteStateMachine _monsterFSM = null;

    [SerializeField]
    protected MonsterSpawnManager _monsterSpawnManager = null;

    [SerializeField]
    protected Chaser _monsterChaser = null;

    [SerializeField]
    protected Collider _attackRangeCollider = null;

    [SerializeField]
    protected Transform _monsterRayPoint = null;

    [SerializeField]
    protected float _monsterInvincibleTime = 0f;

    [SerializeField]
    protected int _monsterMaxHP = 0;

    protected GameObject _target = null;

    protected Transform _initializePosition = null;

    protected Vector3 _collisionVec = Vector3.zero;
    protected Vector3 _lookAtTargetVec = Vector3.zero;

    protected EStateIDs _monsterCurrentState = EStateIDs.None;

    protected float _monsterSpawnDirection = 0f;

    protected bool _isDamaged = false;
    protected bool _isDie = false;
    protected bool _isTargeting = false;
    protected bool _isWithinAttackRange = false;

    protected int _monsterCurrentHP = 0;

    public GameObject Target { get { return _target; } }
    public Chaser MonsterChaser { get { return _monsterChaser; } }
    public Collider MonsterAttackRangeCollider { get { return _attackRangeCollider; } }
    public Transform InitializePosition { get { return _initializePosition; } }
    public Transform MonsterRayPoint { get { return _monsterRayPoint; } }
    public Vector3 CollisionVec { get { return _collisionVec; } }
    public Vector3 LookAtTargetVec { get { return _lookAtTargetVec; } }
    public bool IsWithinAttackRange { get { return _isWithinAttackRange; } }
    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
    public float MonsterSpawnDirection { get { return _monsterSpawnDirection; } }
    public int MonsterMaxHP { get { return _monsterMaxHP; } }

    public EStateIDs MonsterCurrentState { get { return _monsterCurrentState; } set { _monsterCurrentState = value; } }
    public bool IsDamaged { get { return _isDamaged; } set { _isDamaged = value; } }
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsTargeting { get { return _isTargeting; } set { _isTargeting = value; } }
    public int MonsterCurrentHP { get { return _monsterCurrentHP; } set { _monsterCurrentHP = value; } }

    public abstract void Awake();

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
}
