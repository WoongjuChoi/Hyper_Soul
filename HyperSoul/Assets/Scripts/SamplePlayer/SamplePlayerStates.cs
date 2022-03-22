using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerStates : MonoBehaviour
{
    [SerializeField] private SamplePlayerMovement _playerMovement = null;

    private FiniteStateMachine _samplePlayerFSM = new FiniteStateMachine();

    private SamplePlayerIdleState _samplePlayerIdleState = new SamplePlayerIdleState();
    private SamplePlayerMoveState _samplePlayerMoveState = new SamplePlayerMoveState();
    private SamplePlayerAttackState _samplePlayerAttackState = new SamplePlayerAttackState();
    private SamplePlayerDamagedState _samplePlayerDamagedState = new SamplePlayerDamagedState();
    private SamplePlayerDieState _samplePlayerDieState = new SamplePlayerDieState();

    private void Start()
    {
        _samplePlayerFSM.AddState(EStateIDs.Idle, _samplePlayerIdleState);
        _samplePlayerFSM.AddState(EStateIDs.Move, _samplePlayerMoveState);
        _samplePlayerFSM.AddState(EStateIDs.Attack, _samplePlayerAttackState);
        _samplePlayerFSM.AddState(EStateIDs.Damaged, _samplePlayerDamagedState);
        _samplePlayerFSM.AddState(EStateIDs.Die, _samplePlayerDieState);

        _samplePlayerFSM.InitializeState(EStateIDs.Idle);
    }

    private void Update()
    {
        SamplePlayerMoving();
    }

    private void SamplePlayerMoving()
    {
        if (_playerMovement.IsMoving)
        {
            _samplePlayerFSM.ChangeState(EStateIDs.Move);
        }
        else
        {
            _samplePlayerFSM.ChangeState(EStateIDs.Idle);
        }
    }
}
