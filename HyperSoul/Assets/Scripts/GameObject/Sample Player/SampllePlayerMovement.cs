using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampllePlayerMovement : MonoBehaviour
{
    [SerializeField]
    private SamplePlayerInput _samplePlayerInput = null;
    
    [SerializeField]
    private float _moveSpeed = 0f;

    private void Update()
    {
        Vector3 moveVec = new Vector3(_samplePlayerInput.MoveHorizontal, 0f, _samplePlayerInput.MoveVertical).normalized;

        transform.position += _moveSpeed * Time.deltaTime * moveVec;
    }
}
