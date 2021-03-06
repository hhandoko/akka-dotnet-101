﻿namespace AkkaDotNet101.Tutorial09
{
    using System;

    using Akka.Actor;
    using Akka.Routing;

    public class Program
    {
        private static ActorSystem system;

        private static IActorRef spActor;

        private static void Main(string[] args)
        {
            system = ActorSystem.Create("fizz-buzz");
            spActor = system.ActorOf(Props.Create<SupervisorActor>(), "sp-actor");
            spActor.Tell("start");

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

                    var msg2 = string.Format("{0} - {1}", Self.Path, i);
                    var msg3 = string.Empty;

                    if (isFizz && isBuzz)
                    {
                        msg2 = msg2 + " - FizzBuzz";
                        msg3 = "fizzbuzz";
                    }
                    else if (isFizz)
                    {
                        msg2 = msg2 + " - Fizz";
                        msg3 = "fizz";
                    }
                    else if (isBuzz)
                    {
                        msg2 = msg2 + " - Buzz";
                        msg3 = "buzz";
                    }
                    else
                    {
                        msg3 = "number";
                    }

                    Context.ActorSelection("/user/sp-actor/cw-actor").Tell(msg2);
                    Sender.Tell(msg3);
                });
            }
        }

        public class ConsoleWriterActor : ReceiveActor
        {
            private static int counter;

            protected override void PreStart()
            {
                counter = 1;
            }

            public ConsoleWriterActor()
            {
                Receive<string>(msg =>
                {
                    Console.WriteLine("{0}> {1}", counter, msg);
                    counter++;
                });
            }
        }

        public class SupervisorActor : ReceiveActor
        {
            private static IActorRef fbActor;

            private static IActorRef cwActor;

            private static int fizz;

            private static int buzz;

            private static int fizzBuzz;

            private static int total;

            protected override void PreStart()
            {
                fizz = buzz = fizzBuzz = total = 0;

                fbActor = Context.ActorOf(Props.Create<FizzBuzzActor>().WithRouter(new RoundRobinPool(100)), "fb-actor");
                cwActor = Context.ActorOf(Props.Create<ConsoleWriterActor>(), "cw-actor");
            }

            public SupervisorActor()
            {
                Receive<string>(msg =>
                {
                    if (msg == "start")
                    {
                        for (var i = 1; i <= 10000; i++)
                        {
                            fbActor.Tell(new FizzBuzzMessage(i));
                        }
                    }
                    else if (msg == "fizz")
                    {
                        fizz++;
                        total++;
                    }
                    else if (msg == "buzz")
                    {
                        buzz++;
                        total++;
                    }
                    else if (msg == "fizzbuzz")
                    {
                        fizzBuzz++;
                        total++;
                    }
                    else if (msg == "number")
                    {
                        total++;
                    }

                    if (total == 10000)
                    {
                        cwActor.Tell(string.Format("{0} / {1} / {2}", fizz, buzz, fizzBuzz));
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
