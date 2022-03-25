using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{
    void Start()
    {
        _curBulletCnt = 7;
        _maxBulletAmt = 10;
        _reloadTime = 5;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            base.HasReloaded();
        }
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
