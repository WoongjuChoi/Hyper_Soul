using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerMoveState : IfiniteState
{
    public void OnEnter()
    {
        Debug.Log("SamplePlayerMoveState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerMoveState OnExit");
    }

    public void OnInitialize(FiniteStateMachine fsm)
    {
        Debug.Log("SamplePlayerMoveState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerMoveState OnUpdate");
    }
}
