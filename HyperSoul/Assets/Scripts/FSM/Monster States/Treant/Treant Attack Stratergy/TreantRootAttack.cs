using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRootAttack : MonoBehaviour, ITreantAttack
{
    [SerializeField]
    private GameObject _treantRoot = null;

    private Transform _targetTransform = null;

    private bool _isAttack = false;
    private bool _isTargeting = false;

    public void Attack()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, false);

        StartCoroutine(RootAttack());
    }

    private IEnumerator RootAttack()
    {
        while (true)
        {
            if (false == _isTargeting)
            {
                _targetTransform = gameObject.GetComponent<TreantInformation>().Target.transform;

                _treantRoot.SetActive(true);

                _treantRoot.transform.position = new Vector3(_targetTransform.position.x, _treantRoot.transform.position.y, _targetTransform.position.z);

                _isAttack = true;
                _isTargeting = true;
            }

            yield return new WaitForSeconds(0.2f);

            if (_isAttack)
            {
                Vector3 movetargetVec = (_targetTransform.position - _treantRoot.transform.position).normalized;

                float dotrotateToTarget = Vector3.Dot(Vector3.forward, movetargetVec) * Mathf.Rad2Deg;

                _treantRoot.transform.Rotate(0f, dotrotateToTarget, 0f);

                _isAttack = false;
            }

            gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, true);

            _treantRoot.GetComponent<Animator>().SetTrigger(MonsterAnimatorID.HAS_SPAWN);

            yield return new WaitForSeconds(0.1f);

            gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

            yield return new WaitForSeconds(5f);

            _treantRoot.SetActive(false);

            _isTargeting = false;
        }
    }

    public void StopRootAttack()
    {
        StopCoroutine(RootAttack());
    }
}
