using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerDieState : IfiniteState
{
    public void OnEnter()
    {
        Debug.Log("SamplePlayerDieState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerDieState OnExit");
    }

    public void OnInitialize(FiniteStateMachine fsm)
    {
        Debug.Log("SamplePlayerDieState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerDieState OnUpdate");
    }
}
