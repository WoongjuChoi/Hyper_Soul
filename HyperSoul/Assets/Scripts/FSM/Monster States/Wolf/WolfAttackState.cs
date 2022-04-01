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
        if (EStateIDs.Chase == base.CreatureInfomation.MonsterCurrentState)
        {
            _elapsedTime = _changeAttackAnimationTime;
        }
        else if (EStateIDs.Damaged == base.CreatureInfomation.MonsterCurrentState)
        {
            base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_IDLE, true);
        }

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Attack;
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_IDLE, false);
    }

    public override void UpdateState()
    {
        if (base.CreatureInfomation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (false == base.CreatureInfomation.IsWithinAttackRange)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Chase);

            return;
        }

        if (_elapsedTime >= _changeAttackAnimationTime)
        {
            Vector3 raycastOriginVec = base.CreatureInfomation.MonsterRayPoint.position + new Vector3(0f, 0f, 1f);

            RaycastHit hit;

            if (Physics.Raycast(raycastOriginVec, base.GameObject.transform.forward, out hit, 100f))
            {
                if (base.CreatureInfomation.Target.layer != hit.collider.gameObject.layer)
                {
                    base.GameObject.transform.LookAt(base.CreatureInfomation.Target.transform);
                }
            }
            else
            {
                base.GameObject.transform.LookAt(base.CreatureInfomation.Target.transform);
            }

            base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_ATTACK);

            _elapsedTime = 0f;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }
}
