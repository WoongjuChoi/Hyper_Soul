using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _changeRestingAnimationTime = 5f;
    private float _elapsedTime = 0f;

    private int _increaseHealing = 0;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Idle;
    }

    public void ExitState()
    {
        _elapsedTime = 0f;

        _increaseHealing = 0;
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        Vector3 InitializeAngle = new Vector3(0f, _monsterInfo.MonsterSpawnDirection, 0f);

        if (InitializeAngle != _gameObject.transform.eulerAngles)
        {
            _gameObject.transform.rotation = Quaternion.identity;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, _monsterInfo.MonsterSpawnDirection, 0f);

            _gameObject.transform.rotation = monsterInitializeDirection;
        }

        if (_monsterInfo.IsDamaged)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Damaged);

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

        if (-1 == _elapsedTime)
        {
            return;
        }

        ChangeRestingAnimation();
    }

    private void ChangeRestingAnimation()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeRestingAnimationTime)
        {
            _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_RESTING);

            _elapsedTime = -1f;
        }
    }
}
