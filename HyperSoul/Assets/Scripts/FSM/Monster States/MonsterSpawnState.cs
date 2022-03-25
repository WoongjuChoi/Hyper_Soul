using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnState :IfiniteState
{
    private GameObject _gameObject = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _elapsedTime = 0f;

    public void EnterState()
    {
        Debug.Log("MonsterSpawnState EnterState");
    }

    public void ExitState()
    {
        Debug.Log("MonsterSpawnState ExitState");
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;

        Debug.Log("MonsterSpawnState InitializeState");
    }

    public void UpdateState()
    {
        _elapsedTime += Time.deltaTime;

        Debug.Log($"_elapsedTime : {_elapsedTime}");

        if (_elapsedTime >= _gameObject.GetComponent<MonsterInfomations>().MonsterInvincibleTime)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);
        }
        
        Debug.Log("MonsterSpawnState UpdateState");
    }
}
