﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerIdleState : IfiniteState
{
    public void OnEnter()
    {
        Debug.Log("SamplePlayerIdleState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerIdleState OnExit");
    }

    public void OnInitialize(FiniteStateMachine fsm)
    {
        Debug.Log("SamplePlayerIdleState OnInitialize");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerIdleState OnUpdate");
    }
}