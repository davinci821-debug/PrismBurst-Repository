using PrismBurst.Enum;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrismBurst
{
    internal class MonsterManager
    {
        protected List<Monster> monsters;

        public MonsterManager()
        {
            monsters = new List<Monster>();
            monsters.Add(new Monster("슬라임", 100, 15, 5, 3, 3, MonsterType.Normal, 50, 30));

            monsters.Add(new Monster("고블린", 150, 20, 8, 4, 3, MonsterType.Elite, 80, 50));

            monsters.Add(new Monster("오크", 220, 30, 12, 4, 4, MonsterType.Elite, 120, 80));

            monsters.Add(new Monster("드래곤", 500, 50, 20, 5, 5, MonsterType.Boss, 300, 200));
        }
        public Monster StageMonster(int stage)
        {
            int index = stage - 1;
            if(index < 0)
            {
                index = 0;
            }
            if(index > monsters.Count)
            {
                index = monsters.Count - 1;
            }
            Monster monsterData = monsters[index];

            return new Monster(monsterData.Name, monsterData.MaxHp, monsterData.Attack,
                monsterData.Shield, monsterData.Speed, monsterData.MovePoint, 
                monsterData.Type, monsterData.GiveExp, monsterData.GiveGold);
        }
    }    
}
