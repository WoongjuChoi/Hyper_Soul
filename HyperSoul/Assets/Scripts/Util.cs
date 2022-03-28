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
    public const string DOJUMP = "doJump";
    public const string ISJUMP = "isJump";
    public const string ISFALLING = "isFalling";
}

public class MonsterAnimatorID
{
    public const string HAS_SPAWN = "hasSpawn";
    public const string HAS_IDLE = "hasIdle";
    public const string HAS_DAMAGED = "hasDamaged";
}

class TagParameterID
{
    public const string MAP = "Map"; // Layer·Î ¹Ù²ã¾ßÇÔ
    public const string BULLET = "Bullet";
}

public enum EGunState
{
    Ready,
    Empty,
    Reloading,
}