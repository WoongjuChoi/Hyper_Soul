using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<EStateIDs, IfiniteState> _states = new Dictionary<EStateIDs, IfiniteState>();

    private IfiniteState _currState;

    private void Update()
    {
        if (null == _currState)
        {
            return;
        }

        _currState.OnUpdate();
    }

    public void AddState(EStateIDs index, IfiniteState state)
    {
        // ���°� �̹� �ִٸ� �߰���Ű�� ����
        if (_states.ContainsKey(index))
        {
            return;
        }

        // ���� �߰�
        _states.Add(index, state);
        _states[index].OnInitialize(this);
    }

    public void ChangeState(EStateIDs index)
    {
        // ���� ���°� �ٲ� ���°� ���ٸ� ����X
        if (_states[index] == _currState)
        {
            return;
        }

        // ���� �ٸ� ���°� ���� �� Exit
        _currState?.OnExit();

        // ���¸� �ٲ���
        if (_states.ContainsKey(index))
        {
            _currState = _states[index];
            _currState.OnEnter();
        }
    }

    public void InitializeState(EStateIDs index)
    {
        // ���� ���°� �ִٸ� ����X
        if (null != _currState)
        {
            return;
        }

        _currState = _states[index];

        // ������
        Debug.Log($"_currState : {_states[index].GetType()}");
    }
}
