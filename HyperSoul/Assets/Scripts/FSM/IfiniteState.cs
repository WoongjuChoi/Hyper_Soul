using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IfiniteState
{
    public void EnterState();

    public void ExitState();

    public void InitializeState(GameObject obj, FiniteStateMachine fsm);

    public void UpdateState();
}
