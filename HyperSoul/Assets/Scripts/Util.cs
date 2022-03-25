using UnityEditor;
using UnityEngine;

class InputParameterID
{
    public const string VERTICAL = "Vertical";
    public const string HORIZONTAL = "Horizontal";
    public const string MOUSE_X = "Mouse X";
    public const string MOUSE_Y = "Mouse Y";
    public const string JUMP = "Jump";
}

class PlayerAnimatorID
{
    public static readonly int VERTICAL = Animator.StringToHash(InputParameterID.VERTICAL);
    public static readonly int HORIZONTAL = Animator.StringToHash(InputParameterID.HORIZONTAL);
}

public enum EGunState
{
    Ready,
    Empty,
    Reloading,
}