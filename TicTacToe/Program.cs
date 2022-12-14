class Program
{
    static int[,] Board =
    {
        { 0, 0, 0 },
        { 0, 0, 0 },
        { 0, 0, 0 }
    };
    static (int, int) LastPosition = (1, 1);
    static bool AIFirst = true;
    static int CursorValue;
    public static void Main()
    {
        if (AIFirst)
        {
            AIMove(1, 1); //move to center
            switch (Move())
            {
                case (1, 0):
                    AIMove(0, 0); //move to top left
                    if (Move() != (2, 2))
                    {
                        AIMove(2, 2); //win
                        Win();
                    }
                    AIMove(0, 1); //move to middle right
                    if (Move() != (2, 1))
                    {
                        AIMove(2, 1); //win
                        Win();
                    }
                    else
                    {
                        AIMove(0, 2); //win
                        Win();
                    }
                    break;
                case (0, 1):
                    AIMove(0, 2); //move to bottom left
                    if (Move() != (2, 0))
                    {
                        AIMove(2, 0); //win
                        Win();
                    }
                    AIMove(1, 2); //move to bottom middle
                    if (Move() != (0, 1))
                    {
                        AIMove(0, 1); //win
                        Win();
                    }
                    else
                    {
                        AIMove(2, 2); //win
                        Win();
                    }
                    break;
                case (2, 1):
                    AIMove(2, 2); //move to bottom right
                    if (Move() != (0, 0))
                    {
                        AIMove(0, 0); //win
                        Win();
                    }
                    AIMove(1, 2); //move to bottom middle
                    if (Move() != (1, 0))
                    {
                        AIMove(1, 0); //win
                        Win();
                    }
                    else
                    {
                        AIMove(0, 2); //win
                        Win();
                    }
                    break;
                case (1, 2):
                    AIMove(2, 2); //move to bottom right
                    if (Move() != (0, 0))
                    {
                        AIMove(0, 0); //win
                        Win();
                    }
                    AIMove(2, 1); //move to middle right
                    if (Move() != (0, 1))
                    {
                        AIMove(0, 1); //win
                        Win();
                    }
                    else
                    {
                        AIMove(2, 0); //win
                        Win();
                    }
                    break;
                default:
                    //will have pattern for each corner
                    break;
            }
        }
        else
        {
            Move();
            //different setup for going second
        }
    }

    static void AIMove(int x, int y)
    {
        Board[x, y] = 1;
        Render();
    }
    static (int, int) Move()
    {
        CursorValue = Board[LastPosition.Item1, LastPosition.Item2];
        Board[LastPosition.Item1, LastPosition.Item2] = 3;
        Render();
        ConsoleKey key = Console.ReadKey(true).Key;
        while (true)
        {
            switch (key)
            {
                case ConsoleKey.W:
                    if (LastPosition.Item2 == 0)
                    {
                        break;
                    }
                    Board[LastPosition.Item1, LastPosition.Item2] = CursorValue;
                    LastPosition.Item2--;
                    CursorValue = Board[LastPosition.Item1, LastPosition.Item2];
                    Board[LastPosition.Item1, LastPosition.Item2] = 3;
                    break;
                case ConsoleKey.A:
                    if (LastPosition.Item1 == 0)
                    {
                        break;
                    }
                    Board[LastPosition.Item1, LastPosition.Item2] = CursorValue;
                    LastPosition.Item1--;
                    CursorValue = Board[LastPosition.Item1, LastPosition.Item2];
                    Board[LastPosition.Item1, LastPosition.Item2] = 3;
                    break;
                case ConsoleKey.S:
                    if (LastPosition.Item2 == 2)
                    {
                        break;
                    }
                    Board[LastPosition.Item1, LastPosition.Item2] = CursorValue;
                    LastPosition.Item2++;
                    CursorValue = Board[LastPosition.Item1, LastPosition.Item2];
                    Board[LastPosition.Item1, LastPosition.Item2] = 3;
                    break;
                case ConsoleKey.D:
                    if (LastPosition.Item1 == 2)
                    {
                        break;
                    }
                    Board[LastPosition.Item1, LastPosition.Item2] = CursorValue;
                    LastPosition.Item1++;
                    CursorValue = Board[LastPosition.Item1, LastPosition.Item2];
                    Board[LastPosition.Item1, LastPosition.Item2] = 3;
                    break;
                case ConsoleKey.Spacebar:
                    if (CursorValue != 0)
                    {
                        break;
                    }
                    Board[LastPosition.Item1, LastPosition.Item2] = 2;
                    goto endLoop;
            }
            Render();
            key = Console.ReadKey(true).Key;
        }
        endLoop:
        {
            Render();
            return LastPosition;
        }
    }

    static char ToSymbol(int i)
    {
        return i switch
        {
            0 => ' ',
            1 => AIFirst ? 'X' : 'O',
            2 => AIFirst ? 'O' : 'X',
            _ => '.'
        };
    }

    static void Render()
    {
        Console.Clear();
        Console.WriteLine(" _______________________");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"|       |       |       |\n|   {ToSymbol(Board[0, i])}   |   {ToSymbol(Board[1, i])}   |   {ToSymbol(Board[2, i])}   |\n|_______|_______|_______|");
        }
    }

    static void Win()
    {
        Console.WriteLine("\nAI Wins!\nPlay again? (y or n)");
        char c = Console.ReadKey(true).KeyChar;
        Console.Clear();
        if (c == 'y' || c == 'Y')
        {
            //AIFirst = !AIFirst;
            LastPosition = (1, 1);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = 0;
                }
            }
            Main();
        }
        else
        {
            Environment.Exit(0);
        }
    }
}