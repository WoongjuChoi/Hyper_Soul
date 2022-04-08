using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviourPun
{
	public Vector2 MoveVec { get; private set; }
	public Vector2 MousePos { get; private set; }
	public bool IsJump { get; set; }
	public bool IsZoom { get; private set; }
	public bool IsReload { get; set; }
	public bool IsShoot { get; private set; }

	public void OnMove(InputValue value)
	{
		if (photonView.IsMine)
		{
			MoveVec = value.Get<Vector2>();
		}
	}
	public void OnLook(InputValue value)
	{
		if (photonView.IsMine)
		{
			MousePos = value.Get<Vector2>();
		}
	}
	public void OnJump(InputValue value)
	{
		if (photonView.IsMine)
		{
			IsJump = value.isPressed;
		}
	}
	public void OnZoom(InputValue value)
	{
		if (photonView.IsMine)
		{
			IsZoom = value.isPressed;
		}
	}
	public void OnReload(InputValue value)
	{
		if (photonView.IsMine)
		{
			IsReload = value.isPressed;
		}

	}
	public void OnShoot(InputValue value)
	{
		if (photonView.IsMine)
		{
			IsShoot = value.isPressed;
		}
	}
}
