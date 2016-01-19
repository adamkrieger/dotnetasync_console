using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    internal class WhenAllSamples
    {
        internal async Task DoWorkAsync()
        {
            await Task.Run(async () =>
            {
                var numbers = Enumerable.Range(1, 10).ToArray();

                var nonParallel = TimeIt("nonParallel", () => numbers.Select(WaitAndReturn).ToArray());

                var parallel = await TimeIt("parallel", async () =>
                {
                    var tasks = numbers
                        .Select(each => Task
                            .Run(() => WaitAndReturn(each))
                            .ContinueWith(prev => PrintAndContinue(prev))
                        )
                        .ToArray();

                    return await Task.WhenAll(tasks);
                });

                foreach (var i in nonParallel)
                {
                    Console.WriteLine("NonP: {0}", i);
                }

                foreach (var i in parallel)
                {
                    Console.WriteLine("P: {0}", i);
                }
            });
        }

        private static int WaitAndReturn(int input)
        {
            Thread.SpinWait(200000000);
            return input * 10;
        }

        private static int PrintAndContinue(Task<int> prev)
        {
            var result = prev.Result;
            Console.WriteLine("Done {0}", result);
            return result;
        }

        private static T TimeIt<T>(string description, Func<T> func)
        {
            var stopwatch = new Stopwatch();

            Console.WriteLine("Starting: {0}", description);

            stopwatch.Start();
            var result = func();
            stopwatch.Stop();

            Console.WriteLine("Finished: {0}/nDuration: {1}", description, stopwatch.Elapsed);

            return result;
        }
    }
}
