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

    private EPlayerType _playerType;

    private Weapon _playerWeapon;

    public int PhotonViewID;

    public int MaxExp
    {
        get; set;
    }
    public float MoveSpeed
    {
        get; set;
    }
    private const int AMMO_COLLIDER = 11;
    private const int MONSTER_ATTACK_COLLIDER = 13;

    public override void Awake()
    {
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();

        _playerType = DataManager.Instance.PlayerType;

        if (photonView.IsMine)
        {
            Level = 1;

            photonView.RPC(nameof(SetPlayerLevel), RpcTarget.Others, Level);

            CurExp = 0;
            CurScore = 0;

            photonView.RPC(nameof(SetPlayerInformations), RpcTarget.Others, CurScore, CurExp);

            _hpBarOverhead.gameObject.SetActive(false);
        }
        else
        {
            _playerUI.SetActive(false);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "loadPlayer", true } });

        photonView.RPC(nameof(LevelTextUpdate), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetPlayerLevel(int level)
    {
        Level = level;
    }

    [PunRPC]
    public void SetPlayerInformations(int curScore, int curExp)
    {
        CurScore = curScore;
        CurExp = curExp;
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
        if (photonView.IsMine)
        {
            MaxHp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
            MaxExp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
            Attack = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
            Score = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Score;
            Exp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Exp;
            MoveSpeed = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MoveSpeed;

            photonView.RPC(nameof(UpdatePlayerInfo), RpcTarget.AllViaServer, MaxHp, Attack, Score, Exp);

            CurHp = MaxHp;

            photonView.RPC(nameof(SetCurHp), RpcTarget.AllViaServer, CurHp);
        }

        IsDead = false;
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
    }

    [PunRPC]
    private void SetCurHp(int curHp)
    {
        CurHp = curHp;
    }

    [PunRPC]
    public void UpdatePlayerInfo(int maxHp, int attack, int score, int exp)
    {
        MaxHp = maxHp;
        Attack = attack;
        Score = score;
        Exp = exp;
    }

    private void Update()
    {
        _profileCanvas.gameObject.transform.rotation = GameManager.Instance.PlayerCamRotationTransform.rotation;

        if (false == photonView.IsMine)
        {
            return;
        }
        
        HpUpdate();
        AmmoUpdate();
        ExpUpdate();
        ScoreUpdate();
    }

    public override void OnTriggerEnter(Collider collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();

        if (AMMO_COLLIDER == collider.gameObject.layer)
        {
            //Debug.Log($"피격당함\n Attacker : {projectile.ProjectileOwnerID}" +
            //$"\n Damage : {projectile.Attack}" +
            //$"\n HP : {CurHp}");

            TakeDamage(projectile.ProjectileOwnerID, projectile.Attack,
                collider.transform.position, collider.transform.position.normalized);
        }

        LivingEntity livingEntity = collider.gameObject.GetComponentInParent<LivingEntity>();

        if (MONSTER_ATTACK_COLLIDER == collider.gameObject.layer)
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
            StartCoroutine(LevelUp());

            CurExp = 0;
        }
    }

    private void ScoreUpdate()
    {
        _scoreText.text = $"Score : {CurScore}";

        if (GameManager.Instance.IsGameOver)
        {
            photonView.RPC(nameof(MyScoreUpdate), RpcTarget.All);
        }
    }

    [PunRPC]
    private void MyScoreUpdate()
    {
        int myPlayerOrderIndex = DataManager.Instance.MyPlayerOrderIndex;
        DataManager.Instance.PlayerInfos[myPlayerOrderIndex].score = CurScore;
    }

    private IEnumerator LevelUp()
    {
        _levelUpText.gameObject.SetActive(true);
        ++Level;

        photonView.RPC(nameof(SetPlayerLevel), RpcTarget.Others, Level);

        MaxHp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        Score = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Score;
        Exp = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).Exp;
        MoveSpeed = DataManager.Instance.FindPlayerData(_playerType.ToString() + Level.ToString()).MoveSpeed;

        photonView.RPC(nameof(UpdatePlayerInfo), RpcTarget.AllViaServer, MaxHp, Attack, Score, Exp);
        photonView.RPC(nameof(LevelTextUpdate), RpcTarget.AllBuffered);

        CurHp = MaxHp;

        photonView.RPC(nameof(SetCurHp), RpcTarget.AllViaServer, CurHp);

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }

    [PunRPC]
    private void LevelTextUpdate()
    {
        _levelText.text = $"{Level}";
    }

    public override void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }

    [PunRPC]
    public void PlayerActive(bool b)
    {
        gameObject.SetActive(b);
    }
}
