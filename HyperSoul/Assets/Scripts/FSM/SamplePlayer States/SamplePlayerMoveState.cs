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

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerMoveState OnUpdate");
    }
}
