#pragma warning disable
namespace Tetris
{
    class Program
    {
        static ConsoleColor currentColor;
        static (int, int)[] currentPiece = new (int, int)[4];
        static readonly (int, int)[][] nextPieces = new (int, int)[3][];
        static int cursorTop, cursorLeftSide, cursorBottom;
        static int x, y;
        static readonly int cursorLeftMain = 5; //10 = width (20 cells)
        static HashSet<(int, int)> positionsTaken = new();

        public static async Task Main()
        {
            if (OperatingSystem.IsWindows()) //buffer size is windows only
            {
                Console.BufferHeight = 100;
            }
            cursorTop = Console.CursorTop;
            cursorLeftSide = cursorLeftMain * 4 + 4;
            cursorBottom = cursorTop + 20; //20 = height
            x = cursorLeftMain;
            y = cursorTop;
            for (int i = cursorTop; i < cursorBottom; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write('|');
            }
            Console.CursorTop++;
            Console.CursorLeft = 0;
            Console.Write($"x - - - - - - - - - -x");
            Console.CursorTop = cursorTop;
            for (int i = cursorTop; i < cursorBottom; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 4 * cursorLeftMain + 1;
                Console.Write('|');
            }
            NewPiece();
            RenderPiece();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorTop = cursorTop + 23 + debugList;
            for (int i = 0; i < 3; i++)
            {
                nextPieces[i] = Piece.GetCoords(cursorLeftSide, cursorLeftSide, Piece.Random());
            }
            while (true)
            {
                CancellationTokenSource cancellationTokenSource = new();
                cancellationTokenSource.CancelAfter(1000);
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    ConsoleKey input = (await ReadKeyAsync(cancellationTokenSource.Token)).Key;
                    switch (input)
                    {
                        case ConsoleKey.LeftArrow: //LEFT
                        case ConsoleKey.A:
                            bool verify = true;
                            foreach ((int currentX, int currentY) in currentPiece)
                            {
                                if (currentX == 1)
                                {
                                    verify = false;
                                    break;
                                }
                            }
                            if (verify)
                            {
                                x--;
                                ClearPiece();
                                for (int i = 0; i < 4; i++)
                                {
                                    currentPiece[i] = (currentPiece[i].Item1 - 1, currentPiece[i].Item2);
                                }
                                RenderPiece();
                            }
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                        case ConsoleKey.D:
                            verify = true;
                            foreach ((int currentX, int currentY) in currentPiece)
                            {
                                if (currentX == 10)
                                {
                                    verify = false;
                                    break;
                                }
                            }
                            if (verify)
                            {
                                x++;
                                (int, int)[] temp = currentPiece;
                                ClearPiece();
                                for (int i = 0; i < 4; i++)
                                {
                                    currentPiece[i] = (temp[i].Item1 + 1, temp[i].Item2);
                                }
                                RenderPiece();
                            }
                            break;
                        case ConsoleKey.DownArrow: //DOWN
                        case ConsoleKey.S:
                            verify = true;
                            foreach ((int currentX, int currentY) in currentPiece)
                            {
                                if (currentY == 20)
                                {
                                    verify = false;
                                    break;
                                }
                            }
                            if (verify)
                            {
                                y++;
                                (int, int)[] temp = currentPiece;
                                ClearPiece();
                                for (int i = 0; i < 4; i++)
                                {
                                    currentPiece[i] = (temp[i].Item1, temp[i].Item2 + 1);
                                }
                                RenderPiece();
                            }
                            break;
                        case ConsoleKey.Spacebar: //SPAM DOWN
                            int yTotal = 0;
                            verify = true;
                            while (verify)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (currentPiece[i].Item2 + yTotal == cursorTop + 19 || positionsTaken.Contains((currentPiece[i].Item1, currentPiece[i].Item2 + yTotal)))
                                    {
                                        verify = false;
                                        break;
                                    }
                                }
                                Debug(yTotal);
                                yTotal++;
                            }
                            if (yTotal == 0)
                            {
                                break;
                            }
                            (int, int)[] tempYTotal = currentPiece;
                            ClearPiece();
                            for (int i = 0; i < 4; i++)
                            {
                                currentPiece[i] = (tempYTotal[i].Item1, tempYTotal[i].Item2 + yTotal);
                            }
                            RenderPiece();
                            break;
                        case ConsoleKey.UpArrow: //ROTATE
                        case ConsoleKey.W:
                            (int, int)[] result = Piece.Rotate(currentPiece, x, y);
                            if (Piece.InBounds(result))
                            {
                                ClearPiece();
                                Console.SetCursorPosition(x * 2, y);
                                currentPiece = result;
                                RenderPiece();
                            }
                            break;
                    }
                }
                bool onGround = false;
                foreach ((int currentX, int currentY) in currentPiece)
                {
                    if (currentY == cursorTop + 19 || positionsTaken.Contains((currentX, currentY + 1)))
                    {
                        onGround = true;
                        break;
                    }
                }
                if (onGround)
                {
                    foreach ((int currentX, int currentY) in currentPiece)
                    {
                        positionsTaken.Add((currentX, currentY));
                    }
                    NewPiece();
                    RenderPiece();
                    continue; //skip going down
                }
                ClearPiece();
                for (int i = 0; i < 4; i++)
                {
                    currentPiece[i] = (currentPiece[i].Item1, currentPiece[i].Item2 + 1);
                }
                y++;
                RenderPiece();
            }
            return;
        }
        static void NewPiece()
        {
            Pieces pieces = Piece.Random();
            currentPiece = Piece.GetCoords(cursorLeftMain, cursorTop, pieces);
            currentColor = (ConsoleColor)(int)pieces;
        }
        static void ClearPiece()
        {
            for (int i = 0; i < 4; i++)
            {
                if (currentPiece[i].Item2 < cursorTop) //buffer zone
                {
                    continue;
                }
                Console.SetCursorPosition(currentPiece[i].Item1 * 2, currentPiece[i].Item2);
                Console.Write(' ');
            }
        }
        static void RenderPiece()
        {
            Console.ForegroundColor = currentColor;
            foreach ((int x, int y) in currentPiece)
            {
                if (y < cursorTop) //buffer zone
                {
                    continue;
                }
                Console.SetCursorPosition(x * 2, y);
                Console.Write('#');
            }
            Console.ForegroundColor = ConsoleColor.Gray;
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
        static int debugList = 0;
        public static void Debug(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            debugList++;
            int previousTop = Console.CursorTop, previousLeft = Console.CursorLeft;
            Console.SetCursorPosition(30, cursorTop + debugList);
            Console.WriteLine(text);
            Console.SetCursorPosition(previousLeft, previousTop);
            Console.ForegroundColor = previous;
        }
        public static void  Debug (object obj, ConsoleColor color = ConsoleColor.Gray) => Debug(obj.ToString(), color);
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
}