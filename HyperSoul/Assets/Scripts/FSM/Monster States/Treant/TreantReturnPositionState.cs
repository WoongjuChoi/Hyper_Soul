using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantReturnPositionState : BaseState<TreantInformation>
{
    private bool _isLocatedLeftSide = false;
    private bool _isLocatedRightSide = false;

    private int _increaseHealing = 0;

    private const string IS_LEFT_ROTATE = "isLeftRotate";
    private const string IS_RIGHT_ROTATE = "isRightRotate";

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.ReturnPosition;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;

        _isLocatedLeftSide = false;
        _isLocatedRightSide = false;

        GameObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
        GameObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);
    }

    public override void UpdateState()
    {
        // ȸ�� �� �ϸ� Idle ���·� ���ư�
        if (CreatureInformation.OriginVec == GameObject.transform.forward)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        // ���� ü���� �ִ� ü���� �ƴ϶�� ������ ����
        if (CreatureInformation.CurHp >= CreatureInformation.MaxHp)
        {
            CreatureInformation.CurHp = CreatureInformation.MaxHp;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            CreatureInformation.CurHp += _increaseHealing;
        }

        // ���� �ٶ󺸴� ������ Ȯ��
        CheckCurrentPosition();

        // �� �������� ȸ�� �̵�
        StartCoroutine(ReturnPosition());
    }

    private void CheckCurrentPosition()
    {
        Vector3 crossCurrentVec = Vector3.Cross(CreatureInformation.OriginVec, GameObject.transform.forward);

        if (crossCurrentVec.y > 0f)
        {
            _isLocatedLeftSide = true;
            _isLocatedRightSide = false;
        }
        else if (crossCurrentVec.y < 0f)
        {
            _isLocatedLeftSide = false;
            _isLocatedRightSide = true;
        }
    }

    private IEnumerator ReturnPosition()
    {
        while (CreatureInformation.OriginVec != GameObject.transform.forward)
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
