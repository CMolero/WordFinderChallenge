namespace WordFinder.Demo
{
    public static class Program
    {
        public static void Main()
        {
            RunDemo();
            Console.WriteLine();
        }
        private static void RunDemo()
        {
            Console.WriteLine("==== Demo ====");
            var matrix = new[]
            {
                "chill",
                "oxxxx",
                "lxxxx",
                "dxxxx",
                "wind!",
            };

            var finder = new WordFinder(matrix);
            var stream = new[]
            {
                "chill",
                "wind",
                "cold",
                "hot",
                "ox",
            };

            var results = finder.Find(stream).ToList();

            Console.WriteLine($"stream: [{string.Join(", ", stream)}]");
            Console.WriteLine($"Found words: [{string.Join(", ", results)}]");
        }
    }
}