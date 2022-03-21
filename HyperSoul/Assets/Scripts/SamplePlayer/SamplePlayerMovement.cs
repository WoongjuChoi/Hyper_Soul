using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerMovement : MonoBehaviour
{
    [SerializeField] private SamplePlayerInput _playerInput = null;

    [SerializeField] private float _moveSpeed = 0f;

    private void Update()
    {
        Vector3 moveVec = new Vector3(_playerInput.MoveHorizontal, 0f, _playerInput.MoveVertical).normalized;

        transform.position += _moveSpeed * Time.deltaTime * moveVec;
    }
}
