using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	public Vector2 MoveVec { get; private set; }
	public Vector2 MousePos { get; private set; }
	public bool IsJump { get; set; }
	public bool IsZoom { get; private set; }

	public bool IsShot { get; private set; }
	public bool IsSingleShot { get; private set; }

	public void OnMove(InputValue value)
    {
		MoveVec = value.Get<Vector2>();
    }
	public void OnLook(InputValue value)
    {
		MousePos = value.Get<Vector2>();
    }
	public void OnJump(InputValue value)
    {
		IsJump = value.isPressed;
    }
	public void OnZoom(InputValue value)
    {
		IsZoom = value.isPressed;
    }
	public void OnShot(InputValue value)
	{
		IsShot = value.isPressed;
	}

}
