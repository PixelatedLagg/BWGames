class Program
{
    static string[,] Combinations =
    {
        { "Tie!", "AI Wins: Paper > Rock.", "Player wins: Rock > Scissors." },
        { "Player wins: Paper > Rock.", "Tie!", "AI wins: Scissors > Paper."},
        { "AI wins: Rock > Scissors.", "Player wins: Scissors > Paper.", "Tie!"}
    };
    public static void Main()
    {
        Console.WriteLine("Pick one:\n[1] Rock\n[2] Paper\n[3] Scissors");
        char input = Console.ReadKey(true).KeyChar;
        while (InvalidInput(input))
        {
            input = Console.ReadKey(true).KeyChar;
        }
        int choice = input - '0', aiChoice = new Random().Next(0, 2);
        Console.WriteLine($"\n{Combinations[choice - 1, aiChoice]}\nPlay again? (y or n)");
        char c = Console.ReadKey(true).KeyChar;
        Console.Clear();
        if (c == 'y' || c == 'Y')
        {
            Main();
        }
    }
    static bool InvalidInput(char input)
    {
        if (input != '1' && input != '2' && input != '3')
        {
            return true;
        }
        return false;
    }
}