using Blackjack;

public class Program
{
    public static Random random = new();
    public static int CursorTop, CursorLeft = 0, CursorTopText;
    public static List<int> Aces = [];
    public static int PlayerTotal = 0;
    public static int PlayerCardNumber = 0, AICardNumber = 0;
    public static int AITotal;
    public static List<string> AICards = [];
    public static void Main()
    {
        if (OperatingSystem.IsWindows()) //buffer height is windows only
        {
            Console.BufferHeight = 100;
        }
        Console.WriteLine("Blackjack (ASCII art by ejm98)\n");
        CursorTop = Console.CursorTop;
        CursorTopText = CursorTop + 7;
        GeneratePlayerCard();
        GeneratePlayerCard();
        do
        {
            if (!HitOrStay())
            {
                return;
            }
        }
        while (GameNotOver());
        return;
    }
    public static void GeneratePlayerCard()
    {
        PlayerCardNumber++;
        int card = random.Next(1, 15);
        if (card > 10)
        {
            card = 10;
        }
        CursorLeft += 8;
        switch (card)
        {
            case 1:
                Aces.Add(Console.CursorLeft);
                AceCard();
                break;
            case 10:
                FaceCard();
                break;
            default:
                NumberCard(card);
                break;
        }
        PlayerTotal += card;
    }
    public static void FaceCard()
    {
        string[] card = Cards.Face[random.Next(0, 4) * 3 + random.Next(0, 3)].Split('~');
        Console.Write(card[0]);
        NewLine();
        string[] color = card[1].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        NewLine();
        Console.Write(card[2]);
        NewLine();
        Console.Write(card[3]);
        NewLine();
        Console.Write(card[4]);
        NewLine();
        color = card[5].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        Console.Write(" ");
        Console.CursorTop = CursorTop;
    }
    public static void AceCard()
    {
        string[] card = Cards.Ace[random.Next(0, 4)].Split('~');
        Console.Write(card[0]);
        NewLine();
        string[] color = card[1].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        NewLine();
        Console.Write(card[2]);
        NewLine();
        Console.Write(card[3]);
        NewLine();
        Console.Write(card[4]);
        NewLine();
        color = card[5].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        Console.Write(" ");
        Console.CursorTop = CursorTop;
    }
    public static void NumberCard(int number)
    {
        int suite = random.Next(0, 4);
        string[] card;
        if (suite == 0)
        {
            card = Cards.Number[number - 2].Split('~');
        }
        else
        {
            card = Cards.Number[number - 2].Replace('^', Cards.Symbol[suite - 1]).Split('~');
        }
        Console.Write(card[0]);
        NewLine();
        string[] color = card[1].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        NewLine();
        Console.Write(card[2]);
        NewLine();
        Console.Write(card[3]);
        NewLine();
        Console.Write(card[4]);
        NewLine();
        color = card[5].Split('=');
        Console.Write(color[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(color[1]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(color[2]);
        Console.Write(" ");
        Console.CursorTop = CursorTop;
    }
    public static void NewLine()
    {
        Console.CursorTop++;
        Console.CursorLeft -= 7;
    }
    public static bool HitOrStay()
    {
        Console.CursorTop = CursorTopText;
        Console.CursorLeft = 0;
        bool choice = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Hit");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" Stay");
        ConsoleKey key = Console.ReadKey(true).Key;
        while (key != ConsoleKey.Enter)
        {
            if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                choice = !choice;
            }
            Console.CursorLeft = 0;
            if (choice)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Hit");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Stay");
            }
            else
            {
                Console.Write("Hit ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Stay");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            key = Console.ReadKey(true).Key;
        }
        Console.CursorLeft = 0;
        Console.Write("        ");
        if (choice) //hit
        {
            Console.SetCursorPosition(CursorLeft, CursorTop);
            GeneratePlayerCard();
            return true;
        }
        else //stay
        {
            if (Aces.Count != 0)
            {
                foreach (int position in Aces)
                {
                    Console.SetCursorPosition(position + 3, CursorTopText - 1);
                    Console.Write('^');
                    Console.SetCursorPosition(0, CursorTopText);
                    bool aceChoice = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("One");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" Eleven");
                    key = Console.ReadKey(true).Key;
                    while (key != ConsoleKey.Enter)
                    {
                        if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                        {
                            aceChoice = !aceChoice;
                        }
                        Console.CursorLeft = 0;
                        if (aceChoice)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("One");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(" Eleven");
                        }
                        else
                        {
                            Console.Write("One ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Eleven");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        key = Console.ReadKey(true).Key;
                    }
                    if (!aceChoice)
                    {
                        PlayerTotal += 10;
                    }
                    Console.SetCursorPosition(position + 3, CursorTopText - 1);
                    Console.Write(' ');
                }
            }
            Console.CursorTop = CursorTopText;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            if (PlayerTotal > AITotal)
            {
                Console.Write("You win!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            if (PlayerTotal == AITotal)
            {
                Console.Write("You tied!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            Console.Write("You lose!");
            Console.ForegroundColor = ConsoleColor.Gray;
            return false;
        }
    }
    public static bool GameNotOver()
    {
        Console.CursorTop = CursorTopText;
        Console.CursorLeft = 0;
        Console.ForegroundColor = ConsoleColor.Red;
        if (PlayerTotal > 21)
        {
            Console.Write("You lose!");
            Console.ForegroundColor = ConsoleColor.Gray;
            return false;
        }
        if (PlayerCardNumber == 5)
        {
            Console.Write("You win!");
            Console.ForegroundColor = ConsoleColor.Gray;
            return false;
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        return true;
    }
    public static void GenerateAICards()
    {
        AICardNumber++;
        int card = random.Next(1, 15);
        if (card > 10)
        {
            card = 10;
        }
        switch (card)
        {
            case 1:
                if (AICardNumber == 1)
                {
                    PlayerTotal += 10;
                }
                AICards.Add(Cards.Ace[random.Next(0, 4)]);
                break;
            case 10:
                AICards.Add(Cards.Face[random.Next(0, 4) * 3 + random.Next(0, 3)]);
                break;
            default:
                int suite = random.Next(0, 4);
                if (suite == 0)
                {
                    AICards.Add(Cards.Number[card - 2]);
                }
                else
                {
                    AICards.Add(Cards.Number[card - 2].Replace('^', Cards.Symbol[suite - 1]));
                }
                break;
        }
        PlayerTotal += card;
    }
}