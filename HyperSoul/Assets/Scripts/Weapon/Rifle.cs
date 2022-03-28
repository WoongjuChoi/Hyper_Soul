using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private Animator _playerAnimator;

    private void OnEnable()
    {
        _curBulletCnt = 20;
        _maxBulletAmt = 20;
        _reloadTime = 2f;
        _gunState = EGunState.Ready;
    }
    public override void Fire() 
    {
    }
    public override void Zoom() 
    {
    }
}
