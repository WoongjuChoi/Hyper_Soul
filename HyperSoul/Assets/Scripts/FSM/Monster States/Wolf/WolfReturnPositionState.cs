using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfReturnPositionState : BaseState<WolfInformation>
{
    [SerializeField]
    private AudioSource _returnPositionAudioSource;

    private float _distance = 0f;

    private int _increaseHealing = 0;

    private bool _isDoneHealing = false;

    private const string IS_WALK = "isWalk";

    public override void EnterState()
    {
        CreatureInformation.MonsterCurrentState = EStateIDs.ReturnPosition;

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_CHASE, true);

        _returnPositionAudioSource.Play();
    }

    public override void ExitState()
    {
        _isDoneHealing = false;

        CreatureInformation.IsTargeting = false;

        CreatureInformation.MonsterChaser.ResetPath();

        CreatureInformation.MonsterChaser.IsActive = false;

        GameObject.transform.position = CreatureInformation.InitializeTransform.position;

        CreatureInformation.CreatureAnimator.SetBool(IS_WALK, false);

        CreatureInformation.CreatureAnimator.SetBool(MonsterAnimatorID.IS_CHASE, false);
        
        _returnPositionAudioSource.Stop();
    }

    public override void UpdateState()
    {
        _distance = (GameObject.transform.position - CreatureInformation.InitializeTransform.position).magnitude;

        if (_distance <= 2f)
        {
            CreatureInformation.CreatureAnimator.SetBool(IS_WALK, true);
        }
        
        if (_distance <= 0.5f)
        {
            InitializeDirection();

            FiniteStateMachine.ChangeState(EStateIDs.Idle);

            return;
        }

        IncreaseHealing();

        CreatureInformation.MonsterChaser.IsTarget = CreatureInformation.InitializeTransform;

        CreatureInformation.MonsterChaser.IsActive = true;
    }

    private void IncreaseHealing()
    {
        if (false == _isDoneHealing)
        {
            // 현재 체력이 최대 체력이 아니라면 서서히 증가
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

        if (InitializeAngle != GameObject.transform.eulerAngles)
        {
            GameObject.transform.eulerAngles = Vector3.zero;

            Quaternion monsterInitializeDirection = Quaternion.Euler(0f, CreatureInformation.MonsterSpawnDirection, 0f);

            GameObject.transform.rotation = monsterInitializeDirection;

            //Debug.Log($"ReturnPosition GameObject.transform.rotation : {GameObject.transform.rotation}\n" +
            //    $"GameObject.transform.eulerAngles : {GameObject.transform.eulerAngles}");
        }
    }
}
