class Program
{
    static Random random = new Random();
    static char[,] board = new char[10, 20];
    static char currentSymbol;
    static (int, int)[] currentTetromino = new (int, int)[4];
    static int lines = 0;
    static int position = 4;
    static int currentRotation = 0;
    public static async Task Main()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                board[i, j] = '.';
            }
        }
        currentSymbol = '.';
        NewTetromino();
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (currentTetromino[i].Item2 == 19 || (board[currentTetromino[i].Item1, currentTetromino[i].Item2 + 1] != '.' && !currentTetromino.Contains((currentTetromino[i].Item1, currentTetromino[i].Item2 + 1))))
                {
                    if (board[position, 0] != '.')
                    {
                        goto GameOver;
                    }
                    NewTetromino();
                }
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                ConsoleKey input = (await ReadKeyAsync(cancellationTokenSource.Token)).Key;
                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        int i = 0;
                        for (; i < 4; i++)
                        {
                            if (currentTetromino[i].Item1 == 0 || (board[currentTetromino[i].Item1 - 1, currentTetromino[i].Item2] != '.' && !currentTetromino.Contains((currentTetromino[i].Item1 - 1, currentTetromino[i].Item2))))
                            {
                                break;
                            }
                        }
                        if (i != 4)
                        {
                            break;
                        }
                        position--;
                        for (i = 0; i < 4; i++)
                        {
                            board[currentTetromino[i].Item1, currentTetromino[i].Item2] = '.';
                        }
                        for (i = 0; i < 4; i++)
                        {
                            currentTetromino[i].Item1--;
                            board[currentTetromino[i].Item1, currentTetromino[i].Item2] = currentSymbol;
                        }
                        Render();
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        int j = 0;
                        for (; j < 4; j++)
                        {
                            if (currentTetromino[j].Item1 == 9 || (board[currentTetromino[j].Item1 + 1, currentTetromino[j].Item2] != '.' && !currentTetromino.Contains((currentTetromino[j].Item1 + 1, currentTetromino[j].Item2))))
                            {
                                break;
                            }
                        }
                        if (j != 4)
                        {
                            break;
                        }
                        position++;
                        for (j = 0; j < 4; j++)
                        {
                            board[currentTetromino[j].Item1, currentTetromino[j].Item2] = '.';
                        }
                        for (j = 0; j < 4; j++)
                        {
                            currentTetromino[j].Item1++;
                            board[currentTetromino[j].Item1, currentTetromino[j].Item2] = currentSymbol;
                        }
                        Render();
                        break;
                    case ConsoleKey.Spacebar:
                        while (true)
                        {
                            int d = 0;
                            for (; d < 4; d++)
                            {
                                if (currentTetromino[d].Item2 == 19 || (board[currentTetromino[d].Item1, currentTetromino[d].Item2 + 1] != '.' && !currentTetromino.Contains((currentTetromino[d].Item1, currentTetromino[d].Item2 + 1))))
                                {
                                    break;
                                }
                            }
                            if (d != 4)
                            {
                                break;
                            }
                            for (d = 0; d < 4; d++)
                            {
                                board[currentTetromino[d].Item1, currentTetromino[d].Item2] = '.';
                            }
                            for (d = 0; d < 4; d++)
                            {
                                currentTetromino[d].Item2++;
                                board[currentTetromino[d].Item1, currentTetromino[d].Item2] = currentSymbol;
                            }
                        }
                        goto CallRender;
                    case ConsoleKey.UpArrow:
                        switch (currentSymbol)
                        {
                            case '#':
                                if (currentRotation == 0)
                                {
                                    if (board[currentTetromino[0].Item1 - 1, currentTetromino[0].Item2 + 1] != '.')
                                    {
                                        break;
                                    }
                                    if (board[currentTetromino[2].Item1 + 1, currentTetromino[2].Item2 - 1] != '.')
                                    {
                                        break;
                                    }
                                    if (board[currentTetromino[3].Item1 + 2, currentTetromino[3].Item2 - 2] != '.')
                                    {
                                        break;
                                    }
                                    for (j = 0; j < 4; j++)
                                    {
                                        board[currentTetromino[j].Item1, currentTetromino[j].Item2] = '.';
                                    }
                                    currentTetromino[0].Item1--;
                                    currentTetromino[0].Item2++;
                                    currentTetromino[2].Item1++;
                                    currentTetromino[2].Item2--;
                                    currentTetromino[3].Item1 += 2;
                                    currentTetromino[3].Item2 -= 2;
                                    currentRotation = 1;
                                    Render();
                                }
                                else
                                {
                                    if (board[currentTetromino[0].Item1 + 1, currentTetromino[0].Item2 - 1] != '.')
                                    {
                                        break;
                                    }
                                    if (board[currentTetromino[2].Item1 - 1, currentTetromino[2].Item2 + 1] != '.')
                                    {
                                        break;
                                    }
                                    if (board[currentTetromino[3].Item1 - 2, currentTetromino[3].Item2 + 2] != '.')
                                    {
                                        break;
                                    }
                                    for (j = 0; j < 4; j++)
                                    {
                                        board[currentTetromino[j].Item1, currentTetromino[j].Item2] = '.';
                                    }
                                    currentTetromino[0].Item1++;
                                    currentTetromino[0].Item2--;
                                    currentTetromino[2].Item1--;
                                    currentTetromino[2].Item2++;
                                    currentTetromino[3].Item1 -= 2;
                                    currentTetromino[3].Item2 += 2;
                                    currentRotation = 0;
                                    Render();
                                }
                                break;
                            case '&':
                                break;
                            case '@':
                                break;
                            case '?':
                                break;
                            case '$':
                                break;
                            case '=':
                                break;
                            case 'x':
                                break;
                        }
                        break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                board[currentTetromino[i].Item1, currentTetromino[i].Item2] = '.';
            }
            for (int i = 0; i < 4; i++)
            {
                currentTetromino[i].Item2++;
                board[currentTetromino[i].Item1, currentTetromino[i].Item2] = currentSymbol;
            }
            CallRender:
            {
                Render();
            }
        }
        GameOver:
        {
            Console.WriteLine($"You've completed {lines} {(lines == 1 ? "line" : "lines")}!\nPlay again? (y or n)");
            char c = Console.ReadKey(true).KeyChar;
            Console.Clear();
            if (c == 'y' || c == 'Y')
            {
                await Main();
            }
        }
    }
    static void Render()
    {
        Console.Clear();
        for (int i = 0; i < 4; i++)
        {
            board[currentTetromino[i].Item1, currentTetromino[i].Item2] = currentSymbol;
        }
        for (int i = 0; i < 20; i++)
        {
            Console.WriteLine($" {board[0, i]} {board[1, i]} {board[2, i]} {board[3, i]} {board[4, i]} {board[5, i]} {board[6, i]} {board[7, i]} {board[8, i]} {board[9, i]}");
        }
    }
    static bool NewTetromino()
    {
        switch (random.Next(0, 6))
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                currentSymbol = '#';
                currentTetromino[0] = (position, 0);
                currentTetromino[1] = (position, 1);
                currentTetromino[2] = (position, 2);
                currentTetromino[3] = (position, 3);
                Console.WriteLine(position);
                board[position, 0] = '#';
                board[position, 1] = '#';
                board[position, 2] = '#';
                board[position, 3] = '#';
                currentRotation = 0;
                return true;
            /*case 1:
                currentSymbol = '&';
                currentTetromino[0] = (position, 0);
                currentTetromino[1] = (position, 1);
                currentTetromino[2] = (position, 2);
                currentTetromino[3] = (position, 3);
                board[position, 0] = '&';
                board[position, 1] = '&';
                board[position, 2] = '&';
                board[position, 3] = '&';
                break;
            case 2:
                currentSymbol = '@';
                break;
            case 3:
                currentSymbol = '?';
                break;
            case 4:
                currentSymbol = '$';
                break;
            case 5:
                currentSymbol = '=';
                break;
            case 6:
                currentSymbol = 'x';
                break;*/
        }
        return false;
    }
    static async Task<ConsoleKeyInfo> ReadKeyAsync(CancellationToken cancellationToken, int responsiveness = 100)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(true);
                }
                await Task.Delay(responsiveness, cancellationToken);
            }
        }
        catch
        {
            return default;
        }
        return default;
    }
}

/*

#  #  #  #

&
&  &  &

      @
@  @  @

?  ?
?  ?

   $  $
$  $

   =
=  =  =

x  x
   x  x

*/