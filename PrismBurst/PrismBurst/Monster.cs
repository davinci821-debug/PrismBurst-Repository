using PrismBurst.Enum;

namespace PrismBurst
{
    internal class Monster : Character
    {
        public MonsterType Type { get; protected set; }
        public int GiveExp { get; protected set; }
        public int GiveGold { get; protected set; }
        public DangerLevel DangerState { get; protected set; }

        public Monster(string name, int hp, int attack, int shield, int speed, int movePoint,
            MonsterType monsterType, int giveExp,int giveGold)
            : base(name, hp, attack, shield, speed, movePoint, new Position(8, 5))
        {
            Type = monsterType;
            GiveExp = giveExp;
            GiveGold = giveGold;
        }

        public override string ShowStatus()
        {
            return
                $"이름 : {Name}\n " +
                $"타입 : {Type}\n " +
                $"HP : {Hp}/{MaxHp}\n " +
                $"공격력 : {Attack}\n " +
                $"방어력 : {Shield}";
        }

        public virtual void MoveAI(Player player, int mapWidth = 10, int mapHeight = 10)
        {
            int nextX = Position.X;
            int nextY = Position.Y;

            if (player.Position.X < Position.X)
                nextX--;
            else if (player.Position.X > Position.X)
                nextX++;

            if (player.Position.Y < Position.Y)
                nextY--;
            else if (player.Position.Y > Position.Y)
                nextY++;

        
            if (nextX >= 0 && nextX < mapWidth &&
                nextY >= 0 && nextY < mapHeight)
            {
                Move(
                    nextX - Position.X,
                    nextY - Position.Y);
            }
        }
        public void UpdateDangerState(int distance)
        {
            Attack = BaseAttack;

            CritChance = 0;

            DangerState = DangerLevel.None;

            if (distance <= 1)
            {
                DangerState = DangerLevel.Berserk;

                Attack += 10;

                CritChance = 70;
            }
            else if (distance <= 2)
            {
                DangerState = DangerLevel.Critical;

                Attack += 5;

                CritChance = 20;
            }
            else if (distance <= 3)
            {
                DangerState = DangerLevel.Warning;

                Attack += 3;

                CritChance = 10;
            }
        }
        public virtual void Action(Player player, int mapWidth = 10, int mapHeight = 10)
        {
            int distance = GetDistance(player);

            UpdateDangerState(distance);

            if (distance > 1)
            {
                MoveAI(player, mapWidth, mapHeight);
            }            
        }
    }
}