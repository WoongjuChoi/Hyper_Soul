﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class SamplePlayerDamagedState : IfiniteState
{
    public void OnEnter()
    {
        Debug.Log("SamplePlayerDamagedState OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("SamplePlayerDamagedState OnExit");
    }

    public void OnUpdate()
    {
        Debug.Log("SamplePlayerDamagedState OnUpdate");
    }
}
