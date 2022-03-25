using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<EStateIDs, IfiniteState> _states = new Dictionary<EStateIDs, IfiniteState>();

    private IfiniteState _currState;

    private void Update()
    {
        // ������
        Debug.Log("FiniteStateMachine Update");

        if (null == _currState)
        {
            return;
        }

        _currState.UpdateState();
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

        _states[index].InitializeState(this);
    }

    public void ChangeState(EStateIDs index)
    {
        // ���� �ٸ� ���°� ���� �� Exit
        _currState?.ExitState();

        // ���¸� �ٲ���
        if (_states.ContainsKey(index))
        {
            _currState = _states[index];
            _currState.EnterState();
        }
    }

    public void InitializeState(EStateIDs index)
    {
        // ���� ���°� �ִٸ� �ʱ�ȭ���� ����
        if (null != _currState)
        {
            return;
        }

        _currState = _states[index];
    }
}
