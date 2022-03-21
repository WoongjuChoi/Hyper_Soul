using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float VerticalMoveInput { get; private set; }
    public float HorizontalMoveInput { get; private set; }

    // Update is called once per frame
    void Update()
    {
        VerticalMoveInput = Input.GetAxis(InputParameterID.VERTICAL);
        HorizontalMoveInput = Input.GetAxis(InputParameterID.HORIZONTAL);
    }
}
