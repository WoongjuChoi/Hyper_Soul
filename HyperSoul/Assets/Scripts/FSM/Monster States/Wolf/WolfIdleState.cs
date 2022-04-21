using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfIdleState : BaseState<WolfInformation>
{
    private bool _playAnimation = false;
    private bool _isReturnPosition = false;

    private float _changeRestingAnimationTime = 5f;
    private float _elapsedTime = 0f;

    private int _increaseHealing = 0;

    private bool _isDoneHealing = false;

    public override void EnterState()
    {
        if (EMonsterStateIDs.ReturnPosition == CreatureInformation.MonsterCurrentState)
        {
            _isReturnPosition = true;
        }

        CreatureInformation.MonsterCurrentState = EMonsterStateIDs.Idle;
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RETURN_POSITION, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        _playAnimation = false;
        _isDoneHealing = false;
        _isReturnPosition = false;

        _increaseHealing = 0;

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RETURN_POSITION, false);
    }

    public override void UpdateState()
    {
        if (_isReturnPosition)
        {
            InitializeDirection();
        }

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EMonsterStateIDs.Damaged);

            return;
        }

        IncreaseHealing();

        ChangeRestingAnimation();
    }

    private void ChangeRestingAnimation()
    {
        if (_playAnimation)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeRestingAnimationTime)
        {
            StartCoroutine(RestingAnimator());
            _playAnimation = true;
        }
    }

    private IEnumerator RestingAnimator()
    {
        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RESTING, true);

        yield return new WaitForSeconds(0.1f);

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RESTING, false);
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

    private void InitializeDirection()
    {
        Vector3 InitializeAngle = new Vector3(0f, CreatureInformation.MonsterSpawnDirection, 0f);

        if (InitializeAngle != MonsterObject.transform.eulerAngles)
        {
            MonsterObject.transform.eulerAngles = Vector3.zero;
            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, CreatureInformation.MonsterSpawnDirection, 0f);
            MonsterObject.transform.rotation = monsterInitializeDirection;
        }
    }
}
