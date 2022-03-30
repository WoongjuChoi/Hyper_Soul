using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SamplePlayerRotation : MonoBehaviour
{
    [SerializeField]
    private SamplePlayerInput _samplePlayerInput = null;

    [SerializeField]
    private float _rotateSpeed = 0f;

    private void Update()
    {
        float turnSpeed = _samplePlayerInput.RotateHorizontal * _rotateSpeed * Time.deltaTime;

        gameObject.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0f, turnSpeed, 0f);
    }
}