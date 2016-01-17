using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentStrategies.Samples;

namespace ConcurrentStrategies
{
    class Program
    {
        public static bool UserHasQuit = false;

        static void Main()
        {
            Welcome();
            var menu = Menu();

            var chosenSample = GetUserChoice(menu);

            var stopwatch = new Stopwatch();

            Console.WriteLine("=== Sample Start ===\n\n");

            stopwatch.Start();
            menu[chosenSample].ActionAsync().Wait();
            stopwatch.Stop();

            Console.WriteLine("\n\n=== Sample End ({0}) ===", stopwatch.Elapsed);
            Console.ReadKey();
        }

        private static void Welcome()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("   Welcome! You have {0} logical processors.", Environment.ProcessorCount);
            int workerThreads;
            int ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            Console.WriteLine("   Workers: " + workerThreads + " IO: " + ioThreads);
            Console.WriteLine("==================================================");
        }

        private static string GetUserChoice(Dictionary<string, MenuOption> menu)
        {
            string choice = null;

            while (choice == null)
            {
                foreach (var menuOption in menu)
                {
                    Console.WriteLine("{0}: {1}", menuOption.Key, menuOption.Value.Description);
                }
                var userChoice = Console.ReadLine();
                if (menu.Keys.Any(key => key == userChoice))
                {
                    choice = userChoice;
                }
            }

            return choice;
        }

        private static Dictionary<string, MenuOption> Menu()
        {
            var awaitSamples = new AwaitSamples();
            var continueSamples = new ContinueSamples();
            var cancelSamples = new CancelSamples();

            return new Dictionary<string, MenuOption>
            {
                {"1", MenuOption.New("async/await", awaitSamples.DoWorkAsync)},
                {"2", MenuOption.New("ContinueWith", continueSamples.DoWorkAsync)},
                {"3", MenuOption.New("CancellationToken", cancelSamples.DoWorkAsync)}
            };
        }

        private class MenuOption
        {
            public static MenuOption New(string description, Func<Task> action)
            {
                return new MenuOption()
                {
                    Description = description,
                    ActionAsync = action
                };
            }

            public string Description { get; private set; }
            public Func<Task> ActionAsync { get; private set; }
        }
    }
}
