using PrismBurst.Enum;
using PrismBurst.Scenes;

namespace PrismBurst
{
    internal class Player : Character
    {
        public int Level { get; protected set; }
        public int Exp { get; protected set; }
        public int Gold { get; protected set; }
        public MagicType ColorMagic { get; set; }
        public int ComboCount { get; set; }
        public int ShuffleCount { get; protected set; }
        public Player()
            : base("컬러매지션", 150, 30, 10, 5, 4, new Position(1, 5))
        {
            Level = 1;
            Exp = 0;
            Gold = 0;
            ShuffleCount = 2;
            ComboCount = 0;
            ColorMagic = MagicType.None;
            CritChance = 20;
        }

        public override string ShowStatus()
        {
            return
                $"이름 : {Name}\n " +
                $"HP : {Hp}/{MaxHp}\n " +
                $"공격력 : {Attack}\n " +
                $"방어력 : {Shield}\n " +
                $"이동력 : {MovePoint}/{MaxMovePoint}";
        }

        public void GetExp(int exp)
        {
            Exp += exp;

            if (Exp >= MaxExp())
            {
                LevelUP();
            }
        }
        public int MaxExp()
        {
            return Level * 100 + Level * 20;
        }

        public void LevelUP()
        {
            Level++;
            Exp = 0;            
            MaxHp += 20 * Level;
            Hp = MaxHp;
            Attack += 5 * Level + 5;
            BaseAttack = Attack;
            Shield += 3 * Level + 2;
            if (Level % 5 == 0)
            {
                MaxMovePoint++;

                MovePoint = MaxMovePoint;
            }
        }
        public void GetGold(int gold)
        {
            Gold += gold;
        }
        public override void StartTurn()
        {
            base.StartTurn();

            ComboCount = 0;
        }
        public void ChangeMagic(MagicType magic)
        {
            ColorMagic = magic;
        }
    }
}
