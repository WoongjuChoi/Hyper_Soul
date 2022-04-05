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
        base.CreatureInformation.MonsterCurrentState = EStateIDs.ReturnPosition;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;

        _isLocatedLeftSide = false;
        _isLocatedRightSide = false;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
        base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);
    }

    public override void UpdateState()
    {
        // ȸ�� �� �ϸ� Idle ���·� ���ư�
        if (base.CreatureInformation.OriginVec == base.GameObject.transform.forward)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        // ���� ü���� �ִ� ü���� �ƴ϶�� ������ ����
        if (base.CreatureInformation.MonsterCurrentHP >= base.CreatureInformation.MonsterMaxHP)
        {
            base.CreatureInformation.MonsterCurrentHP = base.CreatureInformation.MonsterMaxHP;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            base.CreatureInformation.MonsterCurrentHP += _increaseHealing;
        }

        // ���� �ٶ󺸴� ������ Ȯ��
        CheckCurrentPosition();

        // �� �������� ȸ�� �̵�
        StartCoroutine(ReturnPosition());
    }

    private void CheckCurrentPosition()
    {
        Vector3 crossCurrentVec = Vector3.Cross(base.CreatureInformation.OriginVec, base.GameObject.transform.forward);

        if (crossCurrentVec.y > 0f)
        {
            _isLocatedLeftSide = true;
            _isLocatedRightSide = false;

            Debug.Log("���� ��");
        }
        else if (crossCurrentVec.y < 0f)
        {
            _isLocatedLeftSide = false;
            _isLocatedRightSide = true;

            Debug.Log("������ ��");
        }
    }

    private IEnumerator ReturnPosition()
    {
        while (base.CreatureInformation.OriginVec != base.GameObject.transform.forward)
        {
            float rotateSpeed = base.CreatureInformation.RotateSpeed * Time.deltaTime;

            base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_LEFT_ROTATE, _isLocatedLeftSide);
            base.GameObject.GetComponentInChildren<Animator>().SetBool(IS_RIGHT_ROTATE, _isLocatedRightSide);

            if (_isLocatedRightSide)
            {
                base.GameObject.transform.Rotate(0f, rotateSpeed, 0f);
            }
            else if (_isLocatedLeftSide)
            {
                base.GameObject.transform.Rotate(0f, -rotateSpeed, 0f);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
