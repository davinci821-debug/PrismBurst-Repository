using PrismBurst.Enum;

namespace PrismBurst
{
    internal abstract class Character
    {
        public string Name { get; protected set; }
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; }        
        public int Attack { get; protected set; }
        public int BaseAttack { get; protected set; }
        public int Shield { get; set; }
        public int Speed { get; protected set; }
        public int MovePoint { get; protected set; }
        public int MaxMovePoint { get; protected set; }
        public Position Position { get; protected set; }
        public StateType State { get; set; }

        protected int CritChance;
        protected float CritDamage;

        public bool IsCritical { get; protected set; }

        protected Random random = new Random();
        public bool IsDead
        {
            get
            {
                return Hp <= 0;
            }
        }

        public Character(string name, int hp, int attack, int shield, int speed,int movePoint,
            Position position)
        {
            Name = name;
            Hp = hp;
            MaxHp = hp;
            Attack = attack;
            BaseAttack = attack;
            Shield = shield;
            Speed = speed;
            MovePoint = movePoint;
            MaxMovePoint = movePoint;
            Position = position;            
            State = StateType.None;
            CritChance = 0;
            CritDamage = 1.5f;
            IsCritical = false;
        }
        public virtual string ShowStatus()
        {
            return
                $"이름 : {Name}\n" +
                $"HP : {Hp}/{MaxHp}\n" +
                $"공격력 : {Attack}\n" +
                $"방어력 : {Shield}";
        }
        public virtual void TakeDamage(int damage)
        {
            damage -= Shield;

            if (damage < 0)
            { damage = 0;}
            Hp -= damage;
            if (Hp <= 0)
            {Hp = 0;}
        }
        public int CriticalDamage()
        {
            int damage = Attack;
            IsCritical = random.Next(100) < CritChance;

            if (IsCritical)
            {
                damage = (int)(damage * CritDamage);
            }
            return damage;
        }

        public virtual void Use(Character target)
        {
            int damage = CriticalDamage();

            target.TakeDamage(damage);
        }
        public virtual bool Move(int x, int y)
        {
            if (MovePoint <= 0)
            {return false;}

            Position = new Position(Position.X + x, Position.Y + y);

            MovePoint--;

            return true;
        }

        public virtual void StartTurn()
        {
            MovePoint = MaxMovePoint;
        }

        public int GetDistance(Character target)
        {
            int distanceX = Math.Abs(Position.X - target.Position.X);

            int distanceY = Math.Abs(Position.Y - target.Position.Y);

            return Math.Max(distanceX, distanceY);
        }
    }
}