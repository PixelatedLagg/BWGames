class Program
{
    static HttpClient client = new HttpClient();
    static int cursorTopText = Console.CursorTop + 7;
    public static async Task Main()
    {
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        int cursorTop = Console.CursorTop;
        string target = "";
        HttpResponseMessage response = await client.GetAsync("https://random-word-api.herokuapp.com/word?length=5");
        if (response.IsSuccessStatusCode)
        {
            using (HttpContent content = response.Content)
            {
                Task<string> result = content.ReadAsStringAsync();
                target = result.Result.Substring(2, result.Result.Length - 4);
                content.Dispose();
            }
        }
        bool correct = false;
        for (int i = 0; i < 6; i++)
        {
            Console.CursorTop = cursorTopText;
            Console.Write("Guess:      ");
            Console.CursorLeft -= 5;
            string guess = Console.ReadLine() ?? "";
            while (guess.Length > 5)
            {
                Console.CursorTop = cursorTopText;
                Console.Write($"Guess: {new string(' ', guess.Length)}");
                Console.CursorLeft -= guess.Length;
                guess = Console.ReadLine() ?? "";
            }
            Console.CursorTop = cursorTop + i;
            if (guess == target)
            {
                correct = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  {guess[0]}    {guess[1]}    {guess[2]}    {guess[3]}    {guess[4]}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"You've guessed the word in {i + 1} {(i == 0 ? "try" : "tries")}!");
                return;
            }
            bool[] filled = new bool[5];
            for (int j = 0; j < 5; j++)
            {
                bool output = false;
                if (guess[j] == target[j])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"  {guess[j]}  ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    filled[j] = true;
                    continue;
                }
                for (int k = 0; k < 5; k++)
                {
                    if (filled[k])
                    {
                        continue;
                    }
                    if (target[k] == guess[j])
                    {
                        if (target[k] == guess[k])
                        {
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"  {guess[j]}  ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        filled[k] = true;
                        output = true;
                        break;
                    }
                }
                if (!output)
                {
                    Console.Write("  #  ");
                }
            }
            Console.WriteLine();
        }
        if (!correct)
        {
            Console.CursorTop = cursorTopText;
            Console.WriteLine($"The word was \"{target}\".");
        }
    }
    static void OnProcessExit(object? sender, EventArgs e)
    {
        Console.SetCursorPosition(cursorTopText + 2, 0);
    }
}