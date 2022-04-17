using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerInfo : LivingEntity
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
    private Text _nickNameText;

    [SerializeField]
    private GameObject _playerUI;

    private PlayerType _playerType;

    private Weapon _playerWeapon;

    public int PhotonViewID;

    public int MaxExp
    {
        get; set;
    }

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
            _levelText.gameObject.SetActive(false);
            _nickNameText.gameObject.SetActive(false);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "loadPlayer", true } });
    }

    private void Start()
    {


        PhotonViewID = photonView.ViewID;
        NickName = photonView.Owner.NickName;
        _playerWeapon = GetComponentInChildren<Weapon>();

        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _hitImage.SetActive(false);

        _nickNameText.text = NickName;
    }

    private void OnEnable()
    {
        MaxHp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        Score = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Score;

        if (photonView.IsMine)
        {
            photonView.RPC(nameof(UpdatePlayerInfo), RpcTarget.AllViaServer, Level, MaxHp, Attack, Score);
        }

        CurHp = MaxHp;
        CurScore = 0;
        IsDead = false;
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
    }

    [PunRPC]
    public void UpdatePlayerInfo(int level, int maxHp, int attack, int score)
    {
        Level = level;
        MaxHp = maxHp;
        Attack = attack;
        Score = score;
    }

    private void Update()
    {
        //// 디버깅용
        //Debug.Log($"CurExp : {CurExp}");

        HpUpdate();
        AmmoUpdate();
        ExpUpdate();
        ScoreUpdate();
        LevelUpdate();
    }

    private void LateUpdate()
    {
        _profileCanvas.gameObject.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;    // (22.04.16) 플레이어 레벨 회전
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (null != projectile)
        {
            Debug.Log($"피격당함\n Attacker : {projectile.ProjectileOwnerID}" +
           $"\n Damage : {projectile.Attack}" +
           $"\n HP : {CurHp}");

            TakeDamage(projectile.ProjectileOwnerID, projectile.Attack,
                collision.transform.position, collision.transform.position.normalized);
        }

        LivingEntity livingEntity = collision.gameObject.GetComponentInParent<LivingEntity>();

        if (MONSTER_ATTACK_COLLIDER == collision.gameObject.layer)
        {
            Debug.Log($"피격당함\nAttacker : {livingEntity.gameObject.name}" +
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
        Score = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Score;

        photonView.RPC(nameof(UpdatePlayerInfo), RpcTarget.AllViaServer, Level, MaxHp, Attack, Score);

        CurHp = MaxHp;

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }

    private void LevelUpdate()
    {
        _levelText.text = $"{Level}";
    }

    public override void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }
}
