using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantInformation : MonsterInformation
{
    [SerializeField]
    private float _viewAngle = 0f;

    [SerializeField]
    private float _rotateSpeed = 0f;

    private TreantAttackState _treantAttackState = null;
    private TreantDamagedState _treantDamagedState = null;
    private TreantDieState _treantDieState = null;
    private TreantIdleState _treantIdleState = null;
    private TreantRotatePositionState _treantRotatePositionState = null;
    private TreantSpawnState _treantSpawnState = null;

    private Vector3 _vecMonsterToTarget = Vector3.zero;

    private bool _existInSight = false;

    private const float DOT_120_DEGREE = -0.5f;

    public Vector3 VectorMonsterYoTarget { get { return _vecMonsterToTarget; } }
    public bool ExistInSight { get { return _existInSight; } }
    public float RotateSpeed { get { return _rotateSpeed; } }

    public override void Awake()
    {
        _treantAttackState = GetComponent<TreantAttackState>();
        _treantDamagedState = GetComponent<TreantDamagedState>();
        _treantDieState = GetComponent<TreantDieState>();
        _treantIdleState = GetComponent<TreantIdleState>();
        _treantRotatePositionState = GetComponent<TreantRotatePositionState>();
        _treantSpawnState = GetComponent<TreantSpawnState>();

        _monsterFSM.AddState(EStateIDs.Attack, _treantAttackState);
        _monsterFSM.AddState(EStateIDs.Damaged, _treantDamagedState);
        _monsterFSM.AddState(EStateIDs.Die, _treantDieState);
        _monsterFSM.AddState(EStateIDs.Idle, _treantIdleState);
        _monsterFSM.AddState(EStateIDs.RotatePosition, _treantRotatePositionState);
        _monsterFSM.AddState(EStateIDs.Spawn, _treantSpawnState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (false == _isDamaged && SampleObjectParameterID.LAYER_SAMPLE_AMMO == collision.gameObject.layer)
        {
            if (EStateIDs.Attack == _monsterCurrentState || EStateIDs.Idle == _monsterCurrentState || EStateIDs.RotatePosition == _monsterCurrentState)
            {
                _collisionVec = collision.gameObject.transform.position;

                Vector3 collisionVec = new Vector3(_collisionVec.x, gameObject.transform.position.y, _collisionVec.z);

                collisionVec = (collisionVec - gameObject.transform.position).normalized;

                float internalAngle = Vector3.Dot(gameObject.transform.forward, collisionVec);

                if (internalAngle > DOT_120_DEGREE)
                {
                    _isDamaged = true;
                }

                if (false == _isTargeting)
                {
                    _target = collision.gameObject.GetComponent<BazookaMissile>().ProjectileOwner.gameObject;

                    _isTargeting = true;
                }
            }
        }
    }

    private void Update()
    {
        if (_isTargeting)
        {
            ExistInTreantSight();
        }

        // µð¹ö±ë¿ë
        Debug.DrawRay(_collisionVec, _lookAtTargetVec * 1000f, Color.red);
        Debug.DrawRay(_monsterRayPoint.position, gameObject.transform.forward * 1000f, Color.black);

        //Debug.Log($"_monsterCurrentHP : {_monsterCurrentHP}");
        //Debug.Log($"gameObject.transform.eulerAngles: {gameObject.transform.eulerAngles}");
    }

    private void ExistInTreantSight()
    {
        Vector3 targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);

        _lookAtTargetVec = targetPosition - _collisionVec;

        Vector3 monsterSightPosition = new Vector3(_monsterRayPoint.position.x, targetPosition.y, _monsterRayPoint.position.z);

        _vecMonsterToTarget = (targetPosition - monsterSightPosition).normalized;

        float dotMonsterToTarget = Vector3.Dot(gameObject.transform.forward, _vecMonsterToTarget);

        if (dotMonsterToTarget > Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
        {
            _existInSight = true;
        }
        else
        {
            _existInSight = false;
        }
    }
}
