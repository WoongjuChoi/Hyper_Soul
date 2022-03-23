using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerStates : MonoBehaviour
{
    [SerializeField]
    private SamplePlayerAttack _samplePlayerAttack = null;

    [SerializeField]
    private SamplePlayerHP _samplePlayerHP = null;

    [SerializeField]
    private SamplePlayerMovement _samplePlayerMovement = null;

    private FiniteStateMachine _samplePlayerFSM = new FiniteStateMachine();

    private SamplePlayerIdleState _samplePlayerIdleState = new SamplePlayerIdleState();
    private SamplePlayerMoveState _samplePlayerMoveState = new SamplePlayerMoveState();
    private SamplePlayerAttackState _samplePlayerAttackState = new SamplePlayerAttackState();
    private SamplePlayerDamagedState _samplePlayerDamagedState = new SamplePlayerDamagedState();
    private SamplePlayerDieState _samplePlayerDieState = new SamplePlayerDieState();

    private EStateIDs _samplePlayerState = EStateIDs.None;

    private void Start()
    {
        _samplePlayerFSM.AddState(EStateIDs.Idle, _samplePlayerIdleState);
        _samplePlayerFSM.AddState(EStateIDs.Move, _samplePlayerMoveState);
        _samplePlayerFSM.AddState(EStateIDs.Attack, _samplePlayerAttackState);
        _samplePlayerFSM.AddState(EStateIDs.Damaged, _samplePlayerDamagedState);
        _samplePlayerFSM.AddState(EStateIDs.Die, _samplePlayerDieState);

        _samplePlayerFSM.InitializeState(EStateIDs.Idle);
        _samplePlayerState = EStateIDs.Idle;
    }

    private void Update()
    {
        _samplePlayerFSM.UpdateState();

        ChangeSamplePlayerState();

        ChangeSamplePlayerFSM();

        // µð¹ö±ë¿ë
        Debug.Log($"SamplePlayerStates.cs _samplePlayerState : {_samplePlayerState}");
    }

    private void ChangeSamplePlayerState()
    {
        if (false == _samplePlayerMovement.IsMoving && false == _samplePlayerAttack.IsAttack && false == _samplePlayerHP.IsDie && false == _samplePlayerHP.IsDamaged)
        {
            _samplePlayerState = EStateIDs.Idle;
        }

        if (_samplePlayerMovement.IsMoving)
        {
            _samplePlayerState = EStateIDs.Move;
        }

        if (_samplePlayerAttack.IsAttack)
        {
            _samplePlayerState = EStateIDs.Attack;
        }

        if (_samplePlayerHP.IsDie)
        {
            _samplePlayerState = EStateIDs.Die;
        }
        else if (_samplePlayerHP.IsDamaged)
        {
            _samplePlayerState = EStateIDs.Damaged;
        }
    }

    private void ChangeSamplePlayerFSM()
    {
        switch (_samplePlayerState)
        {
            case EStateIDs.Attack:
                _samplePlayerFSM.ChangeState(EStateIDs.Attack);
                break;
            case EStateIDs.Damaged:
                _samplePlayerFSM.ChangeState(EStateIDs.Damaged);
                break;
            case EStateIDs.Die:
                _samplePlayerFSM.ChangeState(EStateIDs.Die);
                break;
            case EStateIDs.Idle:
                _samplePlayerFSM.ChangeState(EStateIDs.Idle);
                break;
            case EStateIDs.Move:
                _samplePlayerFSM.ChangeState(EStateIDs.Move);
                break;
            default:
                break;
        }
    }
}
