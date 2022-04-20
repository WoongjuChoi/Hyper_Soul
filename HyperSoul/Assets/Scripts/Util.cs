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
    public static readonly int AIM = Animator.StringToHash("Aim");
    public const string DOJUMP = "DoJump";
    public const string ISJUMP = "IsJump";
    public const string FALLING = "Falling";
    public const string RELOAD = "Reload";
    public const string RELOAD_SPEED = "ReloadSpeed";
    public const string ISSHOOT = "IsShoot";
    public const string SINGLESHOT = "SingleShot";
    public const string MOVEMENT = "Movement";
}

public class MonsterAnimatorID
{
    public const string IS_RESTING = "isResting";
    public const string IS_SPAWN = "isSpawn";
    public const string IS_ALERT = "isAlert";
    public const string IS_ATTACK = "isAttack";
    public const string IS_DAMAGED = "isDamaged";
    public const string IS_DIE = "isDie";
    public const string IS_CHASE = "isChase";
    public const string IS_RETURN_POSITION = "isReturnPosition";
    public const string IS_TREANT_ROOT_ATTACK = "isRootAttack";
    public const string IS_TREANT_STOMP_ATTACK = "isStompAttack";
}

public class CommonAnimatorID
{
    public const string DIE = "Die";
    public const string HIT = "Hit";
}

public class ResultSceneParameterID
{
    public const string WIN = "Win";
    public const string LOSE = "Lose";
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

    public const int LAYER_PLAYER = 10;
    public const int LAYER_AMMO = 11;
    public const int LAYER_BOUNDARY = 12;
}

public enum EPlayerType
{
    Rifle = 0,
    Bazooka = 1,
    Sniper = 2
}
    

public enum CharacterType
{
    Player,
    Monster,
}

public enum MonsterType
{
    Wolf,
    Tree,
    Golem,
}
