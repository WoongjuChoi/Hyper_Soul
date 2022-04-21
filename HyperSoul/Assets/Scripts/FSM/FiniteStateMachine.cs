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
        // 상태가 이미 있다면 추가시키지 않음
        if (_states.ContainsKey(index))
        {
            return;
        }

        // 상태 추가
        _states.Add(index, state);

        _states[index].InitializeState(_gameObject, this);
    }

    public void ChangeState(EMonsterStateIDs index)
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
}
