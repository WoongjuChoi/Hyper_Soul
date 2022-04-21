using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviourPun
{
    [SerializeField]
    private GameObject _gameObject = null;

    private Dictionary<EMonsterStateIDs, IfiniteState> _states = new Dictionary<EMonsterStateIDs, IfiniteState>();
    private IfiniteState _currState;

    private void Update()
    {
        if (null == _currState || false == PhotonNetwork.IsMasterClient)
        {
            return;
        }

        _currState.UpdateState();
    }

    public void AddState(EMonsterStateIDs index, IfiniteState state)
    {
        // ���°� �̹� �ִٸ� �߰���Ű�� ����
        if (_states.ContainsKey(index))
        {
            return;
        }

        // ���� �߰�
        _states.Add(index, state);

        _states[index].InitializeState(_gameObject, this);
    }

    public void ChangeState(EMonsterStateIDs index)
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
}
