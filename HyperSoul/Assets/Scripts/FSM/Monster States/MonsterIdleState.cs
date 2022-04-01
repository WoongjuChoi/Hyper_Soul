using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : BaseState<MonsterInfomations>
{
    private float _changeRestingAnimationTime = 5f;
    private float _elapsedTime = 0f;

    private int _increaseHealing = 0;

    public override void EnterState()
    {
        base.CreatureInfomation = base.GameObject.GetComponent<MonsterInfomations>();

        base.CreatureInfomation.MonsterCurrentState = EStateIDs.Idle;
    }

    public override void ExitState()
    {
        _elapsedTime = 0f;

        _increaseHealing = 0;
    }

    public override void UpdateState()
    {
        Vector3 InitializeAngle = new Vector3(0f, base.CreatureInfomation.MonsterSpawnDirection, 0f);

        if (InitializeAngle != base.GameObject.transform.eulerAngles)
        {
            base.GameObject.transform.rotation = Quaternion.identity;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, base.CreatureInfomation.MonsterSpawnDirection, 0f);

            base.GameObject.transform.rotation = monsterInitializeDirection;
        }

        if (base.CreatureInfomation.IsDamaged)
        {
            base.FiniteStateMachine.ChangeState(EStateIDs.Damaged);

            return;
        }

        // ���� ü���� �ִ� ü���� �ƴ϶�� ������ ����
        if (base.CreatureInfomation.MonsterCurrentHP >= base.CreatureInfomation.MonsterMaxHP)
        {
            base.CreatureInfomation.MonsterCurrentHP = base.CreatureInfomation.MonsterMaxHP;

            _increaseHealing = 0;
        }
        else
        {
            float increaseHealing = Random.Range(0f, 1f);

            _increaseHealing += (int)Mathf.Round(increaseHealing * 10);

            base.CreatureInfomation.MonsterCurrentHP += _increaseHealing;
        }

        // ������ �ִϸ��̼��� �����ϸ� �� �̻� �������� �ʰԲ� ����
        if (-1 == _elapsedTime)
        {
            return;
        }

        ChangeRestingAnimation();
    }

    private void ChangeRestingAnimation()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _changeRestingAnimationTime)
        {
            base.GameObject.GetComponentInChildren<Animator>().SetTrigger(MonsterAnimatorID.HAS_RESTING);

            _elapsedTime = -1f;
        }
    }
}
