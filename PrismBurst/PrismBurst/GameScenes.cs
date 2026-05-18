using PrismBurst.Enum;
using System;

namespace PrismBurst.Scenes
{
    internal class GameScene
    {
        public void Draw(Player player, Monster monster, TileManager tileManager, int angle,
            int power, int playerBattleX, int monsterBattleX, BattleLog battleLog, int stage)
        {
            Frame();
            PlayerUI(player);
            MonsterUI(monster);
            BattleArea(player, monster, angle, power, playerBattleX, monsterBattleX);
            TileMap(tileManager, player, monster);
            MagicInfo(player);
            Log(battleLog);
            StageGame(stage);

            Console.SetCursorPosition(90, 38);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("TURN STATUS");

            Console.SetCursorPosition(90, 40);

            Console.ForegroundColor = ConsoleColor.Cyan;

            if (player.MovePoint > 0)
            {
                Console.Write("타일 이동 중");
            }
            else
            {
                Console.Write("공격 준비 중");
            }

            Console.ResetColor();
        }


        private void Frame()
        {
            Console.SetCursorPosition(0, 0);

            Console.WriteLine(
                "┌─────────────────────────" +
                "──────────────────────────" +
                "──────────────────────────" +
                "──────────────────────────" +
                "─────────────┐");
            Console.WriteLine(
                "│                                                Prism Burst" +
                "                                                         │");
            Console.WriteLine(
                "├──────────────────────────" +
                "───────────────────────────" +
                "───────────────────────────" +
                "───────────────────────────" +
                "─────────┤");

            for (int i = 0; i < 21; i++)
            {
                Console.WriteLine(
                    "│                                                 " +
                    "                                                   " +
                    "                │");
            }
            Console.WriteLine(
                "├───────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "─────┤");

            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine(
                    "│                                                   " +
                    "                                                     " +
                    "            │");
            }

            Console.WriteLine(
                "├───────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "─────┤");

            Console.WriteLine(
                "│                                                      " +
                "                                                         " +
                "     │");
            Console.WriteLine(
                "│                                                      " +
                "                                                         " +
                "     │"); 

            Console.WriteLine(
                "│                                                      " +
                "                                                         " +
                "     │");

            Console.WriteLine(
                "└───────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "────────────────────────────" +
                "─────┘");
        }

        private void PlayerUI(Player player)
        {
            Console.SetCursorPosition(2, 3);
            Console.Write("PLAYER");

            Console.SetCursorPosition(2, 4);
            Console.Write($"이름 : {player.Name}");

            Console.SetCursorPosition(2, 5);
            Console.Write($"LEVEL : {player.Level}");

            Console.SetCursorPosition(2, 6);

            string hpGauge = new string('|', player.Hp * 20 / player.MaxHp);

            string hpEmpty = new string('-', 20 - player.Hp * 20 / player.MaxHp);

            Console.Write($"HP [{hpGauge}{hpEmpty}] {player.Hp} / {player.MaxHp}");

            Console.SetCursorPosition(2, 7);
            Console.Write($"공격력 : {player.Attack}");

            Console.SetCursorPosition(2, 8);
            Console.Write($"방어력 : {player.Shield}");

            Console.SetCursorPosition(2, 9);
            Console.Write($"속도 : {player.Speed}");

            Console.SetCursorPosition(2, 10);
            Console.Write($"셔플횟수 : {player.ShuffleCount}");

            Console.SetCursorPosition(2, 11);
            Console.Write($"EXP : {player.Exp} / {player.MaxExp()}");

            Console.SetCursorPosition(2, 12);
            Console.Write($"GOLD : {player.Gold}");
            Console.SetCursorPosition(2, 13);

            Console.ForegroundColor = ConsoleColor.Cyan;

            if (player.State == StateType.None)
            {
                Console.Write("상태 : NORMAL");
            }
            else
            {
                Console.Write($"상태 : {player.State}");
            }

            Console.ResetColor();
        }

        private void MonsterUI(Monster monster)
        {
            Console.SetCursorPosition(80, 3);
            Console.Write("MONSTER");

            Console.SetCursorPosition(80, 4);
            Console.Write($"이름 : {monster.Name}");

            Console.SetCursorPosition(80, 5);
            Console.Write($"타입 : {monster.Type}");

            Console.SetCursorPosition(80, 6);

            string hpGauge = new string('|', monster.Hp * 20 / monster.MaxHp);

            string hpEmpty = new string('-', 20 - monster.Hp * 20 / monster.MaxHp);

            Console.Write($"HP [{hpGauge}{hpEmpty}] {monster.Hp} / {monster.MaxHp}");

            Console.SetCursorPosition(80, 7);
            Console.Write($"공격력 : {monster.Attack}");

            Console.SetCursorPosition(80, 8);
            Console.Write($"방어력 : {monster.Shield}");

            Console.SetCursorPosition(80, 9);
            Console.Write($"속도 : {monster.Speed}");

            Console.SetCursorPosition(80, 11);

            switch (monster.DangerState)
            {
                case DangerLevel.Warning:

                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write("WARNING");

                    break;

                case DangerLevel.Critical:

                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write("CRITICAL");

                    break;

                case DangerLevel.Berserk:

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.Write("BERSERK");

                    break;

                default:

                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("NORMAL");

                    break;
            }
            Console.SetCursorPosition(80, 12);

            Console.ForegroundColor = ConsoleColor.Magenta;

            if (monster.State == StateType.None)
            {
                Console.Write("상태이상 : NORMAL");
            }
            else
            {
                Console.Write($"상태이상 : {monster.State}");
            }
            Console.SetCursorPosition(80, 10);

            switch (monster.DangerState)
            {
                case DangerLevel.Warning:

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("적이 경계 중...");
                    break;

                case DangerLevel.Critical:

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("적이 흉폭해졌다!");
                    break;

                case DangerLevel.Berserk:

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("적이 광폭화했다!");
                    break;

                default:

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("적이 플레이어를 탐색 중");
                    break;
            }

            Console.ResetColor();
        }

        private void BattleArea(Player player, Monster monster, int angle, int power,
            int playerBattleX, int monsterBattleX)
        {
            Console.SetCursorPosition(playerBattleX, 20);

            Console.Write("P ");

            Console.SetCursorPosition(monsterBattleX, 20);

            Console.Write("M ");

            Console.SetCursorPosition(2, 22);
            Console.Write($"Angle : {angle}");

            string powerGauge =
                new string('|', power / 5);

            string powerEmpty =
                new string('.', 20 - power / 5);

            Console.SetCursorPosition(2, 23);

            Console.Write($"POWER : [{powerGauge}{powerEmpty}] {power}");
        }

        private void TileMap(TileManager tileManager, Player player, Monster monster)
        {
            Console.SetCursorPosition(9, 25);

            Console.Write("< TILE MAP >");

            tileManager.DrawMap(player, monster);
        }

        private void MagicInfo(Player player)
        {
            Console.SetCursorPosition(65, 27);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("=== COMBO / MAGIC INFO ===");

            Console.SetCursorPosition(65, 29);

            switch (player.ColorMagic)
            {
                case MagicType.Fire:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("현재 속성 : FIRE");
                    break;

                case MagicType.Ice:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("현재 속성 : ICE");
                    break;

                case MagicType.Thunder:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("현재 속성 : THUNDER");
                    break;

                case MagicType.Poison:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("현재 속성 : POISON");
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("현재 속성 : NONE");
                    break;
            }

            Console.ResetColor();

            Console.SetCursorPosition(65, 31);
            Console.Write($"현재 콤보 : x{player.ComboCount}");

            Console.SetCursorPosition(65, 33);

            string moveGauge =
                new string('■', player.MovePoint) +
                new string('□', player.MaxMovePoint - player.MovePoint);

            Console.Write($"MOVE : [{moveGauge}]");

            Console.SetCursorPosition(35, 26);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[1] FIRE");

            Console.SetCursorPosition(35, 27);
            Console.ResetColor();
            Console.Write("추가 데미지 증가");

            Console.SetCursorPosition(35, 29);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[2] ICE");

            Console.SetCursorPosition(35, 30);
            Console.ResetColor();
            Console.Write("보호막 생성");

            Console.SetCursorPosition(35, 32);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[3] THUNDER");

            Console.SetCursorPosition(35, 33);
            Console.ResetColor();
            Console.Write("확률적으로 스턴");

            Console.SetCursorPosition(35, 35);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[4] POISON");

            Console.SetCursorPosition(35, 36);
            Console.ResetColor();
            Console.Write("지속 피해 부여");

            Console.ResetColor();
        }
        private void Log(BattleLog battleLog)
        {
            Console.SetCursorPosition(2, 38);

            Console.Write(new string(' ', 100));

            Console.SetCursorPosition(2, 39);

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write("[W][A][S][D] 이동 / [SPACE] 발사 / [Q][E] 각도조절 / [1~4] 속성변경");

            Console.ResetColor();

            
            Console.SetCursorPosition(2, 40);

            Console.Write(new string(' ', 100));

            Console.SetCursorPosition(2, 43);

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($"LOG : {battleLog.GetMessage()}");

            Console.ResetColor();
        }
        private void StageGame(int stage)
        {
            Console.SetCursorPosition(50, 3);

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($"STAGE : {stage}");

            Console.ResetColor();
        }

    }
      
}