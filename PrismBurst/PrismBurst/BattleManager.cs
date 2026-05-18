using PrismBurst.Enum;
using PrismBurst.Scenes;

namespace PrismBurst
{
    internal class BattleManager
    {
        protected Player player;
        protected Monster monster;
        protected TileManager tileManager;
        protected GameScene gameScene;
        protected DamageManager damageManager;
        protected BattleLog battleLog;
        protected BulletManager bulletManager;    
        protected TurnSystem turnSystem;
        protected MonsterManager monsterManager;
        protected int stage;

        public BattleManager()
        {
            player = new Player();

            monsterManager = new MonsterManager();     

            tileManager = new TileManager(10, 10);

            gameScene = new GameScene();

            damageManager = new DamageManager();

            battleLog = new BattleLog();

            stage = 1;

            LoopBattle();
        }
              
        public void LoopBattle()
        {
            monster = monsterManager.StageMonster(stage);

            bulletManager = new BulletManager(player, monster, tileManager, damageManager,
                battleLog, gameScene, stage);

            turnSystem = new TurnSystem(player, monster, tileManager, battleLog, bulletManager,
                gameScene, stage);

            player.StartTurn();

            tileManager.CreateMap();
        }


        public void Start()
        {
            while (true)
            {
                DrawBattleScene();

                BattleState result = turnSystem.Update();

                if (result == BattleState.Win)
                {
                    stage++;
                    ShowResult(true);
                    LoopBattle();
                    continue;
                }
                if (result == BattleState.Lose)
                {
                    ShowResult(false);
                    return;
                }
            }
        }

        public void ShowResult(bool isWin)
        {
            Console.Clear();

            Console.SetCursorPosition(50, 15);

            if (isWin)
            {
                player.GetExp(monster.GiveExp);

                player.GetGold(monster.GiveGold);

                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("=== VICTORY! ===");

                Console.SetCursorPosition(45, 17);

                Console.WriteLine($"{monster.Name} 을(를) 쓰러뜨렸다!");

                Console.SetCursorPosition(45, 19);

                Console.WriteLine($"획득 경험치 : {monster.GiveExp}");

                Console.SetCursorPosition(45, 21);

                Console.WriteLine($"획득 골드 : {monster.GiveGold}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("=== GAME OVER ===");

                Console.SetCursorPosition(45, 17);

                Console.WriteLine("플레이어가 쓰러졌다...");
            }

            Console.ResetColor();

            Console.SetCursorPosition(45, 25);

            Console.WriteLine("아무 키나 누르면 종료합니다.");

            Console.ReadKey(true);
        }

        public void DrawBattleScene()
        {
            Console.Clear();

            gameScene.Draw(player, monster, tileManager, bulletManager.Angle, 
                bulletManager.Power, bulletManager.PlayerBattleX, bulletManager.MonsterBattleX,
                battleLog, stage);
        }
    }
}