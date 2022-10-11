class Program
{
    static HttpClient client = new HttpClient();
    public static async Task Main()
    {
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
            string guess = Console.ReadLine() ?? "";
            if (guess == target)
            {
                correct = true;
                Console.WriteLine($" [{guess[0]}]  [{guess[1]}]  [{guess[2]}]  [{guess[3]}]  [{guess[4]}]\nYou've guessed the word in {i + 1} {(i == 0 ? "try" : "tries")}!");
                return;
            }
            bool[] filled = new bool[5];
            for (int j = 0; j < 5; j++)
            {
                bool output = false;
                if (guess[j] == target[j])
                {
                    Console.Write($" [{guess[j]}] ");
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
                        Console.Write($" ({guess[j]}) ");
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
            Console.WriteLine($"The word was \"{target}\".");
        }
        Console.WriteLine("Play again? (y or n)");
        char c = Console.ReadKey().KeyChar;
        Console.Clear();
        if (c == 'y' || c == 'Y')
        {
            await Main();
        }
    }
}