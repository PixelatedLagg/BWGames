class Program
{
    static Random random = new Random();
    static char[,] board = new char[10, 20];
    static char currentSymbol;
    static (int, int)[] currentTetromino = new (int, int)[4];
    static int lines = 0;
    static int position = 4;
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
        bool activeTetromino = true;
        NewTetromino();
        while (true)
        {
            if (!activeTetromino)
            {
                if (board[0, position] != '.')
                {
                    break;
                }
                NewTetromino();
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            ConsoleKey input = (await ReadKeyAsync(cancellationTokenSource.Token)).Key;
            switch (input)
            {
                case ConsoleKey.A: //left
                    int i = 0;
                    for (; i < 4; i++)
                    {
                        if (currentTetromino[i].Item1 == 0)
                        {
                            break;
                        }
                    }
                    if (i != 3)
                    {
                        break;
                    }
                    for (i = 0; i < 4; i++)
                    {
                        board[currentTetromino[i].Item1, currentTetromino[i].Item2] = '.';
                        currentTetromino[i].Item1--;
                        position--;
                        board[currentTetromino[i].Item1, currentTetromino[i].Item2] = currentSymbol;
                    }
                    break;
                case ConsoleKey.D: //right
                    int j = 0;
                    for (; j < 4; j++)
                    {
                        if (currentTetromino[j].Item1 == 9)
                        {
                            break;
                        }
                    }
                    if (j != 3)
                    {
                        break;
                    }
                    for (j = 0; j < 4; j++)
                    {
                        board[currentTetromino[j].Item1, currentTetromino[j].Item2] = '.';
                        currentTetromino[j].Item1++;
                        position++;
                        board[currentTetromino[j].Item1, currentTetromino[j].Item2] = currentSymbol;
                    }
                    break;
                case ConsoleKey.LeftArrow: //rotation -90 degrees
                    break;
                case ConsoleKey.RightArrow: //rotation 90 degrees
                    break;
            }
            for (int i = 0; i < 4; i++)
            {
                if (currentTetromino[i].Item2 == 19 || board[currentTetromino[i].Item1, currentTetromino[i].Item2 + 1] != '.')
                {
                    activeTetromino = false;
                    break;
                }
            }
            if (activeTetromino)
            {
                for (int i = 0; i < 4; i++)
                {
                    board[currentTetromino[i].Item1, currentTetromino[i].Item2] = '.';
                    currentTetromino[i].Item1++;
                    board[currentTetromino[i].Item1, currentTetromino[i].Item2] = currentSymbol;
                }
            }
            Console.Clear();
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($" {board[0, i]} {board[1, i]} {board[2, i]} {board[3, i]} {board[4, i]} {board[5, i]} {board[6, i]} {board[7, i]} {board[8, i]} {board[9, i]}");
            }
            Console.WriteLine("render!");
        }
        Console.WriteLine($"You've reached {lines} {(lines == 1 ? "line" : "lines")}!\nPlay again? (y or n)");
        char c = Console.ReadKey(true).KeyChar;
        Console.Clear();
        if (c == 'y' || c == 'Y')
        {
            await Main();
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
                currentTetromino[0] = (position - 3, 0);
                currentTetromino[1] = (position - 3, 1);
                currentTetromino[2] = (position - 3, 2);
                currentTetromino[3] = (position - 3, 3);
                return true;
            /*case 1:
                currentSymbol = '&';
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

.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
.  .  .  .  .  .  .  .  .  .
*/