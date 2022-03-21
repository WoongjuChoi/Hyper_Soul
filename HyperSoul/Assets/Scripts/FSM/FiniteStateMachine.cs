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
        // ���°� �̹� �ִٸ� �߰���Ű�� ����
        if (_states.ContainsKey(index))
        {
            return;
        }

        // ���� �߰�
        _currState.OnInitialize(this);
        _states.Add(index, state);
    }

    public void ChangeState(EStateID index)
    {
        // ���� ���°� ���� �� Exit
        _currState?.OnExit();

        // ���¸� �ٲ���
        if (_states.ContainsKey(index))
        {
            _currState = _states[index];
            _currState.OnEnter();
        }
    }
}
