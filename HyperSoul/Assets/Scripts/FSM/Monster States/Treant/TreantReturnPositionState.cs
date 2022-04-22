using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantReturnPositionState : BaseState<TreantInformation>
{
    private Coroutine _returnPositionCoroutine;

    private bool _isDoneHealing = false;
    private bool _isLocatedLeftSide = false;
    private bool _isLocatedRightSide = false;
    private int _increaseHealing = 0;

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.ReturnPosition;
    }

    public override void ExitState()
    {
        _increaseHealing = 0;

        _isDoneHealing = false;
        _isLocatedLeftSide = false;
        _isLocatedRightSide = false;

        MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_LEFT_ROTATE, _isLocatedLeftSide);
        MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RIGHT_ROTATE, _isLocatedRightSide);
    }

    public override void UpdateState()
    {
        if (CreatureInformation.OriginVec == MonsterObject.transform.forward)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Idle);

            return;
        }

        IncreaseHealing();

        CheckCurrentPosition();

        if (null != _returnPositionCoroutine)
        {
            StopCoroutine(_returnPositionCoroutine);
        }

        _returnPositionCoroutine = StartCoroutine(ReturnPosition());
    }

    private void CheckCurrentPosition()
    {
        Vector3 crossCurrentVec = Vector3.Cross(CreatureInformation.OriginVec, MonsterObject.transform.forward);

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
        while (CreatureInformation.OriginVec != MonsterObject.transform.forward)
        {
            float dotMonsterVec = Vector3.Dot(CreatureInformation.OriginVec, MonsterObject.transform.forward);

            if (dotMonsterVec >= 0.95)
            {
                MonsterObject.transform.forward = CreatureInformation.OriginVec;

                yield break;
            }

            float rotateSpeed = CreatureInformation.RotateSpeed * Time.deltaTime;

            MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_LEFT_ROTATE, _isLocatedLeftSide);
            MonsterObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RIGHT_ROTATE, _isLocatedRightSide);

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

    private void IncreaseHealing()
    {
        if (false == _isDoneHealing)
        {
            if (CreatureInformation.CurHp >= CreatureInformation.MaxHp)
            {
                CreatureInformation.CurHp = CreatureInformation.MaxHp;
                _increaseHealing = 0;
                CreatureInformation.SetMonsterHp(CreatureInformation.CurHp);
                _isDoneHealing = true;
            }
            else
            {
                float increaseHealing = Random.Range(0f, 1f);
                _increaseHealing += (int)Mathf.Round(increaseHealing * 10);
                CreatureInformation.CurHp += _increaseHealing;
                CreatureInformation.SetMonsterHp(CreatureInformation.CurHp);
            }
        }
    }
}
