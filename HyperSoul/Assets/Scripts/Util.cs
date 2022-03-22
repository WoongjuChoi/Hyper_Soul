using UnityEditor;
using UnityEngine;

class InputParameterID
{
    public const string VERTICAL = "Vertical";
    public const string HORIZONTAL = "Horizontal";
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

class TagParameterID
{
    public const string MAP = "Map";
    public const string BULLET = "Bullet";
}