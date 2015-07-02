namespace AkkaDotNet101.Tutorial01
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            for (var i = 1; i <= 10000; i++)
            {
                var isFizz = false;
                var isBuzz = false;

                if (i % 3 == 0) { isFizz = true; }
                if (i % 5 == 0) { isBuzz = true; }

                Console.Write(i);

                if (isFizz && isBuzz)
                {
                    Console.WriteLine(" - FizzBuzz");
                }
                else if (isFizz)
                {
                    Console.WriteLine(" - Fizz");
                }
                else if (isBuzz)
                {
                    Console.WriteLine(" - Buzz");
                }
                else
                {
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
        }
    }
}
