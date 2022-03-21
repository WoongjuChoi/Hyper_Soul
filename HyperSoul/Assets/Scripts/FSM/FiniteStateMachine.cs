using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<EStateID, IfiniteState> _states;

    private IfiniteState _currState;

    private void Update()
    {
        if (_currState == null)
        {
            return;
        }

        _currState.OnUpdate();
    }

    public void AddState(EStateID index, IfiniteState state)
    {
        // 상태가 이미 있다면 추가시키지 않음
        if (_states.ContainsKey(index))
        {
            return;
        }

        // 상태 추가
        _currState.OnInitialize(this);
        _states.Add(index, state);
    }

    public void ChangeState(EStateID index)
    {
        // 현재 상태가 있을 때 Exit
        _currState?.OnExit();

        // 상태를 바꿔줌
        if (_states.ContainsKey(index))
        {
            _currState = _states[index];
            _currState.OnEnter();
        }
    }
}
