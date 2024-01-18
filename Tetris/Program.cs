#pragma warning disable
namespace Tetris
{
    class Program
    {
        static ConsoleColor currentColor;
        static (int, int)[] currentPiece = new (int, int)[4];
        static int currentRotation = 0;
        static readonly (int, int)[][] nextPieces = new (int, int)[3][];
        static int cursorTop, cursorLeftSide, cursorBottom;
        static int x, y;
        static readonly int cursorLeftMain = 5; //10 = width (20 cells)
        public static async Task Main()
        {
            if (OperatingSystem.IsWindows()) //buffer height is windows only
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
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                        case ConsoleKey.D:
                            break;
                        case ConsoleKey.DownArrow: //DOWN
                        case ConsoleKey.S:
                            break;
                        case ConsoleKey.Spacebar: //SPAM DOWN
                            await Task.Delay(500);
                            break;
                        case ConsoleKey.UpArrow: //ROTATE
                        case ConsoleKey.W:
                            currentRotation++;
                            currentPiece = Piece.Rotate(currentPiece, currentRotation, x, y);
                            break;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(currentPiece[i].Item1 * 2, currentPiece[i].Item2);
                    Console.Write(' ');
                    currentPiece[i] = (currentPiece[i].Item1, currentPiece[i].Item2 + 1);
                    y++;
                }
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
        static void RenderPiece()
        {
            Console.ForegroundColor = currentColor;
            foreach ((int x, int y) in currentPiece)
            {
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
            Console.SetCursorPosition(0, cursorTop + 22 + debugList);
            Console.WriteLine(text);
            Console.SetCursorPosition(previousLeft, previousTop);
            Console.ForegroundColor = previous;
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
}