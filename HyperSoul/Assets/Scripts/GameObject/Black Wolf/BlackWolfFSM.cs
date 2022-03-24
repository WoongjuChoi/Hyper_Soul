using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWolfFSM : MonoBehaviour
{
    [SerializeField]
    private FiniteStateMachine _blackWolfFSM = null;

    private BlackWolfIdleState _blackWolfIdleState = new BlackWolfIdleState();

    private void Awake()
    {
        _blackWolfFSM.AddState(EStateIDs.Idle, _blackWolfIdleState);

        _blackWolfFSM.InitializeState(EStateIDs.Idle);
    }
}
