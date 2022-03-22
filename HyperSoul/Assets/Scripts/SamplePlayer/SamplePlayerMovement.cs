using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerMovement : MonoBehaviour
{
    [SerializeField] private SamplePlayerInput _playerInput = null;

    [SerializeField] private float _moveSpeed = 0f;

    private bool _isMoving = false;

    public bool IsMoving { get { return _isMoving; } }

    private void Update()
    {
        Vector3 _bufferPosition = transform.position;

        Vector3 moveVec = new Vector3(_playerInput.MoveHorizontal, 0f, _playerInput.MoveVertical).normalized;

        transform.position += _moveSpeed * Time.deltaTime * moveVec;

        if (_bufferPosition != transform.position)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }
}
