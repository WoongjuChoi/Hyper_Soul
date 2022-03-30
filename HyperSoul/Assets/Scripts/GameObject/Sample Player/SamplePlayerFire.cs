using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerFire : MonoBehaviour
{
    [SerializeField]
    private SamplePlayerInput _samplePlayerInput = null;

    [SerializeField]
    private GameObject _ammo = null;

    private Vector3 _fireVec = Vector3.zero;

    private bool _fireDelay = false;

    public Vector3 FireVec { get { return _fireVec; } }

    public bool FireDelay { get { return _fireDelay; } set { _fireDelay = value; } }

    private void Update()
    {
        if (_samplePlayerInput.IsFire && false == _fireDelay)
        {
            _ammo.SetActive(true);

            _fireVec = transform.forward;

            _samplePlayerInput.IsFire = false;

            _fireDelay = true;

        }

        // 디버깅용(Ammo 발사 방향)
        Debug.DrawRay(transform.position, transform.forward * 1000f, Color.blue);
    }
}
