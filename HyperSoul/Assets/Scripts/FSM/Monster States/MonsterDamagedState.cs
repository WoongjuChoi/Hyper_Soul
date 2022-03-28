using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class MonsterDamagedState : IfiniteState
{
    private GameObject _gameObject = null;

    private MonsterInfomations _monsterInfo = null;

    private FiniteStateMachine _finiteStateMachine = null;

    private Vector3 _lookAtTargetVec = Vector3.zero;

    private Vector3 _rayCastOriginVec = Vector3.zero;

    public void EnterState()
    {
        _monsterInfo = _gameObject.GetComponent<MonsterInfomations>();

        _monsterInfo.MonsterCurrentState = EStateIDs.Damaged;

        _gameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_DAMAGED);

        _monsterInfo.IsDamaged = false;

        _rayCastOriginVec = _gameObject.transform.position + new Vector3(0f, 5f, 0f);

        // 수정 필요 : 현재 Damaged 상태일 때 플레이어의 위치를 가져옴
        // 탄환이 발사됐을 때의 플레이어의 위치를 가져와야함
        _lookAtTargetVec = _monsterInfo.Target.transform.position - _rayCastOriginVec;
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
        _monsterInfo.MonsterHP -= 5;

        // HP <= 0 이면 Die 상태
        if (_monsterInfo.MonsterHP <= 0)
        {
            _finiteStateMachine.ChangeState(EStateIDs.Die);
        }

        // 레이케스트 쐈을 때
            // 적이 있을때
                // 공격 범위 안이면 Attck 상태
                // else Chase 상태
            // 적이 없다면 Alert 상태

        RaycastHit hit;

        if (Physics.Raycast(_rayCastOriginVec, _lookAtTargetVec, out hit, _lookAtTargetVec.magnitude))
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
        else
        {
            _finiteStateMachine.ChangeState(EStateIDs.Alert);
        }
    }
}
