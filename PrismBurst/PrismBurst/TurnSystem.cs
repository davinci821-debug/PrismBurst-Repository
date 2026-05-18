using PrismBurst.Enum;
using PrismBurst.Scenes;

namespace PrismBurst
{
    internal class TurnSystem
    {
        protected Player player;
        protected Monster monster;        
        protected TileManager tileManager;
        protected BattleLog battleLog;
        protected BulletManager bulletManager;
        protected GameScene gameScene;
        protected BattleState battleState;
        protected int stage;
        public BattleState CurrentState
        {
            get { return battleState; }
        }

        public TurnSystem(Player player, Monster monster, TileManager tileManager,
            BattleLog battleLog, BulletManager bulletManager, GameScene gameScene, int stage)
        {
            this.player = player;
            this.monster = monster;
            this.tileManager = tileManager;
            this.battleLog = battleLog;
            this.bulletManager = bulletManager;            
            this.gameScene = gameScene;
            battleState = BattleState.SelectMagic;
            this.stage = stage;
        }

        public BattleState Update()
        {
            
            if (monster.IsDead)
            {
                battleState = BattleState.Win;
            }
            else if (player.IsDead)
            {
                battleState = BattleState.Lose;
            }

            switch (battleState)
            {
                case BattleState.SelectMagic:

                    SelectMagic();

                    break;

                case BattleState.MoveTile:

                    MoveTile();

                    break;

                case BattleState.Aim:

                    bulletManager.Aim();
                   
                    if (monster.IsDead)
                    {
                        battleState = BattleState.Win;
                    }
                    else
                    {
                        battleState = BattleState.MonsterTurn;
                    }

                    break;

                case BattleState.MonsterTurn:

                    MonsterTurn();

                    break;
            }

            return battleState;
        }

        public void SelectMagic()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:

                    player.ColorMagic = MagicType.Fire;

                    break;

                case ConsoleKey.D2:

                    player.ColorMagic = MagicType.Ice;

                    break;

                case ConsoleKey.D3:

                    player.ColorMagic = MagicType.Thunder;

                    break;

                case ConsoleKey.D4:

                    player.ColorMagic = MagicType.Poison;

                    break;

                default:

                    return;
            }

            battleLog.SetMessage($"{player.ColorMagic} 속성 활성화");

            battleState = BattleState.MoveTile;
        }

        public void MoveTile()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            int nextX = player.Position.X;

            int nextY = player.Position.Y;

            bool moved = false;

            if (player.ComboCount >= 3)
            {
                battleLog.SetMessage($"🔥 {player.ComboCount} COMBO BURST!");
            }
            else
            {
                battleLog.SetMessage($"{player.ComboCount} COMBO");
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.Spacebar:

                    battleState = BattleState.Aim;

                    return;

                case ConsoleKey.W:
                    nextY--;
                    break;

                case ConsoleKey.S:
                    nextY++;
                    break;

                case ConsoleKey.A:
                    nextX--;
                    break;

                case ConsoleKey.D:
                    nextX++;
                    break;
            }

            if (!tileManager.IsInside(nextX, nextY))
            {
                return;
            }

            TileType targetTile = tileManager.GetTile(nextX, nextY);

            TileType currentType = GetTileType();

            if (targetTile == TileType.Empty)
            {
                return;
            }

            if (targetTile != currentType)
            {
                return;
            }

            moved = player.Move(nextX - player.Position.X, nextY - player.Position.Y);

            if (moved)
            {
                tileManager.SaveTile(nextX, nextY);

                player.ComboCount = tileManager.GetComboCount();

                battleLog.SetMessage($"{player.ComboCount} COMBO!");
            }

        }

        public TileType GetTileType()
        {
            switch (player.ColorMagic)
            {
                case MagicType.Fire:
                    return TileType.Red;

                case MagicType.Ice:
                    return TileType.Blue;

                case MagicType.Thunder:
                    return TileType.Yellow;

                case MagicType.Poison:
                    return TileType.Green;
            }

            return TileType.None;
        }

        public void MonsterTurn()
        {
           
            monster.StartTurn();

            battleLog.SetMessage("몬스터 턴");

            Console.SetCursorPosition(0, 0);

            gameScene.Draw(player, monster, tileManager, bulletManager.Angle, 
                bulletManager.Power, bulletManager.PlayerBattleX, bulletManager.MonsterBattleX,
                battleLog, stage);

            Thread.Sleep(400);

            int finalDistance = monster.GetDistance(player);

            for (int step = 0; step < monster.MaxMovePoint; step++)
            {
                finalDistance = monster.GetDistance(player);

                if (finalDistance <= 1)
                    break;          

               
                int nextX = monster.Position.X;
                int nextY = monster.Position.Y;

                if (player.Position.X < monster.Position.X)
                    nextX--;
                else if (player.Position.X > monster.Position.X)
                    nextX++;
                else if (player.Position.Y < monster.Position.Y)
                    nextY--;
                else if (player.Position.Y > monster.Position.Y)
                    nextY++;
                                
                if (!tileManager.IsInside(nextX, nextY))
                    break;
                                
                TileType nextTile = tileManager.GetTile(nextX, nextY);

                if (nextTile == TileType.Empty)
                    break;
                                
                tileManager.RemoveTile(
                    monster.Position.X,
                    monster.Position.Y);
               
                monster.Move(nextX - monster.Position.X, nextY - monster.Position.Y);

              
                bulletManager.SyncMonsterBattleX(monster.Position.X, tileManager.Width);
                              
                battleLog.SetMessage($"{monster.Name} 이동 중... 거리 : {monster.GetDistance(player)}");

                Console.SetCursorPosition(0, 0);

                gameScene.Draw(player, monster, tileManager, bulletManager.Angle,
                    bulletManager.Power, bulletManager.PlayerBattleX, bulletManager.MonsterBattleX,
                    battleLog, stage);

                Thread.Sleep(350);
            }
           
            monster.UpdateDangerState(finalDistance);

            int damage = monster.CriticalDamage();

            string dangerPrefix = monster.DangerState switch
            {
                DangerLevel.Berserk  => "[광폭화] ",
                DangerLevel.Critical => "[흉폭화] ",
                DangerLevel.Warning  => "[경계] ",
                _                    => ""
            };

            string critText = monster.IsCritical ? "크리티컬! " : "";

            battleLog.SetMessage($"{dangerPrefix}{critText} {monster.Name} 공격!");
            
            bulletManager.MonsterShoot(damage, monster.IsCritical);

            player.TakeDamage(damage);

            battleLog.SetMessage($"{dangerPrefix}{critText}" +
                                 $"{monster.Name} 공격! " +
                                 $"{damage} 데미지!");

            Console.SetCursorPosition(0, 0);

            gameScene.Draw(player, monster, tileManager, bulletManager.Angle,
                bulletManager.Power, bulletManager.PlayerBattleX, bulletManager.MonsterBattleX,
                battleLog, stage);

            Thread.Sleep(1000);

            player.StartTurn();

            tileManager.RefillEmptyTiles();

            battleState = BattleState.SelectMagic;
        }
    }
}