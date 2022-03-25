using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{
    void Start()
    {
        _curBulletCnt = 10;
        _maxBulletAmt = 10;
        _reloadTime = 5;
    }

    void Update()
    {

    }


    public override void Fire()
    {
        base.Fire();
    }

    public override void Zoom()
    {
        base.Zoom();
    }

}
