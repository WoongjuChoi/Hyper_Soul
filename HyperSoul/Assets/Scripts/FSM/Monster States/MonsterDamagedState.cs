using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class MonsterDamagedState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private Vector3 _lookAtTargetVec = Vector3.zero;

    private Vector3 _raycastOriginVec = Vector3.zero;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Damaged;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DAMAGED);

        _raycastOriginVec = _monsterInfo.CollisionVec;

        _lookAtTargetVec = _monsterInfo.LookAtTargetVec;
    }

    public void ExitState()
    {
        _monsterInfo.IsDamaged = false; 
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        // 데미지 받고 (수정 필요)
        _monsterInfo.MonsterCurrentHP -= 50;

        // HP <= 0 이면 Die 상태
        if (_monsterInfo.MonsterCurrentHP <= 0)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Die);

            return;
        }

        // 적을 한번 타겟팅 하면 
        if (_monsterInfo.IsTargeting)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Chase);

            return;
        }

        // 레이케스트 쐈을 때
            // 적이 있을때
                // 공격 범위 안이면 Attck 상태
                // else Chase 상태
            // 적이 없다면 Alert 상태
        // 레이케스트 아무것도 안맞았다면 Alert 상태

        RaycastHit hit;

        if (Physics.Raycast(_raycastOriginVec, _lookAtTargetVec, out hit, 1000f))
        {
            if (_monsterInfo.Target.layer == hit.collider.gameObject.layer)
            {
                _monsterInfo.MonsterChaser.IsTarget = _monsterInfo.Target.transform;

                _monsterInfo.IsTargeting = true;

                if (_monsterInfo.IsWithinAttackRange)
                {
                    _finiteStateMachine.ChangeState(EStateIDs.Attack);

                    return;
                }
                else
                {
                    _finiteStateMachine.ChangeState(EStateIDs.Chase);

                    return;
                }
            }
            else
            {
                _finiteStateMachine.ChangeState(EStateIDs.Alert);

                return;
            }
        }
        else
        {
            _finiteStateMachine.ChangeState(EStateIDs.Alert);

            return;
        }
    }
}
