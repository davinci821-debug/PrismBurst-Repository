namespace PrismBurst
{
    internal class TitleScene
    {
        private string[] menus = { "새게임", "계속하기", "옵션", "종료" };
        private int selectIndex = 0;
        public void Run()
        {
            while (true)
            {
                Draw();
                Input();
            }
        }
        private void Draw()
        {
            Console.Clear();
            DrawTitle();
            DrawMenu();
        }
        private void DrawTitle()
        {
            Console.SetCursorPosition(40, 3);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("P");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("R");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("I");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("S");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("M");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("B");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("U");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("R");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("S");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("I");

            Console.SetCursorPosition(50, 4);
            Console.Write("컬러매지션");
            Console.ResetColor();
        }
        private void DrawMenu()
        {
            for(int i = 0; i < menus.Length; i++)
            {
                Console.SetCursorPosition(45, 10 + i);

                if (i == selectIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(">  ");
                }
                else
                {
                    Console.Write(' ');
                }
                Console.Write(menus[i]);
                Console.ResetColor();
            }
        }
        private void Input()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow :
                    selectIndex--;
                    if (selectIndex < 0)
                    {
                        selectIndex = menus.Length - 1;                        
                    }
                    break;
                case ConsoleKey.DownArrow :
                    selectIndex++;
                    if(selectIndex >= menus.Length)
                    {
                        selectIndex = 0;                        
                    }
                    break;
                case ConsoleKey.Enter :
                    SelectMenu();
                    break;
            }
        }
        private void SelectMenu()
        {
            switch(selectIndex)
            {
                case 0:

                    BattleManager battleManager = new BattleManager();

                    battleManager.Start();

                    return;
                case 3 :
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
