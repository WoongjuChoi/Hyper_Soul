using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfInformation : MonsterInformation
{
    [SerializeField]
    private GameObject _attackCollider = null;
    [SerializeField]
    private GameObject _startPoint = null;

    public GameObject AttackCollider { get { return _attackCollider; } }
    public GameObject StartPoint { get { return _startPoint; } }

    public override void Awake()
    {
        _hitImage.SetActive(false);
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);

        _animator = _monsterAnimatorObject.GetComponent<Animator>();

        _monsterType = EMonsterType.Wolf;
        Level = 1;

        _attackCollider.SetActive(false);

        _monsterFSM.AddState(EMonsterStateIDs.Alert, GetComponent<WolfAlertState>());
        _monsterFSM.AddState(EMonsterStateIDs.Attack, GetComponent<WolfAttackState>());
        _monsterFSM.AddState(EMonsterStateIDs.Chase, GetComponent<WolfChaseState>());
        _monsterFSM.AddState(EMonsterStateIDs.Damaged, GetComponent<WolfDamagedState>());
        _monsterFSM.AddState(EMonsterStateIDs.Die, GetComponent<WolfDieState>());
        _monsterFSM.AddState(EMonsterStateIDs.Idle, GetComponent<WolfIdleState>());
        _monsterFSM.AddState(EMonsterStateIDs.ReturnPosition, GetComponent<WolfReturnPositionState>());
        _monsterFSM.AddState(EMonsterStateIDs.Spawn, GetComponent<WolfSpawnState>());
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (LayerParameter.LAYER_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }

        if (null != other.gameObject.GetComponent<PhotonView>())
        {
            int colliderID = other.gameObject.GetComponent<PhotonView>().ViewID;

            MonsterDamage(colliderID);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerParameter.LAYER_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerParameter.LAYER_PLAYER == other.gameObject.layer)
        {
            _isWithinAttackRange = false;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (LayerParameter.LAYER_PLAYER == collision.gameObject.layer)
        {
            _isWithinAttackRange = true;
        }
    }

    public override void MonsterDamage(int ID)
    {
        GameObject collideObject = PhotonView.Find(ID).gameObject;

        if (LayerParameter.LAYER_AMMO == collideObject.layer)
        {
            _attackerInfo = collideObject.GetComponent<Projectile>();
            _attacker = PhotonView.Find(_attackerInfo.ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;

            if (false == IsDamaged)
            {
                if (EMonsterStateIDs.Alert == MonsterCurrentState || EMonsterStateIDs.Attack == MonsterCurrentState || EMonsterStateIDs.Chase == MonsterCurrentState || EMonsterStateIDs.Idle == MonsterCurrentState)
                {
                    IsDamaged = true;
                    _collisionVec = gameObject.transform.position;
                    _target = PhotonView.Find(_attackerInfo.ProjectileOwnerID).GetComponent<LivingEntity>().gameObject;
                    Vector3 targetPosition = _target.transform.position + new Vector3(0f, 1.3f, 0f);
                    _lookAtTargetVec = targetPosition - _collisionVec;
                }
            }
        }
    }
}
