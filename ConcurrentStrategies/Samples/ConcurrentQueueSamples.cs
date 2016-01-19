using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    class ConcurrentQueueSamples
    {
        private static readonly ConcurrentQueue<int> ConcurrentQueue = new ConcurrentQueue<int>();
        private static int _concurrentSequence = 1;

        public async Task DoWorkAsync()
        {
            await Task.Run(() =>
            {
                var cancelSource = new CancellationTokenSource();
                var cancelToken = cancelSource.Token;

                Task.Run(() => Publisher(cancelToken), cancelToken);
                Task.Run(() => Consumer(cancelToken), cancelToken);
                Task.Run(() => Consumer(cancelToken), cancelToken);

                cancelSource.CancelAfter(1000);
            });

            Console.ReadKey();
            Console.WriteLine("Cancellation Complete");
        }

        private static void Publisher(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var next = _concurrentSequence++;
                ConcurrentQueue.Enqueue(next);
                Console.WriteLine("Published {0}", next);
                Thread.Sleep(40);
            }
        }

        private static void Consumer(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (ConcurrentQueue.Count > 0)
                {
                    int item;
                    if (ConcurrentQueue.TryDequeue(out item))
                    {
                        Console.WriteLine("Consumed {0}", item);
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
