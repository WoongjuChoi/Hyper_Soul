using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerInput : MonoBehaviour
{
    private float _moveHorizontal = 0f;
    private float _moveVertical = 0f;

    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";

    public float MoveHorizontal {  get { return _moveHorizontal; } }
    public float MoveVertical {  get { return _moveVertical; } }

    private void Update()
    {
        _moveHorizontal = Input.GetAxisRaw(HORIZONTAL);
        _moveVertical = Input.GetAxisRaw(VERTICAL);
    }
}
