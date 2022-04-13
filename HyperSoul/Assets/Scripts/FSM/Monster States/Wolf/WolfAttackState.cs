using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : BaseState<WolfInformation>
{
    [SerializeField]
    private GameObject _endPoint = null;

    private bool _isAttack = false;

    private float _distance = 0f;

    private const float MOVE_SPEED = 3f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.Attack;

        _distance = (_endPoint.transform.position - CreatureInformation.AttackCollider.transform.position).magnitude;
    }

    public override void ExitState()
    {
        _isAttack = false;

        _distance = (_endPoint.transform.position - CreatureInformation.AttackCollider.transform.position).magnitude;

        CreatureInformation.AttackCollider.SetActive(false);

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ATTACK, false);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (false == CreatureInformation.IsWithinAttackRange)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Chase);

            return;
        }

        if (false == _isAttack)
        {
            StartCoroutine(WolfAttack());
        }
    }

    private IEnumerator WolfAttack()
    {
        _isAttack = true;

        Vector3 raycastOriginVec = CreatureInformation.MonsterRayPoint.position + new Vector3(0f, 0f, 1f);

        RaycastHit hit;

        if (Physics.Raycast(raycastOriginVec, GameObject.transform.forward, out hit, 100f))
        {
            if (CreatureInformation.Target.layer != hit.collider.gameObject.layer)
            {
                GameObject.transform.LookAt(CreatureInformation.Target.transform);
            }
        }
        else
        {
            GameObject.transform.LookAt(CreatureInformation.Target.transform);
        }

        yield return new WaitForSeconds(0.1f);

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ATTACK, true);

        CreatureInformation.AttackCollider.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ATTACK, false);

        while (_distance > 1f)
        {
            CreatureInformation.AttackCollider.transform.position += MOVE_SPEED * Time.deltaTime * GameObject.transform.forward;

            _distance = (_endPoint.transform.position - CreatureInformation.AttackCollider.transform.position).magnitude;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        CreatureInformation.AttackCollider.transform.position = CreatureInformation.StartPoint.transform.position;

        _distance = (_endPoint.transform.position - CreatureInformation.StartPoint.transform.position).magnitude;
        
        CreatureInformation.AttackCollider.SetActive(false);

        yield return new WaitForSeconds(3f);

        _isAttack = false;
    }
}
