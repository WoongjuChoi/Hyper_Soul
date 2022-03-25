using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<EStateIDs, IfiniteState> _states = new Dictionary<EStateIDs, IfiniteState>();

    private IfiniteState _currState;

    private void Update()
    {
        // 디버깅용
        Debug.Log("FiniteStateMachine Update");

        if (null == _currState)
        {
            return;
        }

        _currState.UpdateState();
    }

    public void AddState(EStateIDs index, IfiniteState state)
    {
        // 상태가 이미 있다면 추가시키지 않음
        if (_states.ContainsKey(index))
        {
            return;
        }

        // 상태 추가
        _states.Add(index, state);

        _states[index].InitializeState(this);
    }

    public void ChangeState(EStateIDs index)
    {
        // 현재 다른 상태가 있을 때 Exit
        _currState?.ExitState();

        // 상태를 바꿔줌
        if (_states.ContainsKey(index))
        {
            _currState = _states[index];
            _currState.EnterState();
        }
    }

    public void InitializeState(EStateIDs index)
    {
        // 현재 상태가 있다면 초기화하지 않음
        if (null != _currState)
        {
            return;
        }

        _currState = _states[index];
    }
}
