using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerInput : MonoBehaviour
{
    private float _moveHorizontal = 0f;
    private float _moveVertical = 0f;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float MOVEHORIZONTAL { get { return _moveHorizontal; } }
    public float MOVEVERTICAL { get { return _moveVertical; } }

    private void Update()
    {
        _moveHorizontal = Input.GetAxisRaw(HORIZONTAL);

        _moveVertical = Input.GetAxisRaw(VERTICAL);
    }
}
