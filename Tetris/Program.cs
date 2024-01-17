#pragma warning disable
namespace Tetris
{
    class Program
    {
        static ConsoleColor currentColor;
        static (int, int)[] currentPiece = new (int, int)[4];
        static readonly (int, int)[][] nextPieces = new (int, int)[3][];
        static int cursorTop, cursorLeftSide, cursorBottom;
        static readonly int cursorLeftMain = 10; //10 = width (20 cells)
        public static async Task Main()
        {
            if (OperatingSystem.IsWindows()) //buffer height is windows only
            {
                Console.BufferHeight = 100;
            }
            cursorTop = Console.CursorTop;
            cursorLeftSide = cursorLeftMain * 2 + 4;
            cursorBottom = cursorTop + 20; //20 = height
            for (int i = 0; i < 20; i++)
            {
                Console.CursorTop++;
                Console.CursorLeft = 0;
                Console.Write('|');
            }
            Console.CursorTop++;
            Console.CursorLeft = 0;
            Console.Write($"x - - - - - - - - - -x");
            Console.CursorTop = cursorTop;
            for (int i = 0; i < 20; i++)
            {
                Console.CursorTop++;
                Console.CursorLeft = cursorLeftMain * 2 + 1;
                Console.Write('|');
            }
            Console.CursorTop = cursorTop + 30;
            NewPiece();
            RenderPiece();
            return;
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
                            break;
                    }
                }
                
            }
        }
        static void NewPiece()
        {
            Pieces pieces = Piece.Random();
            currentPiece = Piece.GetCoords(cursorTop, cursorLeftMain, pieces);
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