using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class MonsterDamagedState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private Vector3 _lookAtTargetVec = Vector3.zero;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Damaged;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DAMAGED);

        _monsterInfo.IsDamaged = false;

        _lookAtTargetVec = _monsterInfo.Target.transform.position - _gameObject.transform.position;
    }

    public void ExitState()
    {
    }

    public void InitializeState(GameObject obj, FiniteStateMachine fsm)
    {
        _gameObject = obj;

        _finiteStateMachine = fsm;
    }

    public void UpdateState()
    {
        // 데미지 받고
        //Debug.Log("데미지 입음");

        // 레이케스트 쐈을 때
            // 적이 있을때
                // 공격 범위 안이면 Attck 상태
                // else Chase 상태
            // 적이 없다면 Alert 상태

        RaycastHit hit;

        // 문제 => hit.collider.gameObject가 왜 몬스터인가?
        if (Physics.Raycast(_gameObject.transform.position, _lookAtTargetVec, out hit, _lookAtTargetVec.magnitude))
        {
            if (_monsterInfo.Target == hit.collider.gameObject)
            {
                if (_monsterInfo.IsWithinAttackRange)
                {
                    _finiteStateMachine.ChangeState(EStateIDs.Attack);
                }
                else
                {
                    _finiteStateMachine.ChangeState(EStateIDs.Chase);
                }
            }
            else
            {
                _finiteStateMachine.ChangeState(EStateIDs.Alert);
            }
        }
    }
}
