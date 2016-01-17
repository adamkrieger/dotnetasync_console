using System;
using System.Threading;
using ConcurrentStrategies.Samples;

namespace ConcurrentStrategies
{
    class Program
    {
        public static bool UserHasQuit = false;

        static void Main()
        {
            Welcome();

            Console.WriteLine("=== Sample Start ===\n\n");

            var sample = new OldThreadSamples();
            sample
                .DoWorkAsync();
                //.Wait();

            Console.WriteLine("\n\n=== Sample End ===");
            Console.ReadKey();
        }

        private static void Welcome()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(string.Format("   Welcome! You have {0} logical processors.", Environment.ProcessorCount));
            int workerThreads;
            int ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            Console.WriteLine("   Workers: " + workerThreads + " IO: " + ioThreads);
            Console.WriteLine("==================================================");
        }
    }
}
