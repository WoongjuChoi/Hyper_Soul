using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _monsterDieTime = 2f;
    private float _elapsedTime = 0f;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Die;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DIE);
    }

    public void ExitState()
    {
        _elapsedTime = 0f;

        _gameObject.transform.eulerAngles = Vector3.zero;
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        if (-1f == _elapsedTime)
        {
            return;
        }

        MonsterDie();
    }

    private void MonsterDie()
    {
        if (_elapsedTime >= _monsterDieTime)
        {
            _monsterInfo.IsDie = true;

            _elapsedTime = -1f;

            _gameObject.SetActive(false);

            return;
        }

        _elapsedTime += Time.deltaTime;
    }
}
