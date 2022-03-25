using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfFSM : MonoBehaviour
{    
    [SerializeField]
    private FiniteStateMachine _blackWolfFSM = null;

    private BlackWolfAlertState _blackWolfAlertState = new BlackWolfAlertState();
    private BlackWolfAttackState _blackWolfAttackState = new BlackWolfAttackState();
    private BlackWolfChaseState _blackWolfChaseState = new BlackWolfChaseState();
    private BlackWolfDieState _blackWolfDieState = new BlackWolfDieState();
    private BlackWolfIdleState _blackWolfIdleState = new BlackWolfIdleState();
    private BlackWolfReturnPositionState _blackWolfReturnPositionState = new BlackWolfReturnPositionState();
    private BlackWolfSpawnState _blackWolfSpawnState = new BlackWolfSpawnState();

    private void Awake()
    {
        _blackWolfFSM.AddState(EStateIDs.Alert, _blackWolfAlertState);
        _blackWolfFSM.AddState(EStateIDs.Attack, _blackWolfAttackState);
        _blackWolfFSM.AddState(EStateIDs.Chase, _blackWolfChaseState);
        _blackWolfFSM.AddState(EStateIDs.Die, _blackWolfDieState);
        _blackWolfFSM.AddState(EStateIDs.Idle, _blackWolfIdleState);
        _blackWolfFSM.AddState(EStateIDs.ReturnPosition, _blackWolfReturnPositionState);
        _blackWolfFSM.AddState(EStateIDs.Spawn, _blackWolfSpawnState);
    }

    private void OnEnable()
    {
        _blackWolfFSM.InitializeState(EStateIDs.Spawn);
    }
}
