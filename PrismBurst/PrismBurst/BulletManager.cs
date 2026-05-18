using PrismBurst.Enum;
using PrismBurst.Scenes;

namespace PrismBurst
{
    internal class BulletManager
    {
        protected Player player;
        protected Monster monster;
        protected TileManager tileManager;
        protected DamageManager damageManager;
        protected BattleLog battleLog;
        protected GameScene gameScene;
        protected int stage;

        public int Angle { get; protected set; }
        public int Power { get; protected set; }
        public int PlayerBattleX { get; protected set; }
        public int MonsterBattleX { get; protected set; }

        protected bool powerUP;
        protected int battleMoveCount;
        protected int maxBattleMove;

        private const int playerY = 20;
        private const int monsterY = 20;
        private const int groundY = 20;

        public BulletManager(
            Player player,
            Monster monster,
            TileManager tileManager,
            DamageManager damageManager,
            BattleLog battleLog,
            GameScene gameScene, int stage)
        {
            this.player = player;
            this.monster = monster;
            this.tileManager = tileManager;
            this.damageManager = damageManager;
            this.battleLog = battleLog;
            this.gameScene = gameScene;     
            this.stage = stage;

            Angle = 45;
            Power = 50;
            PlayerBattleX = 10;
            MonsterBattleX = 100;           
            maxBattleMove = 20;

            powerUP = true;

            battleMoveCount = maxBattleMove;
        }

        public void Aim()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);

                gameScene.Draw(player, monster, tileManager, Angle, Power, PlayerBattleX,
                    MonsterBattleX, battleLog, stage);

