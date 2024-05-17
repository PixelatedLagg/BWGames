namespace Battleship
{
    public class Program
    {
        public static (int, int)[][] ships1 = new (int, int)[5][];
        public static (int, int)[][] ships2 = new (int, int)[5][];
        public static char[,] Board = new char[10, 10]; //0 = none, 1 = player1, 2 = player2, 3 = both

        public static void Main()
        {
            Do.Something();
            return;
            for (int i = 0; i < 10; i++) //reset board
            {
                for (int j = 0; j < 10; j++)
                {
                    Board[i, j] = '-';
                }
            }
            Console.WriteLine("Welcome to Battleship! Press any key to begin.\n");
            Helper.original = (Console.CursorLeft, Console.CursorTop);
            Console.ReadKey();
            Helper.SetShips(ships1);
            //Helper.SetShips(ref ships2);
        }
    }
}