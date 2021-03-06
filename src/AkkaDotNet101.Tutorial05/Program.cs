﻿namespace AkkaDotNet101.Tutorial05
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
                actor.Tell(new FizzBuzzMessage(i));
            }

            Console.ReadKey();
        }

        public class FizzBuzzActor : ReceiveActor
        {
            public FizzBuzzActor()
            {
                Receive<FizzBuzzMessage>(msg =>
                {
                    var i = msg.Number;

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
                });
            }
        }

        public class FizzBuzzMessage
        {
            public FizzBuzzMessage(int number)
            {
                Number = number;
            }

            public readonly int Number;
        }
    }
}
