using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantInformation : MonsterInformation
{
    [SerializeField]
    private float _viewAngle = 0f;

    [SerializeField]
    private float _rotateSpeed = 0f;

    private Vector3 _vecMonsterToTarget = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;
    private bool _existInSight = false;

    public Vector3 VectorMonsterToTarget { get { return _vecMonsterToTarget; } }
    public Vector3 OriginVec { get; set; }
    public bool ExistInSight { get { return _existInSight; } }
    public float DistanceMonsterToTarget { get; private set; }
    public float RotateSpeed { get { return _rotateSpeed; } }

    public override void Awake()
    {
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();

        _monsterType = EMonsterType.Tree;
        Level = 1;

        _monsterFSM.AddState(EMonsterStateIDs.Attack, GetComponent<TreantAttackState>());
        _monsterFSM.AddState(EMonsterStateIDs.Damaged, GetComponent<TreantDamagedState>());
        _monsterFSM.AddState(EMonsterStateIDs.Die, GetComponent<TreantDieState>());
        _monsterFSM.AddState(EMonsterStateIDs.Idle, GetComponent<TreantIdleState>());
        _monsterFSM.AddState(EMonsterStateIDs.ReturnPosition, GetComponent<TreantReturnPositionState>());
        _monsterFSM.AddState(EMonsterStateIDs.RotatePosition, GetComponent<TreantRotatePositionState>());
        _monsterFSM.AddState(EMonsterStateIDs.Spawn, GetComponent<TreantSpawnState>());
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (null != other.gameObject.GetComponent<PhotonView>())
        {
            int colliderID = other.gameObject.GetComponent<PhotonView>().ViewID;

            MonsterDamage(colliderID);
        }
    }

    private void Update()
    {
        if (IsTargeting)
        {
            _targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);

            ExistInTreantSight();

            Vector3 gameObjecPosition = new Vector3(gameObject.transform.position.x, _targetPosition.y, gameObject.transform.position.z);
            DistanceMonsterToTarget = (_targetPosition - gameObjecPosition).magnitude;
        }
    }

    private void ExistInTreantSight()
    {
        _lookAtTargetVec = _targetPosition - _collisionVec;
        Vector3 monsterSightPosition = new Vector3(_monsterRayPoint.position.x, _targetPosition.y, _monsterRayPoint.position.z);
        _vecMonsterToTarget = (_targetPosition - monsterSightPosition).normalized;

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

    public override void MonsterDamage(int ID)
    {
        GameObject collideObject = PhotonView.Find(ID).gameObject;

        if (LayerParameter.LAYER_AMMO == collideObject.layer)
        {
            _attackerInfo = collideObject.GetComponent<Projectile>();
            _attacker = PhotonView.Find(_attackerInfo.ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;

            if (IsDamaged)
            {
                return;
            }

            if (EMonsterStateIDs.Attack == MonsterCurrentState || EMonsterStateIDs.Idle == MonsterCurrentState 
                || EMonsterStateIDs.RotatePosition == MonsterCurrentState)
            {
                _collisionVec = collideObject.transform.position;
                IsDamaged = true;

                if (false == IsTargeting)
                {
                    _target = PhotonView.Find(_attackerInfo.ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;
                    IsTargeting = true;
                }
            }
        }
    }
}
