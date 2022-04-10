using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRootAttack : MonoBehaviour, ITreantAttack
{
    [SerializeField]
    private GameObject _treantRoot = null;

    private Coroutine _rootAttackCoroutine = null;

    private Transform _targetTransform = null;

    private bool _isAttack = false;

    public void Attack()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        if (false == _isAttack)
        {
            _rootAttackCoroutine = StartCoroutine(RootAttack());
        }
    }

    private IEnumerator RootAttack()
    {
        _isAttack = true;

        _targetTransform = gameObject.GetComponent<TreantInformation>().Target.transform;

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, true);

        _treantRoot.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        _treantRoot.transform.position = new Vector3(_targetTransform.position.x, _treantRoot.transform.position.y, _targetTransform.position.z);

        yield return new WaitForSeconds(0.1f);

        _treantRoot.GetComponent<Animator>().SetTrigger(MonsterAnimatorID.HAS_SPAWN);

        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

        yield return new WaitForSeconds(4f);

        _treantRoot.SetActive(false);

        yield return new WaitForSeconds(1f);

        _isAttack = false;
    }

    public void StopAttack()
    {
        _isAttack = false;

        _treantRoot.SetActive(false);

        if (null != _rootAttackCoroutine)
        {
            StopCoroutine(_rootAttackCoroutine);
        }
    }
}
