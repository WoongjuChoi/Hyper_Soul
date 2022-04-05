using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfIdleState : BaseState<WolfInformation>
{
    private bool _playAnimation = false;

    private float _changeRestingAnimationTime = 5f;
    private float _elapsedTime = 0f;

    private int _increaseHealing = 0;

    public override void EnterState()
    {
        base.CreatureInformation.MonsterCurrentState = EStateIDs.Idle;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RETURN_POSITION, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        _playAnimation = false;

        _increaseHealing = 0;

        base.GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RETURN_POSITION, false);
    }

    public override void UpdateState()
    {
        InitializeDirection();

        if (base.CreatureInformation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

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
            base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_RESTING);
            
            _playAnimation = true;
        }
    }

    private void InitializeDirection()
    {
        Vector3 InitializeAngle = new Vector3(0f, base.CreatureInformation.MonsterSpawnDirection, 0f);

        if (InitializeAngle != base.GameObject.transform.eulerAngles)
        {
            base.GameObject.transform.rotation = Quaternion.identity;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, base.CreatureInformation.MonsterSpawnDirection, 0f);

            base.GameObject.transform.rotation = monsterInitializeDirection;
        }
    }

    private void IncreaseHealing()
    {
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
    }
}
