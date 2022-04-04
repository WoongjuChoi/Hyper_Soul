using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity
{
    public int CurExp { get; set; }
    public int Exp { get; set; }
    public string NickName { get; set; }

    [SerializeField]
    private Slider _hpSlider;
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

    private Weapon _playerWeapon;

    private void Start()
    {
        _playerWeapon = GetComponentInChildren<Weapon>();
        Attack = 1f;
        CurExp = 0;
        Exp = 100;

        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        MaxHp = 5;
        CurHp = 5;
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
        _hpSlider.value = (float)CurHp / MaxHp;

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

        _expSlider.value = (float)CurExp / Exp;

        if (CurExp >= Exp)
        {
            StartCoroutine(LevelUp());
            CurExp = 0;
        }
    }

    private IEnumerator LevelUp()
    {
        _levelUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }
}
