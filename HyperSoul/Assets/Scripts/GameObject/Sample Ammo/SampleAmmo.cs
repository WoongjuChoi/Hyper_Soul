using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAmmo : MonoBehaviour
{
    [SerializeField]
    private GameObject _owner = null;

    [SerializeField]
    private GameObject _spawner = null;

    [SerializeField]
    private float _moveSpeed = 0f;

    public GameObject Owner { get { return _owner; } }

    private void OnEnable()
    {
        transform.position = _spawner.transform.position;
    }

    private void Update()
    {
        Vector3 moveVec = _moveSpeed * Time.deltaTime * _owner.GetComponent<SamplePlayerFire>().FireVec;

        transform.position += moveVec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);

        transform.position = _spawner.transform.position;

        _owner.GetComponent<SamplePlayerFire>().FireDelay = false;
    }
}
