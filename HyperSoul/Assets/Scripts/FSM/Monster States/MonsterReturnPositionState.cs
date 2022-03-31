using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnPositionState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _distance = 0f;

    private int _increaseHealing = 0;

    private const string IS_WALK = "isWalk";

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.ReturnPosition;

        _gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, true);
    }

    public void ExitState()
    {
        _monsterInfo.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_IDLE);

        _monsterInfo.IsTargeting = false;

        _monsterInfo.MonsterChaser.ResetPath();

        _monsterInfo.MonsterChaser.IsActive = false;

        _gameObject.transform.position = _monsterInfo.InitializePosition.position;

        _gameObject.transform.rotation = Quaternion.identity;

        Quaternion monsterInitializeDirection = Quaternion.Euler(0f, _monsterInfo.MonsterSpawnDirection, 0f);

        _gameObject.transform.rotation = monsterInitializeDirection;

        _monsterInfo.GetComponentInChildren<Animator>().SetBool(IS_WALK, false);

        _gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_CHASE, false);
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        _distance = (_gameObject.transform.position - _monsterInfo.InitializePosition.position).magnitude;

        if (_distance <= 2f)
        {
            _monsterInfo.GetComponentInChildren<Animator>().SetBool(IS_WALK, true);
        }
        
        if (_distance <= 0.5f)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        // 현재 체력이 최대 체력이 아니라면 서서히 증가
        if (_monsterInfo.MonsterCurrentHP >= _monsterInfo.MonsterMaxHP)
        {
            _monsterInfo.MonsterCurrentHP = _monsterInfo.MonsterMaxHP;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            _monsterInfo.MonsterCurrentHP += _increaseHealing;
        }

        _monsterInfo.MonsterChaser.IsTarget = _monsterInfo.InitializePosition;

        _monsterInfo.MonsterChaser.IsActive = true;
    }
}
