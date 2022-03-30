using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerInput : MonoBehaviour
{
    private float _moveHorizontal = 0f;
    private float _moveVertical = 0f;
    private float _rotateHorizontal = 0f;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOUSE_X = "Mouse X";

    private bool _isFire = false;

    public bool IsFire { get { return _isFire; } set { _isFire = value; } }
    public float MoveHorizontal { get { return _moveHorizontal; } }
    public float MoveVertical { get { return _moveVertical; } }
    public float RotateHorizontal { get { return _rotateHorizontal; } }

    private void Update()
    {
        _moveHorizontal = Input.GetAxisRaw(HORIZONTAL);

        _moveVertical = Input.GetAxisRaw(VERTICAL);

        _rotateHorizontal = Input.GetAxis(MOUSE_X);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isFire = true;
        }
    }
}
