using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity
{
    public string NickName { get; set; } // 로그인 시 닉네임 넣을 것

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

    private PlayerType _playerType; //

    private Weapon _playerWeapon;

    public int PhotonViewID;

    public int MaxExp
    {
        get; set;
    }
    public int CurExp { get; set; }

    private void Start()
    {
        // Temp
        _playerType = PlayerType.Bazooka;
        Level = 1;

        PhotonViewID = photonView.ViewID;
        NickName = photonView.Owner.NickName;
        _playerWeapon = GetComponentInChildren<Weapon>();
        

        Exp = MaxExp;

        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _hitImage.SetActive(false);
    }

    private void OnEnable()
    {
        MaxHp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxHp;
        MaxExp = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).MaxExp;
        Attack = _dataManager.FindPlayerData(_playerType.ToString() + Level.ToString()).Attack;
        CurHp = MaxHp;
        IsDead = false;
    }

    private void Update()
    {
        HpUpdate();
        AmmoUpdate();
        ExpUpdate();
    }


    private void HpUpdate()
    {
        _myHpSlider.value = (float)CurHp / MaxHp;

        if (CurHp <= 0)
        {
            _gameOverText.gameObject.SetActive(true);
        }
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