                if (powerUP)
                {
                    Power += 2;

                    if (Power >= 100)
                    {
                        Power = 100;

                        powerUP = false;
                    }
                }
                else
                {
                    Power -= 2;

                    if (Power <= 0)
                    {
                        Power = 0;

                        powerUP = true;
                    }
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.A:

                            if (battleMoveCount > 0)
                            {
                                PlayerBattleX--;

                                if (PlayerBattleX < 2)
                                {
                                    PlayerBattleX = 2;
                                }
                                else
                                {
                                    battleMoveCount--;
                                }
                            }

                            break;

                        case ConsoleKey.D:

                            if (battleMoveCount > 0)
                            {
                                PlayerBattleX++;

                                if (PlayerBattleX > 60)
                                {
                                    PlayerBattleX = 60;
                                }
                                else
                                {
                                    battleMoveCount--;
                                }
                            }

                            break;

                        case ConsoleKey.Q:

                            Angle++;

                            if (Angle > 90)
                            {
                                Angle = 90;
                            }

                            break;

                        case ConsoleKey.E:

                            Angle--;

                            if (Angle < 0)
                            {
                                Angle = 0;
                            }

                            break;

                        case ConsoleKey.Spacebar:

                            Shoot();

                            return;
                    }
                }

                Thread.Sleep(30);
            }
        }

        public void Shoot()
        {
            double bulletX = PlayerBattleX;
            double bulletY = playerY;

            double radian = Angle * Math.PI / 180;
            double speedPower = Power / 35.0;
            double velocityX = Math.Cos(radian) * speedPower * 0.75;
            double velocityY = -Math.Sin(radian) * speedPower * 0.55;

            double gravity = 0.05;

            for (int i = 0; i < 100; i++)
            {
                (bulletX, bulletY, velocityY) =
                    MoveBullet(bulletX, bulletY, velocityX, velocityY, gravity);

                if (!TryGetBulletPosition(bulletX, bulletY, out int drawX, out int drawY))
                {
                    break;
                }

                if (IsOutOfRange(drawX, drawY))
                {
                    break;
                }

                if (drawY >= groundY + 3)
                {
                    DrawExplosion(drawX, groundY);

                    break;
                }

                Console.SetCursorPosition(0, 0);

                gameScene.Draw(player, monster, tileManager, Angle, Power, PlayerBattleX,
                    MonsterBattleX, battleLog, stage);

                DrawBullet(drawX, drawY, player.ColorMagic);

                if (IsHit(bulletX, bulletY, MonsterBattleX, monsterY))
                {
                    DrawHitEffect(MonsterBattleX, monsterY);

                    int damage = damageManager.CalculateDamage(player, monster, battleLog);

                    monster.TakeDamage(damage);

                    Thread.Sleep(300);

                    break;
                }

                Thread.Sleep(25);
            }
            battleMoveCount = maxBattleMove;
        }

        public void MonsterShoot(int monsterDamage, bool isCritical)
        {
            double bulletX = MonsterBattleX;
            double bulletY = monsterY;
           
            double dx = PlayerBattleX - MonsterBattleX; 

            double gravity = 0.05;

            double velocityX = -1.1;
            double frames = Math.Abs(dx) / Math.Abs(velocityX);

          
            double velocityY = ((playerY - monsterY) - 0.5 * gravity * frames * frames) / frames;

            for (int i = 0; i < 300; i++)
            {
                bulletX += velocityX;
                bulletY += velocityY;
                velocityY += gravity;

                int drawX = (int)Math.Round(bulletX);
                int drawY = (int)Math.Round(bulletY);

                if (drawX < 0 || drawX > 120 || drawY < 0 || drawY > 40)
                    break;

                if (drawY >= groundY)
                {
                    DrawExplosion(drawX, groundY);
                    break;
                }

                Console.SetCursorPosition(0, 0);

                gameScene.Draw(player, monster, tileManager, Angle, Power, PlayerBattleX,
                     MonsterBattleX, battleLog, stage);

                Console.SetCursorPosition(drawX, drawY);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("★");
                Console.ResetColor();

                if (Math.Abs(bulletX - PlayerBattleX) <= 3.0 &&
                    Math.Abs(bulletY - playerY) <= 2.0)
                {
                    DrawHitEffect(PlayerBattleX, playerY);
                    Thread.Sleep(300);
                    break;
                }

                Thread.Sleep(22);
            }
        }

        public void DrawBullet(int x, int y, MagicType magicType)
        {
            Console.SetCursorPosition(x, y);

            switch (magicType)
            {
                case MagicType.Fire:

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("◎");

                    break;

                case MagicType.Ice:

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("◆");

                    break;

                case MagicType.Thunder:

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("★");

                    break;

                case MagicType.Poison:

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("♣");

                    break;

            }
        

            Console.ResetColor();
        }

        public void DrawExplosion(int x, int y)
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write("BOOM!");

            Console.ResetColor();
        }

        public void DrawHitEffect(int x, int y)
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("HIT!!");

            Console.ResetColor();
        }

        public (double, double, double)
            MoveBullet(double bulletX, double bulletY, double velocityX, double velocityY,
            double gravity)
        {
            bulletX += velocityX;

            bulletY += velocityY;

            velocityY += gravity;

            return (bulletX, bulletY, velocityY);
        }

        public bool TryGetBulletPosition(double bulletX, double bulletY, 
            out int drawX, out int drawY)
        {
            drawX = (int)Math.Round(bulletX);

            drawY = (int)Math.Round(bulletY);

            return !(drawX < 0 || drawY < 0);
        }

        public bool IsOutOfRange(int x, int y)
        {
            return x < 0 || y < 0 || x > 120 || y > 40;
        }
        
        public void SyncMonsterBattleX(int tileX, int mapWidth)
        {
            int minBattleX = 20;
            int maxBattleX = 80;

            
            MonsterBattleX = maxBattleX - (int)((double)tileX / (mapWidth - 1) *
                (maxBattleX - minBattleX));

            if (MonsterBattleX < minBattleX)
                MonsterBattleX = minBattleX;
            if (MonsterBattleX > maxBattleX)
                MonsterBattleX = maxBattleX;
        }

        public bool IsHit(double bulletX, double bulletY, double targetX, double targetY)
        {
            double distanceX = Math.Abs(bulletX - targetX);

            double distanceY = Math.Abs(bulletY - targetY);

            return distanceX <= 2.0 && distanceY <= 1.5;
        }
    }
}