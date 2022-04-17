using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRootInformation : MonoBehaviour
{
    [SerializeField]
    private GameObject _treant = null;

    [SerializeField]
    private BoxCollider _attackCollider = null;

    private Vector3 _rotateVec = Vector3.zero;

    private void OnEnable()
    {
        _rotateVec = _treant.transform.eulerAngles;

        gameObject.transform.Rotate(0f, _rotateVec.y, 0f);

        Invoke(nameof(EnabledAttackCollider), 0.5f);
    }

    private void OnDisable()
    {
        _attackCollider.enabled = false;

        gameObject.transform.Rotate(0f, -_rotateVec.y, 0f);
    }

    private void EnabledAttackCollider()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _attackCollider.enabled = true;
        }
    }
}
