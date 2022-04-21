using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : MonoBehaviour, IfiniteState where T : class
{
    public GameObject MonsterObject { get; private set; }
    public FiniteStateMachine FiniteStateMachine { get; private set; }
    public T CreatureInformation { get; set; }

    public virtual void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        MonsterObject = obj;
        CreatureInformation = MonsterObject.GetComponent<T>();
        FiniteStateMachine = fsm;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();
}
