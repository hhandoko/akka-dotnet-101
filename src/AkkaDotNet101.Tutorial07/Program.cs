namespace AkkaDotNet101.Tutorial07
{
    using System;

    using Akka.Actor;

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

                    var msg2 = i.ToString();

                    if (isFizz && isBuzz)
                    {
                        msg2 = msg2 + " - FizzBuzz";
                    }
                    else if (isFizz)
                    {
                        msg2 = msg2 + " - Fizz";
                    }
                    else if (isBuzz)
                    {
                        msg2 = msg2 + " - Buzz";
                    }

                    Context.ActorSelection("/user/sp-actor/cw-actor").Tell(msg2);
                });
            }
        }

        public class ConsoleWriterActor : ReceiveActor
        {
            public ConsoleWriterActor()
            {
                Receive<string>(msg => Console.WriteLine(msg));
            }
        }

        public class SupervisorActor : ReceiveActor
        {
            private static IActorRef fbActor;

            private static IActorRef cwActor;

            protected override void PreStart()
            {
                fbActor = Context.ActorOf(Props.Create<FizzBuzzActor>(), "fb-actor");
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
