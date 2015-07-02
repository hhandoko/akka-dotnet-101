namespace AkkaDotNet101.Tutorial02
{
    using System;

    using Akka.Actor;

    public class Program
    {
        private static void Main(string[] args)
        {
            var system = ActorSystem.Create("fizz-buzz");
            var actor = system.ActorOf(Props.Create<FizzBuzzActor>(), "fb-actor");

            for (var i = 1; i <= 10000; i++)
            {
                actor.Tell(i);
            }

            Console.ReadKey();
        }

        public class FizzBuzzActor : UntypedActor
        {
            protected override void OnReceive(object message)
            {
                if (message is int)
                {
                    var i = (int)message;

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
            }
        }
    }
}
