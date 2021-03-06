using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantStompAttack : MonoBehaviourPun, ITreantAttack
{
    [SerializeField]
    private GameObject _attackCollider = null;
    [SerializeField]
    private GameObject _startPoint = null;
    [SerializeField]
    private GameObject _endPoint = null;
    [SerializeField]
    private Animator _animator = null;

    private Coroutine _stompAttackCoroutine = null;
    private bool _isAttack = false;
    private float _distance = 0f;
    private const float MOVE_SPEED = 5f;

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _attackCollider.SetActive(false);
            photonView.RPC(nameof(StompObjectActive), RpcTarget.Others, false);
        }
    }

    private void Start()
    {
        _distance = (_endPoint.transform.position - _startPoint.transform.position).magnitude;
    }

    public void Attack()
    {
        _animator.SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

        if (false == _isAttack)
        {
            _stompAttackCoroutine = StartCoroutine(StompAttack());
        }
    }

    private IEnumerator StompAttack()
    {
        _isAttack = true;

        yield return new WaitForSeconds(0.1f);

        _animator.SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, true);

        yield return new WaitForSeconds(0.5f);

        _animator.SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        if (PhotonNetwork.IsMasterClient)
        {
            _attackCollider.SetActive(true);
            photonView.RPC(nameof(StompObjectActive), RpcTarget.Others, true);
        }

        while (_distance > 1f)
        {
            _attackCollider.transform.position += MOVE_SPEED * Time.deltaTime * gameObject.transform.up;
            _distance = (_endPoint.transform.position - _attackCollider.transform.position).magnitude;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        _attackCollider.transform.position = _startPoint.transform.position;
        _distance = (_endPoint.transform.position - _startPoint.transform.position).magnitude;

        if (PhotonNetwork.IsMasterClient)
        {
            _attackCollider.SetActive(false);
            photonView.RPC(nameof(StompObjectActive), RpcTarget.Others, false);
        }

        yield return new WaitForSeconds(3f);

        _isAttack = false;
    }

    public void StopAttack()
    {
        _isAttack = false;
        _distance = (_endPoint.transform.position - _startPoint.transform.position).magnitude;
        _attackCollider.transform.position = _startPoint.transform.position;
        _attackCollider.SetActive(false);

        if (null != _stompAttackCoroutine)
        {
            StopCoroutine(_stompAttackCoroutine);
        }
    }

    [PunRPC]
    public void StompObjectActive(bool b)
    {
        _attackCollider.SetActive(b);
    }
}
