﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IfiniteState
{
    public void OnEnter();

    public void OnExit();

    public void OnInitialize(FiniteStateMachine fsm);

    public void OnUpdate();
}