﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    internal class ParallelSamples
    {
        internal async Task DoWorkAsync()
        {
            await Task.Run(() =>
            {
                var numbers = Enumerable.Range(1, 10).ToArray();

                var nonParallel = TimeIt("nonParallel", () => numbers.Select(WaitAndReturn).ToArray());

                var parallel = TimeIt("parallel", () => numbers.AsParallel().Select(WaitAndReturn).ToArray());

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

        private int WaitAndReturn(int input)
        {
            Thread.SpinWait(200000000);
            return input * 10;
        }

        private T TimeIt<T>(string description, Func<T> func)
        {
            var stopwatch =new Stopwatch();

            Console.WriteLine("Starting: {0}", description);
            
            stopwatch.Start();
            var result = func();
            stopwatch.Stop();

            Console.WriteLine("Finished: {0}/nDuration: {1}", description, stopwatch.Elapsed);

            return result;
        }
    }
}
