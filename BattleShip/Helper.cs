/*

carrier = 5
battleship = 4
cruiser = 3
sub = 3
destroyer = 2

*/



namespace Battleship
{
    public static class Helper
    {
        public static (int x, int y) original;
        public static void PrintBoard<T>(T[,] board) //positions are (x, y) * 2
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{board[i, 0]} {board[i, 1]} {board[i, 2]} {board[i, 3]} {board[i, 4]} {board[i, 5]} {board[i, 6]} {board[i, 7]} {board[i, 8]} {board[i, 9]}");
            }
        }

        public static (int, int)[][] SetShips((int, int)[][] ships)
        {
            ReplaceText("First, set the carrier position!\n");
            ships[0] = PlaceShip([(3, 5), (4, 5), (5, 5), (6, 5), (7, 5)]);
            ReplaceText("Next, set the battleship position!\n");
            ships[1] = PlaceShip([(3, 5), (4, 5), (5, 5), (6, 5)]);
            ReplaceText("Next, set the cruiser position!\n");
            ships[2] = PlaceShip([(3, 5), (4, 5), (5, 5)]);
            ReplaceText("Next, set the sub position!\n");
            ships[3] = PlaceShip([(3, 5), (4, 5), (5, 5)]);
            ReplaceText("Next, set the destroyer position!\n");
            ships[4] = PlaceShip([(4, 5), (5, 5)]);
            return ships;
        }

        public static (int x, int y)[] PlaceShip((int x, int y)[] ship)
        {
            char[,] board = new char[10, 10];
            for (int i = 0; i < 10; i++) //reset board
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = '-';
                }
            }
            foreach ((int x, int y) in ship)
            {
                board[x, y] = '#';
            }
            PrintBoard(board);
            (int x, int y)[] previous = new (int x, int y)[ship.Length];
            while (true)
            {
                ship.CopyTo(previous, 0);
                ConsoleKey input = Console.ReadKey().Key;
                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        for (int i = 0; i < ship.Length; i++)
                        {
                            if (ship[i].x == 0) //not allowed
                            {
                                goto next;
                            }
                        }
                        for (int i = 0; i < ship.Length; i++)
                        {
                            ship[i] = (ship[1].x - 1, ship[i].y);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        for (int i = 0; i < ship.Length; i++)
                        {
                            if (ship[i].x == 9) //not allowed
                            {
                                goto next;
                            }
                        }
                        for (int i = 0; i < ship.Length; i++)
                        {
                            ship[i] = (ship[1].x + 1, ship[i].y);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        for (int i = 0; i < ship.Length; i++)
                        {
                            if (ship[i].y == 0) //not allowed
                            {
                                goto next;
                            }
                        }
                        for (int i = 0; i < ship.Length; i++)
                        {
                            ship[i] = (ship[1].x, ship[i].y - 1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        for (int i = 0; i < ship.Length; i++)
                        {
                            if (ship[i].y == 9) //not allowed
                            {
                                goto next;
                            }
                        }
                        for (int i = 0; i < ship.Length; i++)
                        {
                            ship[i] = (ship[1].x, ship[i].y + 1);
                        }
                        break;
                    case ConsoleKey.Enter: //submit values for ship
                        return ship;
                    default:
                        goto next;
                }
                foreach ((int x, int y) in previous)
                {
                    Console.SetCursorPosition(x * 2 + original.x, y + original.y);
                    Console.Write('-');
                }
                foreach ((int x, int y) in ship)
                {
                    Console.SetCursorPosition(x * 2 + original.x, y + original.y);
                    Console.Write('#');
                }
                next:;
            }
        }
        public static void ReplaceText(string text)
        {
            Console.SetCursorPosition(0, original.y - 2);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, original.y);
            Console.WriteLine(text);
        }
    }
}