using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantInformation : MonsterInformation
{
    private TreantAttackState _treantAttackState = null;
    private TreantDamagedState _treantDamagedState = null;
    private TreantDieState _treantDieState = null;
    private TreantIdleState _treantIdleState = null;
    private TreantSpawnState _treantSpawnState = null;

    private const float DOT_120_DEGREE = -0.5f;

    public override void Awake()
    {
        _treantAttackState = GetComponent<TreantAttackState>();
        _treantDamagedState = GetComponent<TreantDamagedState>();
        _treantDieState = GetComponent<TreantDieState>();
        _treantIdleState = GetComponent<TreantIdleState>();
        _treantSpawnState = GetComponent<TreantSpawnState>();

        _monsterFSM.AddState(EStateIDs.Attack, _treantAttackState);
        _monsterFSM.AddState(EStateIDs.Damaged, _treantDamagedState);
        _monsterFSM.AddState(EStateIDs.Die, _treantDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _treantIdleState);
        _monsterFSM.AddState(EStateIDs.Spawn, _treantSpawnState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (false == _isDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Attack == _monsterCurrentState || EStateIDs.Idle == _monsterCurrentState)
            {
                _collisionVec = collision.gameObject.transform.position;

                Vector3 collisionVec = new Vector3(_collisionVec.x, gameObject.transform.position.y, _collisionVec.z);

                collisionVec = (collisionVec - gameObject.transform.position).normalized;

                float internalAngle = Vector3.Dot(gameObject.transform.forward, collisionVec);

                if (internalAngle > DOT_120_DEGREE)
                {
                    _isDamaged = true;
                }

                _target = collision.gameObject.GetComponent<BazookaMissile>().MisilleOwner;

                Vector3 targetPosition = _target.transform.position;

                _lookAtTargetVec = targetPosition - _collisionVec;
            }
        }
    }

    private void Update()
    {
        // ������
        Debug.DrawRay(_collisionVec, _lookAtTargetVec * 1000f, Color.red);
        Debug.DrawRay(_monsterRayPoint.position, gameObject.transform.forward * 1000f, Color.black);

        //Debug.Log($"_monsterCurrentHP : {_monsterCurrentHP}");
        //Debug.Log($"gameObject.transform.eulerAngles: {gameObject.transform.eulerAngles}");
    }
}
