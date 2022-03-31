using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private float _changeAttackAnimationTime = 3f;
    private float _elapsedTime = 0f;

    private const string IS_IDLE = "isIdle";

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        if (EStateIDs.Chase == _monsterInfo.MonsterCurrentState)
        {
            _elapsedTime = _changeAttackAnimationTime;
        }
        else if (EStateIDs.Damaged == _monsterInfo.MonsterCurrentState)
        {
            _gameObject.GetComponentInChildren<Animator>().SetBool(IS_IDLE, true);
        }

        _monsterInfo.MonsterCurrentState = EStateIDs.Attack;
    }

    public void ExitState()
    {
        _elapsedTime = 0f;

        _gameObject.GetComponentInChildren<Animator>().SetBool(IS_IDLE, false);
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        if (_monsterInfo.IsDamaged)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (false == _monsterInfo.IsWithinAttackRange)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Chase);

            return;
        }

        if (_elapsedTime >= _changeAttackAnimationTime)
        {
            Vector3 raycastOriginVec = _monsterInfo.MonsterRayPoint.position + new Vector3(0f, 0f, 1f);

            RaycastHit hit;

            if (Physics.Raycast(raycastOriginVec, _gameObject.transform.forward, out hit, 100f))
            {
                if (_monsterInfo.Target.layer != hit.collider.gameObject.layer)
                {
                    _gameObject.transform.LookAt(_monsterInfo.Target.transform);
                }
            }
            else
            {
                _gameObject.transform.LookAt(_monsterInfo.Target.transform);
            }

            _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_ATTACK);

            _elapsedTime = 0f;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }
}
