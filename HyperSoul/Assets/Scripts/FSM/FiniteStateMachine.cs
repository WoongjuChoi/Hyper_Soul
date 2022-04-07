using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<EStateIDs, IfiniteState> _states;

    private IfiniteState _currState;

    private void Update()
    {
<<<<<<< Updated upstream
        if (_currState == null)
=======
        // ������
        Debug.Log("FiniteStateMachine Update");

        if (null == _currState)
>>>>>>> Stashed changes
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
        _currState.OnInitialize(this);
        _states.Add(index, state);
    }

    public void ChangeState(EStateIDs index)
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
