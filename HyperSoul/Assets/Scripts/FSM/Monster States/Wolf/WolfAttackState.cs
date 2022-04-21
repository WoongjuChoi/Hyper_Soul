using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : BaseState<WolfInformation>
{
    [SerializeField]
    private GameObject _endPoint = null;

    private bool _isAttack = false;
    private float _distance = 0f;

    private const float MOVE_SPEED = 10f;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Attack;
        _distance = (_endPoint.transform.position - CreatureInformation.AttackCollider.transform.position).magnitude;
    }

    public override void ExitState()
    {
        _isAttack = false;
        _distance = (_endPoint.transform.position - CreatureInformation.AttackCollider.transform.position).magnitude;
        CreatureInformation.AttackCollider.SetActive(false);
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_ATTACK, false);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.Target.GetComponent<LivingEntity>().IsDead)
        {
            CreatureInformation.IsTargeting = false;
            FiniteStateMachine.ChangeState(EMonsterStateIDs.ReturnPosition);

            return;
        }

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        if (false == CreatureInformation.IsWithinAttackRange)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Chase);

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

        if (Physics.Raycast(raycastOriginVec, MonsterObject.transform.forward, out hit, 100f))
        {
            if (CreatureInformation.Target.layer != hit.collider.gameObject.layer)
            {
                MonsterObject.transform.LookAt(CreatureInformation.Target.transform);
            }
        }
        else
        {
            MonsterObject.transform.LookAt(CreatureInformation.Target.transform);
        }

        yield return new WaitForSeconds(0.1f);

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_ATTACK, true);

        yield return new WaitForSeconds(0.1f);

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_ATTACK, false);

        if (PhotonNetwork.IsMasterClient)
        {
            CreatureInformation.AttackCollider.SetActive(true);
        }

        while (_distance > 1f)
        {
            CreatureInformation.AttackCollider.transform.position += MOVE_SPEED * Time.deltaTime * MonsterObject.transform.forward;
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
