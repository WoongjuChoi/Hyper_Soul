using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRotatePositionState : BaseState<TreantInformation>
{
    private Coroutine _rotatePositionCoroutine;

    private bool _isLocatedLeftSide = false;
    private bool _isLocatedRightSide = false;

    private const string IS_LEFT_ROTATE = "isLeftRotate";
    private const string IS_RIGHT_ROTATE = "isRightRotate";

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        _isLocatedLeftSide = false;
        _isLocatedRightSide = false;

        MonsterObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
        MonsterObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        if (CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Attack);

            return;
        }

        if (CreatureInformation.Target.GetComponent<LivingEntity>().IsDead)
        {
            CreatureInformation.IsTargeting = false;

            FiniteStateMachine.ChangeState(EMonsterStateIDs.ReturnPosition);

            return;
        }

        CheckTargetPosition();

        if (null != _rotatePositionCoroutine)
        {
            StopCoroutine(_rotatePositionCoroutine);
        }

        _rotatePositionCoroutine = StartCoroutine(RotatePosition());
    }

    private void CheckTargetPosition()
    {
        Vector3 crossMonsterToTarget = Vector3.Cross(MonsterObject.transform.forward, CreatureInformation.VectorMonsterToTarget);

        if (crossMonsterToTarget.y > 0f)
        {
            _isLocatedLeftSide = false;
            _isLocatedRightSide = true;
        }
        else if (crossMonsterToTarget.y < 0f)
        {
            _isLocatedLeftSide = true;
            _isLocatedRightSide = false;
        }
    }

    private IEnumerator RotatePosition()
    {
        while (false == CreatureInformation.ExistInSight)
        {
            float rotateSpeed = CreatureInformation.RotateSpeed * Time.deltaTime;

            MonsterObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
            MonsterObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);

            if (_isLocatedRightSide)
            {
                MonsterObject.transform.Rotate(0f, rotateSpeed, 0f);
            }
            else if (_isLocatedLeftSide)
            {
                MonsterObject.transform.Rotate(0f, -rotateSpeed, 0f);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
