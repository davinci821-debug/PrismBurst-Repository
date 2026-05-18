namespace PrismBurst.Enum
{
    [Flags]
    public enum TileType
    {
        None = 0,
        Red = 1 << 0,
        Blue = 1 << 1,
        Yellow = 1 << 2,
        Green = 1 << 3,
        Empty = 1 << 4
    }
    [Flags]
    public enum MagicType
    {
        None = 0,
        Fire = 1 << 0,
        Ice = 1 << 1,
        Thunder = 1 << 2,
        Poison = 1 << 3
    }
    [Flags]
    public enum StateType
    {
        None = 0,
        Burn = 1 << 0,
        Freeze = 1 << 1,
        Stun = 1 << 2,
        Poison = 1 << 3

    }
    public enum DangerLevel
    {
        None,
        Warning,
        Critical,
        Berserk
    }
    public enum MonsterType
    {
        Normal,
        Elite,
        Boss
    }
    public enum BattleState
    {
        SelectMagic,
        MoveTile,
        Aim,
        Shoot,
        MonsterTurn,
        Win,
        Lose
    }
}
