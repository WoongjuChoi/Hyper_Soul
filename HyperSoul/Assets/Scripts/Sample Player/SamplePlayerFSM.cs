using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerFSM : MonoBehaviour
{
    [SerializeField]
    private FiniteStateMachine _samplePlayerFSM = null;

    private SamplePlayerIdleState _samplePlayerIdleState = new SamplePlayerIdleState();
    private SamplePlayerMoveState _samplePlayerMoveState = new SamplePlayerMoveState();
    private SamplePlayerAttackState _samplePlayerAttackState = new SamplePlayerAttackState();
    private SamplePlayerDamagedState _samplePlayerDamagedState = new SamplePlayerDamagedState();
    private SamplePlayerDieState _samplePlayerDieState = new SamplePlayerDieState();

    private void Awake()
    {
        _samplePlayerFSM.AddState(EStateIDs.Idle, _samplePlayerIdleState);
        _samplePlayerFSM.AddState(EStateIDs.Move, _samplePlayerMoveState);
        _samplePlayerFSM.AddState(EStateIDs.Attack, _samplePlayerAttackState);
        _samplePlayerFSM.AddState(EStateIDs.Damaged, _samplePlayerDamagedState);
        _samplePlayerFSM.AddState(EStateIDs.Die, _samplePlayerDieState);

        _samplePlayerFSM.InitializeState(EStateIDs.Idle);
    }
}
