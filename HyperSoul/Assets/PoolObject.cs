using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviourPun
{
    [PunRPC]
    private void SetActiveObj(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
