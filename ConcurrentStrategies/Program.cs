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
            var whenAllSamples = new WhenAllSamples();
            var parallelSamples = new ParallelSamples();
            var unsafeQueueSamples = new UnsafeQueueSamples();
            var concurrentQueueSamples = new ConcurrentQueueSamples();
            var mutexSamples = new MutexSamples();
            var exceptionSamples = new ExceptionSamples();

            return new Dictionary<string, MenuOption>
            {
                {"1", MenuOption.New("async/await", awaitSamples.DoWorkAsync)},
                {"2", MenuOption.New("ContinueWith", continueSamples.DoWorkAsync)},
                {"3", MenuOption.New("CancellationToken", cancelSamples.DoWorkAsync)},
                {"4", MenuOption.New("WhenAll", whenAllSamples.DoWorkAsync)},
                {"5", MenuOption.New("AsParallel", parallelSamples.DoWorkAsync)},
                {"6", MenuOption.New("Unsafe Queue<>", unsafeQueueSamples.DoWorkAsync)},
                {"7", MenuOption.New("ConcurrentQueue<>", concurrentQueueSamples.DoWorkAsync)},
                {"8", MenuOption.New("Mutex", mutexSamples.DoWorkAsync)},
                {"9", MenuOption.New("Ex Handling", exceptionSamples.DoWorkAsync)}
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
