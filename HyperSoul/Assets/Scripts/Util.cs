using UnityEditor;
using UnityEngine;

class InputParameterID
{
    public const string VERTICAL = "Vertical";
    public const string HORIZONTAL = "Horizontal";
    public const string MOUSE_X = "Mouse X";
    public const string MOUSE_Y = "Mouse Y";
    public const string JUMP = "Jump";
    public const string FIRE = "Fire1";
    public const string ZOOM = "Zoom";
}

class PlayerAnimatorID
{
    public static readonly int VERTICAL = Animator.StringToHash(InputParameterID.VERTICAL);
    public static readonly int HORIZONTAL = Animator.StringToHash(InputParameterID.HORIZONTAL);
    public static readonly int AIM = Animator.StringToHash("Aim");
    public const string DOJUMP = "DoJump";
    public const string ISJUMP = "IsJump";
    public const string FALLING = "Falling";
    public const string DIE = "Die";
    public const string HIT = "Hit";
    public const string RELOAD = "Reload";
}

class TagParameterID
{
    public const string MAP = "Map"; // Layer·Î ¹Ù²ã¾ßÇÔ
    public const string BULLET = "Bullet";
    public const string PLAYER = "Player";
}

public enum EGunState
{
    Ready,
    Empty,
    Reloading,
}