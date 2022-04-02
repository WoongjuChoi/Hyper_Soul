using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : IfiniteState where T : class
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private T _creatureInfo = null;

    public GameObject GameObject { get { return _gameObject; } }
    public FiniteStateMachine FiniteStateMachine { get { return _finiteStateMachine; } }
    public T CreatureInformation { get { return _creatureInfo; } set { _creatureInfo = value; } }

    public abstract void EnterState();

    public abstract void ExitState();

    public virtual void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _creatureInfo = _gameObject.GetComponent<T>();

        _finiteStateMachine = fsm;
    }

    public abstract void UpdateState();
}
