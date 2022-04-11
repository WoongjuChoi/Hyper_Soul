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
        CreatureInformation.MonsterCurrentState = EStateIDs.Idle;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RETURN_POSITION, true);
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        _playAnimation = false;

        _increaseHealing = 0;

        GameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_RETURN_POSITION, false);
    }

    public override void UpdateState()
    {
        InitializeDirection();

        if (CreatureInformation.IsDamaged)
        {
            FiniteStateMachine.ChangeState(EStateIDs.Damaged);

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
            GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_RESTING);
            
            _playAnimation = true;
        }
    }

    private void InitializeDirection()
    {
        Vector3 InitializeAngle = new Vector3(0f, CreatureInformation.MonsterSpawnDirection, 0f);

        if (InitializeAngle != GameObject.transform.eulerAngles)
        {
            GameObject.transform.rotation = Quaternion.identity;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, CreatureInformation.MonsterSpawnDirection, 0f);

            GameObject.transform.rotation = monsterInitializeDirection;
        }
    }

    private void IncreaseHealing()
    {
        // 현재 체력이 최대 체력이 아니라면 서서히 증가
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
    }
}
