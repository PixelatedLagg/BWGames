class Program
{
    static Random random = new Random();
    static char[,] board = new char[10, 20];
    static char currentSymbol;
    static (int, int)[] currentTetromino = new (int, int)[4];
    static int lines = 0;
    static int position = 4;
    static int currentRotation = 0;
    static char[,] tetrominosBoard = new char[6, 10];
    static int[] nextTetrominos = new int[3];
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
        nextTetrominos[0] = random.Next(0, 7);
        nextTetrominos[1] = random.Next(0, 7);
        nextTetrominos[2] = random.Next(0, 7);
        NewTetromino();
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (currentTetromino[i].Item2 == 19 || (board[currentTetromino[i].Item1, currentTetromino[i].Item2 + 1] != '.' && !currentTetromino.Contains((currentTetromino[i].Item1, currentTetromino[i].Item2 + 1))))
                {
                    if (!NewTetromino())
                    {
                        goto GameOver;
                    }
                }
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                ConsoleKey input = (await ReadKeyAsync(cancellationTokenSource.Token)).Key;
                switch (input)
                {
                    case ConsoleKey.LeftArrow: //unable to when 4
                    case ConsoleKey.A:
                        int i = 0;
                        for (; i < 4; i++)
                        {
                            if (currentTetromino[i].Item1 == 0 || (board[currentTetromino[i].Item1 - 1, currentTetromino[i].Item2] != '.' && !currentTetromino.Contains((currentTetromino[i].Item1 - 1, currentTetromino[i].Item2))))
                            {
                                Console.WriteLine($"broke from {currentTetromino[i].Item1},  {currentTetromino[i].Item2}");
                                break;
                            }
                        }
                        if (i != 4)
                        {
                            Console.WriteLine("cant move left!");
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
                    case ConsoleKey.RightArrow: //unable to when 13
                    case ConsoleKey.D:
                        int j = 0;
                        for (; j < 4; j++)
                        {
                            if (currentTetromino[j].Item1 == 9 || (board[currentTetromino[j].Item1 + 1, currentTetromino[j].Item2] != '.' && !currentTetromino.Contains((currentTetromino[j].Item1 + 1, currentTetromino[j].Item2))))
                            {
                                Console.WriteLine($"broke from {currentTetromino[j].Item1},  {currentTetromino[j].Item2}");
                                break;
                            }
                        }
                        if (j != 4)
                        {
                            Console.WriteLine("cant move right!");
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
                return;
            }
        }
    }
    static bool NewTetromino()
    {
        switch (GetNextTetromino())
        {
            case 0:
                if (position == 0)
                {
                    if (board[0, 0] == '.' || board[0, 1] == '.' || board[0, 2] == '.'|| board[0, 3] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (0, 0);
                    currentTetromino[1] = (1, 0);
                    currentTetromino[2] = (2, 0);
                    currentTetromino[3] = (3, 0);
                }
                else if (position == 9)
                {
                    if (board[0, 0] == '.' || board[0, 1] == '.' || board[0, 2] == '.'|| board[0, 3] == '.')
                    {
                        return false;
                    }
                    position = 7;
                    currentTetromino[0] = (6, 0);
                    currentTetromino[1] = (7, 0);
                    currentTetromino[2] = (8, 0);
                    currentTetromino[3] = (9, 0);
                }
                else
                {
                    if (board[position - 1, 0] == '.' || board[position, 0] == '.' || board[position + 1, 0] == '.' || board[position + 2, 0] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position - 1, 0);
                    currentTetromino[1] = (position, 0);
                    currentTetromino[2] = (position + 1, 0);
                    currentTetromino[3] = (position + 2, 0);
                    board[position - 1, 0] = '#';
                    board[position, 0] = '#';
                    board[position + 1, 0] = '#';
                    board[position + 2, 0] = '#';
                }
                currentRotation = 0;
                currentSymbol = '#';
                return true;
            case 1:
                if (position == 0)
                {
                    if (board[0, 0] == '.' || board[0, 1] == '.' || board[1, 1] == '.' || board[2, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (0, 0);
                    currentTetromino[1] = (0, 1);
                    currentTetromino[2] = (1, 1);
                    currentTetromino[3] = (2, 1);
                    board[0, 0] = '&';
                    board[0, 1] = '&';
                    board[1, 1] = '&';
                    board[2, 1] = '&';
                }
                else if (position == 9)
                {
                    if (board[7, 0] == '.' || board[7, 1] == '.' || board[8, 1] == '.' || board[9, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (7, 0);
                    currentTetromino[1] = (7, 1);
                    currentTetromino[2] = (8, 1);
                    currentTetromino[3] = (9, 1);
                    board[7, 0] = '&';
                    board[7, 1] = '&';
                    board[8, 1] = '&';
                    board[9, 1] = '&';
                }
                else
                {
                    if (board[position - 1, 0] == '.' || board[position - 1, 1] == '.' || board[position, 1] == '.' || board[position + 1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position - 1, 0);
                    currentTetromino[1] = (position - 1, 1);
                    currentTetromino[2] = (position, 1);
                    currentTetromino[3] = (position + 1, 1);
                    board[position - 1, 0] = '&';
                    board[position - 1, 1] = '&';
                    board[position, 1] = '&';
                    board[position + 1, 1] = '&';
                }
                currentRotation = 0;
                currentSymbol = '&';
                return true;
            case 2:
                if (position == 0)
                {
                    if (board[2, 0] == '.' || board[2, 1] == '.' || board[1, 1] == '.' || board[0, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (2, 0);
                    currentTetromino[1] = (2, 1);
                    currentTetromino[2] = (1, 1);
                    currentTetromino[3] = (0, 1);
                    board[2, 0] = '@';
                    board[2, 1] = '@';
                    board[1, 1] = '@';
                    board[0, 1] = '@';
                }
                else if (position == 9)
                {
                    if (board[9, 0] == '.' || board[9, 1] == '.' || board[8, 1] == '.' || board[7, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (9, 0);
                    currentTetromino[1] = (9, 1);
                    currentTetromino[2] = (8, 1);
                    currentTetromino[3] = (7, 1);
                    board[9, 0] = '@';
                    board[9, 1] = '@';
                    board[8, 1] = '@';
                    board[7, 1] = '@';
                }
                else
                {
                    if (board[position + 1, 0] == '.' || board[position + 1, 1] == '.' || board[position, 1] == '.' || board[position - 1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position + 1, 0);
                    currentTetromino[1] = (position + 1, 1);
                    currentTetromino[2] = (position, 1);
                    currentTetromino[3] = (position - 1, 1);
                    board[position + 1, 0] = '@';
                    board[position + 1, 1] = '@';
                    board[position, 1] = '@';
                    board[position - 1, 1] = '@';
                }
                currentRotation = 0;
                currentSymbol = '@';
                return true;
            case 3:
                if (position == 0)
                {
                    if (board[0, 0] == '.' || board[0, 1] == '.' || board[1, 0] == '.' || board[1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (0, 0);
                    currentTetromino[1] = (0, 1);
                    currentTetromino[2] = (1, 0);
                    currentTetromino[3] = (1, 1);
                    board[0, 0] = '?';
                    board[0, 1] = '?';
                    board[1, 0] = '?';
                    board[1, 1] = '?';
                }
                else if (position == 9)
                {
                    if (board[8, 0] == '.' || board[8, 1] == '.' || board[9, 0] == '.' || board[9, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (8, 0);
                    currentTetromino[1] = (8, 1);
                    currentTetromino[2] = (9, 0);
                    currentTetromino[3] = (9, 1);
                    board[8, 0] = '?';
                    board[8, 1] = '?';
                    board[9, 0] = '?';
                    board[9, 1] = '?';
                }
                else
                {
                    if (board[position - 1, 0] == '.' || board[position - 1, 1] == '.' || board[position, 0] == '.' || board[position, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position - 1, 0);
                    currentTetromino[1] = (position - 1, 1);
                    currentTetromino[2] = (position, 0);
                    currentTetromino[3] = (position, 1);
                    board[position - 1, 0] = '?';
                    board[position - 1, 1] = '?';
                    board[position, 0] = '?';
                    board[position, 1] = '?';
                }
                currentSymbol = '?';
                break;
            case 4:
                if (position == 0)
                {
                    if (board[2, 0] == '.' || board[1, 0] == '.' || board[1, 1] == '.' || board[0, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (2, 0);
                    currentTetromino[1] = (1, 0);
                    currentTetromino[2] = (1, 1);
                    currentTetromino[3] = (0, 1);
                    board[2, 0] = '$';
                    board[1, 0] = '$';
                    board[1, 1] = '$';
                    board[0, 1] = '$';
                }
                else if (position == 9)
                {
                    if (board[9, 0] == '.' || board[8, 0] == '.' || board[8, 1] == '.' || board[7, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (9, 0);
                    currentTetromino[1] = (8, 0);
                    currentTetromino[2] = (8, 1);
                    currentTetromino[3] = (7, 1);
                    board[9, 0] = '$';
                    board[8, 0] = '$';
                    board[8, 1] = '$';
                    board[7, 1] = '$';
                }
                else
                {
                    if (board[position + 1, 0] == '.' || board[position, 0] == '.' || board[position, 1] == '.' || board[position - 1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position + 1, 0);
                    currentTetromino[1] = (position, 0);
                    currentTetromino[2] = (position, 1);
                    currentTetromino[3] = (position - 1, 1);
                    board[position + 1, 0] = '$';
                    board[position, 0] = '$';
                    board[position, 1] = '$';
                    board[position - 1, 1] = '$';
                }
                currentRotation = 0;
                currentSymbol = '$';
                break;
            case 5:
                if (position == 0)
                {
                    if (board[0, 0] == '.' || board[1, 0] == '.' || board[1, 1] == '.' || board[2, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (0, 0);
                    currentTetromino[1] = (1, 0);
                    currentTetromino[2] = (1, 1);
                    currentTetromino[3] = (2, 1);
                    board[0, 0] = '=';
                    board[1, 0] = '=';
                    board[1, 1] = '=';
                    board[2, 1] = '=';
                }
                else if (position == 9)
                {
                    if (board[7, 0] == '.' || board[8, 0] == '.' || board[8, 1] == '.' || board[9, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (7, 0);
                    currentTetromino[1] = (8, 0);
                    currentTetromino[2] = (8, 1);
                    currentTetromino[3] = (9, 1);
                    board[7, 0] = '=';
                    board[8, 0] = '=';
                    board[8, 1] = '=';
                    board[9, 1] = '=';
                }
                else
                {
                    if (board[position - 1, 0] == '.' || board[position, 0] == '.' || board[position, 1] == '.' || board[position + 1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position - 1, 0);
                    currentTetromino[1] = (position, 0);
                    currentTetromino[2] = (position, 1);
                    currentTetromino[3] = (position + 1, 1);
                    board[position - 1, 0] = '=';
                    board[position, 0] = '=';
                    board[position, 1] = '=';
                    board[position + 1, 1] = '=';
                }
                currentRotation = 0;
                currentSymbol = '=';
                break;
            case 6:
                if (position == 0)
                {
                    if (board[0, 1] == '.' || board[1, 0] == '.' || board[1, 1] == '.' || board[2, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (0, 1);
                    currentTetromino[1] = (1, 0);
                    currentTetromino[2] = (1, 1);
                    currentTetromino[3] = (2, 1);
                    board[0, 1] = 'x';
                    board[1, 0] = 'x';
                    board[1, 1] = 'x';
                    board[2, 1] = 'x';
                }
                else if (position == 9)
                {
                    if (board[7, 1] == '.' || board[8, 0] == '.' || board[8, 1] == '.' || board[9, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (7, 1);
                    currentTetromino[1] = (8, 0);
                    currentTetromino[2] = (8, 1);
                    currentTetromino[3] = (9, 1);
                    board[7, 1] = 'x';
                    board[8, 0] = 'x';
                    board[8, 1] = 'x';
                    board[9, 1] = 'x';
                }
                else
                {
                    if (board[position - 1, 1] == '.' || board[position, 0] == '.' || board[position, 1] == '.' || board[position + 1, 1] == '.')
                    {
                        return false;
                    }
                    currentTetromino[0] = (position - 1, 1);
                    currentTetromino[1] = (position, 0);
                    currentTetromino[2] = (position, 1);
                    currentTetromino[3] = (position + 1, 1);
                    board[position - 1, 1] = 'x';
                    board[position, 0] = 'x';
                    board[position, 1] = 'x';
                    board[position + 1, 1] = 'x';
                }
                currentRotation = 0;
                currentSymbol = 'x';
                break;
        }
        return false;
    }
    static int GetNextTetromino()
    {
        int result = nextTetrominos[0];
        nextTetrominos[0] = nextTetrominos[1];
        nextTetrominos[1] = nextTetrominos[2];
        nextTetrominos[2] = random.Next(0, 7);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tetrominosBoard[i, j] = '.';
            }
        }
        int position = 1;
        foreach (int tetromino in nextTetrominos)
        {
            switch (tetromino)
            {
                case 0:
                    tetrominosBoard[1, position] = '#';
                    tetrominosBoard[2, position] = '#';
                    tetrominosBoard[3, position] = '#';
                    tetrominosBoard[4, position] = '#';
                    position += 3;
                    break;
                case 1:
                    tetrominosBoard[1, position] = '&';
                    position++;
                    tetrominosBoard[1, position] = '&';
                    tetrominosBoard[2, position] = '&';
                    tetrominosBoard[3, position] = '&';
                    position += 2;
                    break;
                case 2:
                    tetrominosBoard[3, position] = '@';
                    position++;
                    tetrominosBoard[1, position] = '@';
                    tetrominosBoard[2, position] = '@';
                    tetrominosBoard[3, position] = '@';
                    position += 2;
                    break;
                case 3:
                    tetrominosBoard[1, position] = '?';
                    tetrominosBoard[2, position] = '?';
                    position++;
                    tetrominosBoard[1, position] = '?';
                    tetrominosBoard[2, position] = '?';
                    position += 2;
                    break;
                case 4:
                    tetrominosBoard[2, position] = '$';
                    tetrominosBoard[3, position] = '$';
                    position++;
                    tetrominosBoard[1, position] = '$';
                    tetrominosBoard[2, position] = '$';
                    position += 2;
                    break;
                case 5:
                    tetrominosBoard[1, position] = 'x';
                    tetrominosBoard[2, position] = 'x';
                    position++;
                    tetrominosBoard[2, position] = 'x';
                    tetrominosBoard[3, position] = 'x';
                    position += 2;
                    break;
                case 6:
                    tetrominosBoard[2, position] = '=';
                    position++;
                    tetrominosBoard[1, position] = '=';
                    tetrominosBoard[2, position] = '=';
                    tetrominosBoard[3, position] = '=';
                    position += 2;
                    break;
            }
        }
        Console.WriteLine($"new tet: {result}");
        return result;
    }
    static async Task<ConsoleKeyInfo> ReadKeyAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(true);
                }
                await Task.Delay(100, cancellationToken);
            }
        }
        catch
        {
            return default;
        }
        return default;
    }
    static void Render()
    {
        Console.Clear();
        for (int j = 0; j < 4; j++)
        {
            board[currentTetromino[j].Item1, currentTetromino[j].Item2] = currentSymbol;
        }
        int i = 0;
        for (; i < 10; i++)
        {
            Console.WriteLine($" {board[0, i]} {board[1, i]} {board[2, i]} {board[3, i]} {board[4, i]} {board[5, i]} {board[6, i]} {board[7, i]} {board[8, i]} {board[9, i]}    {tetrominosBoard[0, i]} {tetrominosBoard[1, i]} {tetrominosBoard[2, i]} {tetrominosBoard[3, i]} {tetrominosBoard[4, i]} {tetrominosBoard[5, i]}");
        }
        for (; i < 20; i++)
        {
            Console.WriteLine($" {board[0, i]} {board[1, i]} {board[2, i]} {board[3, i]} {board[4, i]} {board[5, i]} {board[6, i]} {board[7, i]} {board[8, i]} {board[9, i]}");
        }
        Console.WriteLine(position);
    }
}

/*

# # # #

&
& & &

    @
@ @ @

?  ?
?  ?

   $  $
$  $

x  x
   x  x

   =
=  =  =
*/

/*
. . . . . .
. # # # # .
. . . . . .
. . . . . .
. & . . . .
. & & & . .
. . . . . .
. ? ? . . .
. ? ? . . .
. . . . . .
*/