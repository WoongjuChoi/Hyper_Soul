using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity, IGiveExp
{
    [SerializeField]
    private Slider _myHpSlider;
    [SerializeField]
    private Slider _expSlider;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _expText;

    [SerializeField]
    private Text _killText;
    [SerializeField]
    private Text _levelUpText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private GameObject _playerUI;

    private PlayerType _playerType; //

    private Weapon _playerWeapon;

    public int PhotonViewID;

    public int CurExp { get; set; }
    public int MaxExp
    {
        get; set;
    }

    public override void Awake()
    {
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        // Temp
        _playerType = PlayerType.Bazooka;
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
        //Debug.Log("인포 스타트");

        PhotonViewID = photonView.ViewID;
        NickName = photonView.Owner.NickName;
        _playerWeapon = GetComponentInChildren<Weapon>();

        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _hitImage.SetActive(false);
    }

    private void OnEnable()
    {
        MaxHp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        Debug.Log($"MaxHp : {MaxHp}");
        MaxExp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        CurHp = MaxHp;
        IsDead = false;
        _hitSound.SetActive(false);
        _deathSound.SetActive(false);
    }

    private void Update()
    {
        //// 디버깅용
        //Debug.Log($"CurExp : {CurExp}");

        HpUpdate();
        AmmoUpdate();
        ExpUpdate();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            Debug.Log($"피격당함\n Attacker : {collision.gameObject.GetComponent<Projectile>().ProjectileOwnerID}" +
           $"\n Damage : {collision.gameObject.GetComponent<Projectile>().Attack}" +
           $"\n HP : {CurHp}");

            TakeDamage(collision.gameObject.GetComponent<Projectile>().ProjectileOwnerID, collision.gameObject.GetComponent<Projectile>().Attack,
                collision.transform.position, collision.transform.position.normalized);
        }
    }

    private void HpUpdate()
    {
        _myHpSlider.value = (float)CurHp / MaxHp;

        if (CurHp <= 0 && photonView.IsMine)
        {
            _gameOverText.gameObject.SetActive(true);
        }
    }

    private void AmmoUpdate()
    {
        _ammoText.text = _playerWeapon.CurBulletCnt + " \\ " + _playerWeapon.MaxBulletAmt;
    }

    public void GiveExp(int expAmt)
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

    private IEnumerator LevelUp()
    {
        _levelUpText.gameObject.SetActive(true);
        ++Level;
        MaxHp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        CurHp = MaxHp;

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }

}
