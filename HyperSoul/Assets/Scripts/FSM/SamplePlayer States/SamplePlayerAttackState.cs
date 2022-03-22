using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerAttackState : IfiniteState
{
    public void OnEnter()
    {
        Debug.Log("SamplePlayerAttackState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerAttackState OnExit");
    }

    public void OnInitialize(FiniteStateMachine fsm)
    {
        Debug.Log("SamplePlayerAttackState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerAttackState OnUpdate");
    }
}
