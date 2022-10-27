class Program
{
    static Random random = new Random();
    static char[,] board = new char[10, 20];
    static char currentSymbol;
    static (int, int)[] current = new (int, int)[4];
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
                if (current[i].Item2 == 19 || (board[current[i].Item1, current[i].Item2 + 1] != '.' && !current.Contains((current[i].Item1, current[i].Item2 + 1))))
                {
                    if (!NewTetromino())
                    {
                        goto GameOver;
                    }
                    break;
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
                            if (current[i].Item1 == 0 || (board[current[i].Item1 - 1, current[i].Item2] != '.' && !current.Contains((current[i].Item1 - 1, current[i].Item2))))
                            {
                                break;
                            }
                        }
                        if (i != 4)
                        {
                            break;
                        }
                        position--;
                        Clear();
                        for (i = 0; i < 4; i++)
                        {
                            current[i].Item1--;
                            board[current[i].Item1, current[i].Item2] = currentSymbol;
                        }
                        Render();
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        int j = 0;
                        for (; j < 4; j++)
                        {
                            if (current[j].Item1 == 9 || (board[current[j].Item1 + 1, current[j].Item2] != '.' && !current.Contains((current[j].Item1 + 1, current[j].Item2))))
                            {
                                break;
                            }
                        }
                        if (j != 4)
                        {
                            break;
                        }
                        position++;
                        Clear();
                        for (j = 0; j < 4; j++)
                        {
                            current[j].Item1++;
                            board[current[j].Item1, current[j].Item2] = currentSymbol;
                        }
                        Render();
                        break;
                    case ConsoleKey.Spacebar:
                        while (true)
                        {
                            int d = 0;
                            for (; d < 4; d++)
                            {
                                if (current[d].Item2 == 19 || (board[current[d].Item1, current[d].Item2 + 1] != '.' && !current.Contains((current[d].Item1, current[d].Item2 + 1))))
                                {
                                    break;
                                }
                            }
                            if (d != 4)
                            {
                                break;
                            }
                            Clear();
                            for (d = 0; d < 4; d++)
                            {
                                current[d].Item2++;
                                board[current[d].Item1, current[d].Item2] = currentSymbol;
                            }
                        }
                        goto CallRender;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        switch (currentSymbol)
                        {
                            case '#':
                                switch (currentRotation)
                                {
                                    case 0:
                                        if (current[0].Item2 == 0 || current[0].Item2 > 17 || Full(current[0].Item1 + 2, current[0].Item2 - 1) || Full(current[1].Item1 + 1, current[1].Item2) || Full(current[2].Item1, current[2].Item2 + 1) || Full(current[3].Item1 - 1, current[3].Item2 + 2))
                                        {
                                            break;
                                        }
                                        Clear();
                                        current[0].Item1 += 2;
                                        current[0].Item2--;
                                        current[1].Item1++;
                                        current[2].Item2++;
                                        current[3].Item1--;
                                        current[3].Item2 += 2;
                                        Update();
                                        position++;
                                        currentRotation = 1;
                                        break;
                                    case 1:
                                        if (current[0].Item1 == 9 || current[0].Item1 < 2 || board[current[0].Item1 - 2, current[0].Item2 + 2] != '.' || board[current[1].Item1 - 1, current[1].Item2 + 1] != '.' || board[current[3].Item1 + 1, current[3].Item2 - 1] != '.')
                                        {
                                            break;
                                        }
                                        Clear();
                                        current[0].Item1 -= 2;
                                        current[0].Item2 += 2;
                                        current[1].Item1--;
                                        current[1].Item2++;
                                        current[3].Item1++;
                                        current[3].Item2--;
                                        Update();
                                        position--;
                                        currentRotation = 2;
                                        break;
                                    case 2:
                                        if (current[0].Item1 == 9 || current[0].Item1 < 2 || board[current[0].Item1 - 2, current[0].Item2 + 2] != '.' || board[current[1].Item1 - 1, current[1].Item2 + 1] != '.' || board[current[3].Item1 + 1, current[3].Item2 - 1] != '.')
                                        {
                                            break;
                                        }
                                        Clear();

                                        Update();
                                        position++;
                                        currentRotation = 3;
                                        break;
                                    case 3:
                                        currentRotation = 0;
                                        break;
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
                        Render();
                        break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                board[current[i].Item1, current[i].Item2] = '.';
            }
            for (int i = 0; i < 4; i++)
            {
                current[i].Item2++;
                board[current[i].Item1, current[i].Item2] = currentSymbol;
            }
            CallRender:
            {
                Render();
            }
            foreach ((int, int) position in current)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (board[i, position.Item2] == '.')
                    {
                        goto end;
                    }
                }
                for (int i = position.Item2 - 1; i > 0; i--)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        board[j, i + 1] = board[j, i];
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    board[i, 0] = '.';
                }
                lines++;
                end:
                {
                    continue;
                }
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
    static void Clear()
    {
        foreach ((int, int) position in current)
        {
            board[position.Item1, position.Item2] = '.';
        }
    }
    static void Update()
    {
        foreach ((int, int) position in current)
        {
            board[position.Item1, position.Item2] = currentSymbol;
        }
    }
    static bool NewTetromino()
    {
        switch (GetNextTetromino())
        {
            case 0:
                if (position == 0)
                {
                    if (board[0, 0] != '.' || board[1, 0] != '.' || board[2, 0] != '.'|| board[3, 0] != '.')
                    {
                        return false;
                    }
                    current[0] = (0, 0);
                    current[1] = (1, 0);
                    current[2] = (2, 0);
                    current[3] = (3, 0);
                    board[0, 0] = '#';
                    board[1, 0] = '#';
                    board[2, 0] = '#';
                    board[3, 0] = '#';
                    break;
                }
                if (position == 9 || position == 8)
                {
                    position = 7;
                }
                if (board[position - 1, 0] != '.' || board[position, 0] != '.' || board[position + 1, 0] != '.' || board[position + 2, 0] != '.')
                {
                    return false;
                }
                current[0] = (position - 1, 0);
                current[1] = (position, 0);
                current[2] = (position + 1, 0);
                current[3] = (position + 2, 0);
                board[position - 1, 0] = '#';
                board[position, 0] = '#';
                board[position + 1, 0] = '#';
                board[position + 2, 0] = '#';
                currentRotation = 0;
                currentSymbol = '#';
                return true;
            case 1:
                if (position == 0)
                {
                    if (board[0, 0] != '.' || board[0, 1] != '.' || board[1, 1] != '.' || board[2, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (0, 0);
                    current[1] = (0, 1);
                    current[2] = (1, 1);
                    current[3] = (2, 1);
                    board[0, 0] = '&';
                    board[0, 1] = '&';
                    board[1, 1] = '&';
                    board[2, 1] = '&';
                }
                else if (position == 9)
                {
                    if (board[7, 0] != '.' || board[7, 1] != '.' || board[8, 1] != '.' || board[9, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (7, 0);
                    current[1] = (7, 1);
                    current[2] = (8, 1);
                    current[3] = (9, 1);
                    board[7, 0] = '&';
                    board[7, 1] = '&';
                    board[8, 1] = '&';
                    board[9, 1] = '&';
                }
                else
                {
                    if (board[position - 1, 0] != '.' || board[position - 1, 1] != '.' || board[position, 1] != '.' || board[position + 1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position - 1, 0);
                    current[1] = (position - 1, 1);
                    current[2] = (position, 1);
                    current[3] = (position + 1, 1);
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
                    if (board[2, 0] != '.' || board[2, 1] != '.' || board[1, 1] != '.' || board[0, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (2, 0);
                    current[1] = (2, 1);
                    current[2] = (1, 1);
                    current[3] = (0, 1);
                    board[2, 0] = '@';
                    board[2, 1] = '@';
                    board[1, 1] = '@';
                    board[0, 1] = '@';
                }
                else if (position == 9)
                {
                    if (board[9, 0] != '.' || board[9, 1] != '.' || board[8, 1] != '.' || board[7, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (9, 0);
                    current[1] = (9, 1);
                    current[2] = (8, 1);
                    current[3] = (7, 1);
                    board[9, 0] = '@';
                    board[9, 1] = '@';
                    board[8, 1] = '@';
                    board[7, 1] = '@';
                }
                else
                {
                    if (board[position + 1, 0] != '.' || board[position + 1, 1] != '.' || board[position, 1] != '.' || board[position - 1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position + 1, 0);
                    current[1] = (position + 1, 1);
                    current[2] = (position, 1);
                    current[3] = (position - 1, 1);
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
                    if (board[0, 0] != '.' || board[0, 1] != '.' || board[1, 0] != '.' || board[1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (0, 0);
                    current[1] = (0, 1);
                    current[2] = (1, 0);
                    current[3] = (1, 1);
                    board[0, 0] = '?';
                    board[0, 1] = '?';
                    board[1, 0] = '?';
                    board[1, 1] = '?';
                }
                else if (position == 9)
                {
                    if (board[8, 0] != '.' || board[8, 1] != '.' || board[9, 0] != '.' || board[9, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (8, 0);
                    current[1] = (8, 1);
                    current[2] = (9, 0);
                    current[3] = (9, 1);
                    board[8, 0] = '?';
                    board[8, 1] = '?';
                    board[9, 0] = '?';
                    board[9, 1] = '?';
                }
                else
                {
                    if (board[position - 1, 0] != '.' || board[position - 1, 1] != '.' || board[position, 0] != '.' || board[position, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position - 1, 0);
                    current[1] = (position - 1, 1);
                    current[2] = (position, 0);
                    current[3] = (position, 1);
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
                    if (board[2, 0] != '.' || board[1, 0] != '.' || board[1, 1] != '.' || board[0, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (2, 0);
                    current[1] = (1, 0);
                    current[2] = (1, 1);
                    current[3] = (0, 1);
                    board[2, 0] = '$';
                    board[1, 0] = '$';
                    board[1, 1] = '$';
                    board[0, 1] = '$';
                }
                else if (position == 9)
                {
                    if (board[9, 0] != '.' || board[8, 0] != '.' || board[8, 1] != '.' || board[7, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (9, 0);
                    current[1] = (8, 0);
                    current[2] = (8, 1);
                    current[3] = (7, 1);
                    board[9, 0] = '$';
                    board[8, 0] = '$';
                    board[8, 1] = '$';
                    board[7, 1] = '$';
                }
                else
                {
                    if (board[position + 1, 0] != '.' || board[position, 0] != '.' || board[position, 1] != '.' || board[position - 1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position + 1, 0);
                    current[1] = (position, 0);
                    current[2] = (position, 1);
                    current[3] = (position - 1, 1);
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
                    if (board[0, 0] != '.' || board[1, 0] != '.' || board[1, 1] != '.' || board[2, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (0, 0);
                    current[1] = (1, 0);
                    current[2] = (1, 1);
                    current[3] = (2, 1);
                    board[0, 0] = '=';
                    board[1, 0] = '=';
                    board[1, 1] = '=';
                    board[2, 1] = '=';
                }
                else if (position == 9)
                {
                    if (board[7, 0] != '.' || board[8, 0] != '.' || board[8, 1] != '.' || board[9, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (7, 0);
                    current[1] = (8, 0);
                    current[2] = (8, 1);
                    current[3] = (9, 1);
                    board[7, 0] = '=';
                    board[8, 0] = '=';
                    board[8, 1] = '=';
                    board[9, 1] = '=';
                }
                else
                {
                    if (board[position - 1, 0] != '.' || board[position, 0] != '.' || board[position, 1] != '.' || board[position + 1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position - 1, 0);
                    current[1] = (position, 0);
                    current[2] = (position, 1);
                    current[3] = (position + 1, 1);
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
                    if (board[0, 1] != '.' || board[1, 0] != '.' || board[1, 1] != '.' || board[2, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (0, 1);
                    current[1] = (1, 0);
                    current[2] = (1, 1);
                    current[3] = (2, 1);
                    board[0, 1] = 'x';
                    board[1, 0] = 'x';
                    board[1, 1] = 'x';
                    board[2, 1] = 'x';
                }
                else if (position == 9)
                {
                    if (board[7, 1] != '.' || board[8, 0] != '.' || board[8, 1] != '.' || board[9, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (7, 1);
                    current[1] = (8, 0);
                    current[2] = (8, 1);
                    current[3] = (9, 1);
                    board[7, 1] = 'x';
                    board[8, 0] = 'x';
                    board[8, 1] = 'x';
                    board[9, 1] = 'x';
                }
                else
                {
                    if (board[position - 1, 1] != '.' || board[position, 0] != '.' || board[position, 1] != '.' || board[position + 1, 1] != '.')
                    {
                        return false;
                    }
                    current[0] = (position - 1, 1);
                    current[1] = (position, 0);
                    current[2] = (position, 1);
                    current[3] = (position + 1, 1);
                    board[position - 1, 1] = 'x';
                    board[position, 0] = 'x';
                    board[position, 1] = 'x';
                    board[position + 1, 1] = 'x';
                }
                currentRotation = 0;
                currentSymbol = 'x';
                break;
        }
        return true;
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
        int newBoardPosition = 1;
        foreach (int tetromino in nextTetrominos)
        {
            switch (tetromino)
            {
                case 0:
                    tetrominosBoard[1, newBoardPosition] = '#';
                    tetrominosBoard[2, newBoardPosition] = '#';
                    tetrominosBoard[3, newBoardPosition] = '#';
                    tetrominosBoard[4, newBoardPosition] = '#';
                    newBoardPosition += 3;
                    break;
                case 1:
                    tetrominosBoard[1, newBoardPosition] = '&';
                    newBoardPosition++;
                    tetrominosBoard[1, newBoardPosition] = '&';
                    tetrominosBoard[2, newBoardPosition] = '&';
                    tetrominosBoard[3, newBoardPosition] = '&';
                    newBoardPosition += 2;
                    break;
                case 2:
                    tetrominosBoard[3, newBoardPosition] = '@';
                    newBoardPosition++;
                    tetrominosBoard[1, newBoardPosition] = '@';
                    tetrominosBoard[2, newBoardPosition] = '@';
                    tetrominosBoard[3, newBoardPosition] = '@';
                    newBoardPosition += 2;
                    break;
                case 3:
                    tetrominosBoard[1, newBoardPosition] = '?';
                    tetrominosBoard[2, newBoardPosition] = '?';
                    newBoardPosition++;
                    tetrominosBoard[1, newBoardPosition] = '?';
                    tetrominosBoard[2, newBoardPosition] = '?';
                    newBoardPosition += 2;
                    break;
                case 4:
                    tetrominosBoard[2, newBoardPosition] = '$';
                    tetrominosBoard[3, newBoardPosition] = '$';
                    newBoardPosition++;
                    tetrominosBoard[1, newBoardPosition] = '$';
                    tetrominosBoard[2, newBoardPosition] = '$';
                    newBoardPosition += 2;
                    break;
                case 5:
                    tetrominosBoard[1, newBoardPosition] = 'x';
                    tetrominosBoard[2, newBoardPosition] = 'x';
                    newBoardPosition++;
                    tetrominosBoard[2, newBoardPosition] = 'x';
                    tetrominosBoard[3, newBoardPosition] = 'x';
                    newBoardPosition += 2;
                    break;
                case 6:
                    tetrominosBoard[2, newBoardPosition] = '=';
                    newBoardPosition++;
                    tetrominosBoard[1, newBoardPosition] = '=';
                    tetrominosBoard[2, newBoardPosition] = '=';
                    tetrominosBoard[3, newBoardPosition] = '=';
                    newBoardPosition += 2;
                    break;
            }
        }
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
            board[current[j].Item1, current[j].Item2] = currentSymbol;
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
        Console.WriteLine($"Lines Completed: {lines}");
    }
    static bool Full(int x, int y)
    {
        if (board[x, y] == '.')
        {
            return false;
        }
        foreach ((int, int) position in current)
        {
            if (position == (x, y))
            {
                return false;
            }
        }
        return true;
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