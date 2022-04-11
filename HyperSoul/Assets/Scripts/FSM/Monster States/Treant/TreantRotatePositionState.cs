using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRotatePositionState : BaseState<TreantInformation>
{
    private bool _isLocatedLeftSide = false;
    private bool _isLocatedRightSide = false;

    private const string IS_LEFT_ROTATE = "isLeftRotate";
    private const string IS_RIGHT_ROTATE = "isRightRotate";

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.RotatePosition;
    }

    public override void ExitState()
    {
        _isLocatedLeftSide = false;
        _isLocatedRightSide = false;

        GameObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
        GameObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        if (CreatureInformation.ExistInSight)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Attack);

            return;
        }

        // 외적을 이용해 타겟의 방향을 확인
        CheckTargetPosition();

        // 그 방향으로 회전 이동
        StartCoroutine(RotatePosition());
    }

    private void CheckTargetPosition()
    {
        Vector3 crossMonsterToTarget = Vector3.Cross(GameObject.transform.forward, CreatureInformation.VectorMonsterToTarget);

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

            GameObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
            GameObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);

            if (_isLocatedRightSide)
            {
                GameObject.transform.Rotate(0f, rotateSpeed, 0f);
            }
            else if (_isLocatedLeftSide)
            {
                GameObject.transform.Rotate(0f, -rotateSpeed, 0f);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
