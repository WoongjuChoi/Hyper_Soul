using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfomations : MonoBehaviour
{
    [SerializeField]
    private FiniteStateMachine _monsterFSM = null;

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

    private Vector3 _collisionVec = Vector3.zero;
    private Vector3 _lookAtTargetVec = Vector3.zero;

    private bool _isWithinAttackRange = false;

    private int _monsterCurrentHP = 0;

    public GameObject Target { get; private set; }
    public EStateIDs MonsterCurrentState { get; set; }
    public bool IsDamaged { get; set; }
    public int MonsterCurrentHP { get { return _monsterCurrentHP; } set { _monsterCurrentHP = value; } }

    public Vector3 CollisionVec { get { return _collisionVec; } }
    public Vector3 LookAtTargetVec { get { return _lookAtTargetVec; } }
    public bool IsWithinAttackRange { get { return _isWithinAttackRange; } }
    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
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

        _monsterCurrentHP = _monsterMaxHP;
    }

    private void OnEnable()
    {
        _monsterFSM.ChangeState(EStateIDs.Spawn);
    }

    private void OnTriggerEnter(Collider other)
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
        if (false == IsDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Alert == MonsterCurrentState || EStateIDs.Attack == MonsterCurrentState || EStateIDs.Chase == MonsterCurrentState || EStateIDs.Idle == MonsterCurrentState)
            {
                IsDamaged = true;

                _collisionVec = collision.gameObject.transform.position;
                _lookAtTargetVec = -collision.gameObject.transform.forward;

                Target = collision.gameObject.GetComponent<SampleAmmo>().Owner;
            }
        }

    }

    private void Update()
    {
        // 디버깅용(몬스터가 맞은 위치로부터 플레이어가 쏜 방향의 반대로 레이)
        Debug.DrawRay(_collisionVec, _lookAtTargetVec * 1000f, Color.red);

        Debug.Log($"_monsterCurrentHP : {_monsterCurrentHP}");
    }
}
