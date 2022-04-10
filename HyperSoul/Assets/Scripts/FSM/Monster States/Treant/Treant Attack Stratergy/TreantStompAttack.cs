using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantStompAttack : MonoBehaviour, ITreantAttack
{
    private Coroutine _stompAttackCoroutine = null;

    private bool _isAttack = false;

    public void Attack()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

        if (false == _isAttack)
        {
            _stompAttackCoroutine = StartCoroutine(StompAttack());
        }
    }

    private IEnumerator StompAttack()
    {
        _isAttack = true;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, true);

        yield return new WaitForSeconds(0.3f);

        gameObject.GetComponent<TreantInformation>().StompAttackArea.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        gameObject.GetComponent<TreantInformation>().StompAttackArea.SetActive(false);

        yield return new WaitForSeconds(3f);

        _isAttack = false;
    }

    public void StopAttack()
    {
        _isAttack = false;

        if (null != _stompAttackCoroutine)
        {
            StopCoroutine(_stompAttackCoroutine);
        }
    }
}
