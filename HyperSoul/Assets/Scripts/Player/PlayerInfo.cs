using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity, IGiveExp, IGiveScore
{
    [SerializeField]
    private Text _myHpText;
    [SerializeField]
    private Slider _myHpSlider;
    [SerializeField]
    private Slider _expSlider;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _expText;
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _killText;
    [SerializeField]
    private Text _levelUpText;

    [SerializeField]
    private GameObject _playerUI;

    private PlayerType _playerType;

    private Weapon _playerWeapon;

    public int PhotonViewID;

    public int CurExp { get; set; }
    public int MaxExp
    {
        get; set;
    }
    public int CurScore { get; set; }

    private const int MONSTER_ATTACK_COLLIDER = 13;

    public override void Awake()
    {
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        //_dataManager = GameManager.DataManager;

        // Temp
        _playerType = PlayerType.Rifle;
        Level = 1;
        CurExp = 0;

        if (photonView.IsMine)
        {
            _hpBarOverhead.gameObject.SetActive(false);
        }
        else
        {
            _playerUI.SetActive(false);
        }
    }

    private void Start()
    {
        //Debug.Log("���� ��ŸƮ");

        PhotonViewID = photonView.ViewID;
        NickName = photonView.Owner.NickName;
        _playerWeapon = GetComponentInChildren<Weapon>();

        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _hitImage.SetActive(false);
    }

    private void OnEnable()
    {
        MaxHp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        Score = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Score;
        CurHp = MaxHp;
        CurScore = 0;
        IsDead = false;
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
    }

    private void Update()
    {
        //// ������
        //Debug.Log($"CurExp : {CurExp}");

        HpUpdate();
        AmmoUpdate();
        ExpUpdate();
        ScoreUpdate();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (null != projectile)
        {
            Debug.Log($"�ǰݴ���\n Attacker : {projectile.ProjectileOwnerID}" +
           $"\n Damage : {projectile.Attack}" +
           $"\n HP : {CurHp}");

            TakeDamage(projectile.ProjectileOwnerID, projectile.Attack,
                collision.transform.position, collision.transform.position.normalized);
        }

        LivingEntity livingEntity = collision.gameObject.GetComponentInParent<LivingEntity>();

        if (MONSTER_ATTACK_COLLIDER == collision.gameObject.layer)
        {
            Debug.Log($"�ǰݴ���\nAttacker : {livingEntity.gameObject.name}" +
                    $"\nDamage : {livingEntity.Attack}" +
                    $"\nHP : {CurHp}");

            TakeMonsterDamage(livingEntity.Attack);
        }

    }

    private void HpUpdate()
    {
        _myHpText.text = $"HP : {CurHp} / {MaxHp}";
        _myHpSlider.value = (float)CurHp / MaxHp;
    }

    private void AmmoUpdate()
    {
        _ammoText.text = _playerWeapon.CurBulletCnt + " \\ " + _playerWeapon.MaxBulletAmt;
    }

    public void GiveMonsterExp(int expAmt)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CurExp += expAmt;

            photonView.RPC("UpdateExp", RpcTarget.Others, CurExp);
        }
    }

    [PunRPC]
    public void UpdateExp(int newExp)
    {
        CurExp = newExp;
    }

    private void ExpUpdate()
    {
        _expText.text = "Exp : " + CurExp;

        _expSlider.value = (float)CurExp / MaxExp;

        if (CurExp >= MaxExp && Level < MaxLevel)
        {
            if (photonView.IsMine)
            {
                StartCoroutine(LevelUp());
            }
            
            CurExp = 0;
        }
    }
    public void GiveMonsterScore(int score)
    {
        CurScore += score;
    }

    private void ScoreUpdate()
    {
        _scoreText.text = $"Score : {CurScore}";
    }

    private IEnumerator LevelUp()
    {
        _levelUpText.gameObject.SetActive(true);
        ++Level;
        MaxHp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        CurHp = MaxHp;

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }

    public override void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }
}
