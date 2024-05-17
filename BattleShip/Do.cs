using System.Net;

namespace Battleship
{
    public enum PeopleInPickle
    {
        Will = 0,
        Noah = 1,
        Alex = 2
    }

    class Do
    {
        public static void Something()
        {
            3892232.Testing();
        }
    }

    public static class extensionmethods
    {
        public static void Testing(this int i)
        {
            Console.WriteLine(i);
            string test = Console.ReadLine() ?? "";
            int result = Convert.ToInt32(test);
        }
    }
}