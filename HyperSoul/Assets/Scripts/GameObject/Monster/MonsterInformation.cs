using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterInformation : LivingEntity
{
    [SerializeField]
    protected FiniteStateMachine _monsterFSM;
    [SerializeField]
    protected MonsterSpawnManager _monsterSpawnManager;
    [SerializeField]
    protected GameObject _monsterAnimatorObject;
    [SerializeField]
    protected Chaser _monsterChaser;
    [SerializeField]
    protected Collider _attackRangeCollider;
    [SerializeField]
    protected Transform _monsterRayPoint;
    [SerializeField]
    protected float _monsterInvincibleTime = 0f;
    [SerializeField]
    protected int _monsterMaxHP = 0;

    protected GameObject _target;
    protected Projectile _attackerInfo;
    protected Vector3 _collisionVec;
    protected Vector3 _lookAtTargetVec;
    protected MonsterType _monsterType;
    protected bool _isWithinAttackRange = false;
    protected int _monsterAnimatorIndex = 0;

    public GameObject Target { get { return _target; } }
    public Chaser MonsterChaser { get { return _monsterChaser; } }
    public Projectile AttackerInfo { get { return _attackerInfo; } }
    public Collider MonsterAttackRangeCollider { get { return _attackRangeCollider; } }
    public Transform InitializeTransform { get; set; }
    public Transform MonsterRayPoint { get { return _monsterRayPoint; } }
    public Vector3 CollisionVec { get { return _collisionVec; } }
    public Vector3 LookAtTargetVec { get { return _lookAtTargetVec; } }
    public MonsterType MonsterType { get { return _monsterType; } }
    public bool IsWithinAttackRange { get { return _isWithinAttackRange; } }
    public float MonsterInvincibleTime { get { return _monsterInvincibleTime; } }
    public float MonsterSpawnDirection { get; set; }
    public int MonsterMaxLevel { get { return 5; } }

    public EStateIDs MonsterCurrentState { get; set; }
    public bool IsDamaged { get; set; }
    public bool IsTargeting { get; set; }

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MaxHp = DataManager.Instance.FindMonsterData(_monsterType.ToString() + Level.ToString()).MaxHp;
            Attack = DataManager.Instance.FindMonsterData(_monsterType.ToString() + Level.ToString()).Attack;
            Exp = DataManager.Instance.FindMonsterData(_monsterType.ToString() + Level.ToString()).Exp;
            Score = DataManager.Instance.FindMonsterData(_monsterType.ToString() + Level.ToString()).Score;

            CurHp = MaxHp;

            photonView.RPC(nameof(SetMonsterInformation), RpcTarget.Others, CurHp, MaxHp);
        }

        IsDead = false;

        _monsterFSM.ChangeState(EStateIDs.Spawn);

        IsTargeting = false;
        _isWithinAttackRange = false;
    }

    [PunRPC]
    public void SetMonsterInformation(int curHp, int maxHp)
    {
        CurHp = curHp;
        MaxHp = maxHp;
    }

    private void Start()
    {
        NickName = gameObject.name;
    }

    public override void Respawn()
    {
        photonView.RPC(nameof(MonsterActive), RpcTarget.AllViaServer, false);
    }

    [PunRPC]
    public void MonsterActive(bool b)
    {
        gameObject.SetActive(b);
    }
}
