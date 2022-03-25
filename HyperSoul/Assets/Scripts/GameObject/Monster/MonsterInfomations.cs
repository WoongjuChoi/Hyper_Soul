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
    private MonsterDieState _monsterDieState = new MonsterDieState();
    private MonsterIdleState _monsterIdleState = new MonsterIdleState();
    private MonsterReturnPositionState _monsterReturnPositionState = new MonsterReturnPositionState();
    private MonsterSpawnState _monsterSpawnState = new MonsterSpawnState();

    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
    public int MonsterHP { get { return _monsterHP; } }

    private void Awake()
    {
        _monsterFSM.AddState(EStateIDs.Alert, _monsterAlertState);
        _monsterFSM.AddState(EStateIDs.Attack, _monsterAttackState);
        _monsterFSM.AddState(EStateIDs.Chase, _monsterChaseState);
        _monsterFSM.AddState(EStateIDs.Die, _monsterDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _monsterIdleState);
        _monsterFSM.AddState(EStateIDs.ReturnPosition, _monsterReturnPositionState);
        _monsterFSM.AddState(EStateIDs.Spawn, _monsterSpawnState);
    }

    private void OnEnable()
    {
        _monsterFSM.InitializeState(EStateIDs.Spawn);
    }
}
