using PrismBurst.Enum;

namespace PrismBurst
{
    internal class TileManager
    {
        public int Width { get; protected set; }

        public int Height { get; protected set; }

        protected TileType[,] map;

        protected Stack<TileType> moveTiles;

        protected Random random;

        public TileManager(int width, int height)
        {
            Width = width;

            Height = height;

            map = new TileType[Height, Width];

            moveTiles = new Stack<TileType>();

            random = new Random();

            CreateMap();
        }

        public void CreateMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    map[y, x] = GetRandomTile();
                }
            }
        }

        public TileType GetRandomTile()
        {
            int value = random.Next(0, 4);

            switch (value)
            {
                case 0:
                    return TileType.Red;

                case 1:
                    return TileType.Blue;

                case 2:
                    return TileType.Yellow;

                case 3:
                    return TileType.Green;
            }

            return TileType.Red;
        }

        public TileType GetTile(int x, int y)
        {
            return map[y, x];
        }

        public void SaveTile(int x, int y)
        {
            TileType tile = map[y, x];

            moveTiles.Push(tile);

            map[y, x] = TileType.Empty;
        }

        public void RemoveTile(int x, int y)
        {
            map[y, x] = TileType.Empty;
        }

        public void RefillEmptyTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (map[y, x] == TileType.Empty)
                    {
                        map[y, x] = GetRandomTile();
                    }
                }
            }
        }

        public int GetComboCount()
        {
            if (moveTiles.Count == 0)
            {
                return 0;
            }

            TileType[] tiles = moveTiles.ToArray();

            TileType recentTile = tiles[0];

            int combo = 0;

            foreach (TileType tile in tiles)
            {
                if (tile == recentTile)
                {
                    combo++;
                }
                else
                {
                    break;
                }
            }

            return combo;
        }

        public void DrawTile(TileType tile)
        {
            switch (tile)
            {
                case TileType.Red:
                    Console.BackgroundColor =
                        ConsoleColor.Red;
                    break;

                case TileType.Blue:
                    Console.BackgroundColor =
                        ConsoleColor.Blue;
                    break;

                case TileType.Yellow:
                    Console.BackgroundColor =
                        ConsoleColor.Yellow;
                    break;

                case TileType.Green:
                    Console.BackgroundColor =
                        ConsoleColor.Green;
                    break;

                case TileType.Empty:
                    Console.Write("  ");
                    return;
            }

            Console.Write("  ");

            Console.ResetColor();
        }

        public void DrawMap(Player player,
                            Monster monster)
        {
            int startX = 5;

            int startY = 27;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.SetCursorPosition(
                        startX + x * 2,
                        startY + y);

                    if (player.Position.X == x && player.Position.Y == y)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;

                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write("P");

                        Console.ResetColor();
                    }
                    else if (monster.Position.X == x && monster.Position.Y == y)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;

                        Console.ForegroundColor = ConsoleColor.Magenta;

                        Console.Write("M");

                        Console.ResetColor();
                    }
                    else
                    {
                        DrawTile(map[y, x]);
                    }
                }
            }
        }

        public bool IsInside(int x, int y)
        {
            return
                x >= 0 &&
                x < Width &&
                y >= 0 &&
                y < Height;
        }
    }
}