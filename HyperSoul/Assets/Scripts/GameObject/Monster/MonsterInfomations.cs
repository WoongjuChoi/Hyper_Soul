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
    private int _monsterHP = 0;

    private MonsterAlertState _monsterAlertState = new MonsterAlertState();
    private MonsterAttackState _monsterAttackState = new MonsterAttackState();
    private MonsterChaseState _monsterChaseState = new MonsterChaseState();
    private MonsterDamagedState _monsterDamagedState = new MonsterDamagedState();
    private MonsterDieState _monsterDieState = new MonsterDieState();
    private MonsterIdleState _monsterIdleState = new MonsterIdleState();
    private MonsterReturnPositionState _monsterReturnPositionState = new MonsterReturnPositionState();
    private MonsterSpawnState _monsterSpawnState = new MonsterSpawnState();

    private bool _isWithinAttackRange = false;

    public GameObject Target = null;
    public EStateIDs MonsterCurrentState { get; set; }
    public bool IsDamaged { get; set; }

    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
    public int MonsterHP { get { return _monsterHP; } }
    public bool IsWithinAttackRange { get { return _isWithinAttackRange; } }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EStateIDs.Damaged == MonsterCurrentState || EStateIDs.Chase == MonsterCurrentState)
        {
            _isWithinAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (EStateIDs.Damaged == MonsterCurrentState || EStateIDs.Attack == MonsterCurrentState)
        {
            _isWithinAttackRange = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (false == IsDamaged && EStateIDs.Idle == MonsterCurrentState)
        {
            IsDamaged = true;

            Target = collision.gameObject.GetComponent<SampleAmmo>().Owner;
        }
    }

    private void Update()
    {
    }
}
