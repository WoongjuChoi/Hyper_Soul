using UnityEditor;
using UnityEngine;

class InputParameterID
{
    public const string VERTICAL = "Vertical";
    public const string HORIZONTAL = "Horizontal";
}

class PlayerAnimatorID
{
    public static readonly int VERTICAL = Animator.StringToHash(InputParameterID.VERTICAL);
    public static readonly int HORIZONTAL = Animator.StringToHash(InputParameterID.HORIZONTAL);
<<<<<<< Updated upstream
}
=======
    public static readonly int AIM = Animator.StringToHash("Aim");
    public const string DOJUMP = "DoJump";
    public const string ISJUMP = "IsJump";
    public const string FALLING = "Falling";
    public const string DIE = "Die";
    public const string HIT = "Hit";
    public const string RELOAD = "Reload";
    public const string ISSHOOT = "IsShoot";
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

class LayerParameter
{
    public const int PLAYER = 3;
    public const int MONSTER = 6;
    public const int BULLET = 7;
}

public enum CharacterType
{
    Player,
    Monster,
}

public enum PlayerType
{
    Bazooka,
    Rifle,
    Sniper,
}

public enum MonsterType
{
    Wolf,
    Tree,
    Golem,
}
>>>>>>> Stashed changes
