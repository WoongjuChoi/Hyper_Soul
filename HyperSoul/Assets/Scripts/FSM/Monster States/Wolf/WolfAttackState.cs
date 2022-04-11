using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : BaseState<WolfInformation>
{
    private float _changeAttackAnimationTime = 3f;
    private float _elapsedTime = 0f;

    private const string IS_IDLE = "isIdle";

    public override void EnterState()
    {
        if (EStateIDs.Chase == CreatureInformation.MonsterCurrentState)
        {
            _elapsedTime = _changeAttackAnimationTime;
        }
        else
        {
            _elapsedTime = _changeAttackAnimationTime - 0.5f;
        }

        CreatureInformation.MonsterCurrentState = EStateIDs.Attack;
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

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

        if (_elapsedTime >= _changeAttackAnimationTime)
        {
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

            GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_ATTACK, true);

            _elapsedTime = 0f;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }
}
