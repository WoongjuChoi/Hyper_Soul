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

    public override void EnterState()
    {
        if (EStateIDs.ReturnPosition == CreatureInformation.MonsterCurrentState)
        {
            _isReturnPosition = true;
        }

        CreatureInformation.MonsterCurrentState = EStateIDs.Idle;

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RETURN_POSITION, true);

        //Debug.Log($"Idle Enter GameObject.transform.rotation : {GameObject.transform.rotation}\nb" +
        //    $"GameObject.transform.eulerAngles : { GameObject.transform.eulerAngles}");
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        _playAnimation = false;
        _isReturnPosition = false;

        _increaseHealing = 0;

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_RETURN_POSITION, false);
    }

    public override void UpdateState()
    {
        //Debug.Log($"Idle Update GameObject.transform.rotation : {GameObject.transform.rotation}\nb" +
        //    $"GameObject.transform.eulerAngles : { GameObject.transform.eulerAngles}");

        if (_isReturnPosition)
        {
            InitializeDirection();
        }

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
            CreatureInformation.CreatureAnimator.SetTrigger(MonsterAnimatorID.HAS_RESTING);

            _playAnimation = true;
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
    private void InitializeDirection()
    {
        Vector3 InitializeAngle = new Vector3(0f, CreatureInformation.MonsterSpawnDirection, 0f);

        if (InitializeAngle != GameObject.transform.eulerAngles)
        {
            GameObject.transform.eulerAngles = Vector3.zero;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, CreatureInformation.MonsterSpawnDirection, 0f);

            GameObject.transform.rotation = monsterInitializeDirection;
        }
    }
}
