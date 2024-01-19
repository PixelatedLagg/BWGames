namespace Tetris
{
    public static class Piece
    {
        private static readonly Random random = new();
        public static (int, int)[] GetCoords(int x, int y, Pieces pieces)
        {
            (int, int)[] result = new (int, int)[4];
            switch (pieces)
            {
                case Pieces.Line:
                    for (int i = 0; i < 4; i++)
                    {
                        result[i] = (x - i, y);
                    }
                    break;
                case Pieces.FrontL:
                    result[0] = (x - 1, y);
                    for (int i = -1; i < 2; i++)
                    {
                        result[i + 2] = (x + i, y + 1);
                    }
                    break;
                case Pieces.BackL:
                    result[0] = (x + 1, y);
                    for (int i = -1; i < 2; i++)
                    {
                        result[i + 2] = (x + i, y + 1);
                    }
                    break;
                case Pieces.Square:
                    result[0] = (x, y);
                    result[1] = (x + 1, y);
                    result[2] = (x, y + 1);
                    result[3] = (x + 1, y + 1);
                    break;
                case Pieces.S:
                    result[0] = (x, y);
                    result[1] = (x + 1, y);
                    result[2] = (x, y + 1);
                    result[3] = (x - 1, y + 1);
                    break;
                case Pieces.Z:
                    result[0] = (x, y);
                    result[1] = (x - 1, y);
                    result[2] = (x, y + 1);
                    result[3] = (x + 1, y + 1);
                    break;
                default: //Pieces.T
                    result[0] = (x, y);
                    for (int i = -1; i < 2; i++)
                    {
                        result[i + 2] = (x + i, y + 1);
                    }
                    break;
            }
            return result;
        }
        public static Pieces Random()
        {
            Pieces[] pieces = (Pieces[])Enum.GetValues(typeof(Pieces));
            return pieces[random.Next(0, 7)];
        }
        public static (int, int)[] Rotate((int, int)[] piece, int x, int y)
        {
            (int, int)[] result = new (int, int)[4];
            for (int i = 0; i < 4; i++)
            {
                result[i].Item1 = -1 * (piece[i].Item2 - y) + x;
                result[i].Item2 = piece[i].Item1 - x + y;
            }
            return result;
        }
        public static bool InBounds((int, int)[] piece)
        {
            foreach ((int x, int y) in piece)
            {
                if (x < 1 || x > 10 || y > 19) //ignore when piece is too high (buffer zone)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public enum Pieces
    {
        Line = 11, //CYAN
        FrontL = 9, //BLUE
        BackL = 12, //RED
        Square = 14, //YELLOW
        S = 10, //GREEN
        Z = 4, //DARK RED
        T = 13 //MAGENTA
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