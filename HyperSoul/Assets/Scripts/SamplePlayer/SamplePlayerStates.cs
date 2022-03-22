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

        if (_samplePlayerMovement.IsMoving)
        {
            _samplePlayerState = EStateIDs.Move;
        }
        else
        {
            if (_samplePlayerHP.IsDie)
            {
                _samplePlayerState = EStateIDs.Die;
            }
            else if (_samplePlayerHP.IsDamaged)
            {
                _samplePlayerState = EStateIDs.Damaged;
            }
            else
            {
                _samplePlayerState = EStateIDs.Idle;
            }
        }

        if (_samplePlayerAttack.IsAttack)
        {
            _samplePlayerState = EStateIDs.Attack;
        }

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

        Debug.Log($"_samplePlayerState : {_samplePlayerState}");
    }
}
