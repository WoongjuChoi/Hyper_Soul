using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public float Attack { get; private set; }
    public int CurrHp { get; set; }
    public int MaxHp { get; set; }
    public int CurrExp { get; set; }
    public int Exp { get; set; }
    public bool IsDead { get; set; }

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

    private void Awake()
    {
        _playerWeapon = GetComponentInChildren<Weapon>();
        Attack = 1f;
        CurrExp = 0;
        Exp = 100;
        _killText.gameObject.SetActive(false);
        _levelUpText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        MaxHp = 5;
        CurrHp = 5;
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
        _hpSlider.value = (float)CurrHp / MaxHp;
    }

    private void AmmoUpdate()
    {
        _ammoText.text = _playerWeapon.CurBulletCnt + " \\ " + _playerWeapon.MaxBulletAmt;
    }

    private void ExpUpdate()
    {
        _expText.text = "Exp : " + CurrExp;

        _expSlider.value = CurrExp / Exp;

        if (CurrExp >= Exp)
        {
            StartCoroutine(LevelUp());
            CurrExp = 0;
        }
    }

    private IEnumerator LevelUp()
    {
        _levelUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        _levelUpText.gameObject.SetActive(false);
    }
}
