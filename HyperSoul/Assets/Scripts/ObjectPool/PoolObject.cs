using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviourPun
{
    [PunRPC]
    public void SetActiveObj(bool isActive)
    {
        //if (null != GetComponent<BazookaMissile>())
        //{
        //    if (true == isActive)
        //    {
        //        GetComponent<BazookaMissile>().MissilePrefab.SetActive(true);
        //        GetComponent<BazookaMissile>().RocketParticleEffect.SetActive(false);
        //        GetComponent<BazookaMissile>().ExplosionEffect.SetActive(false);
        //    }
        //    else
        //    {
        //        GetComponent<BazookaMissile>().MissilePrefab.SetActive(false);
        //        GetComponent<BazookaMissile>().RocketParticleEffect.SetActive(false);
        //        GetComponent<BazookaMissile>().ExplosionEffect.SetActive(false);
        //    }
        //}

        gameObject.SetActive(isActive);
    }
}
