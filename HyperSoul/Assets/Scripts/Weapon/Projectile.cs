using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    protected System.Action<GameObject> _projectileReturn;

    public int ProjectileOwnerID { get; set; }
    public int Attack { get; set; }

    public void ReceiveReturnProjectileFunc(System.Action<GameObject> returnProjectile)
    {
        _projectileReturn = returnProjectile;
    }

    [PunRPC]
    protected void ReceiveInfo(int ownerID, int attack)
    {
        ProjectileOwnerID = ownerID;
        Attack = attack;
    }

}