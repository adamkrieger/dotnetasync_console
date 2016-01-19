using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    class UnsafeQueueSamples
    {
        private static readonly Queue<int> Queue = new Queue<int>();
        private static int _sequence = 1;

        public async Task DoWorkAsync()
        {
            await Task.Run(() =>
            {
                var cancelSource = new CancellationTokenSource();
                var cancelToken = cancelSource.Token;

                var publisher = Task.Run(() => Publisher(cancelToken), cancelToken);
                var consumer = Task.Run(() => Consumer(cancelToken), cancelToken);
                var consumer2 = Task.Run(() => Consumer(cancelToken), cancelToken);

                cancelSource.CancelAfter(1000);
            });

            Console.ReadKey();
            Console.WriteLine("Cancellation Complete");
        }

        private static void Publisher(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var next = _sequence++;
                Queue.Enqueue(next);
                Console.WriteLine("Published {0}", next);
                Thread.Sleep(100);
            }
        }

        private static void Consumer(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (Queue.Count > 0)
                {
                    var item = Queue.Dequeue();
                    Console.WriteLine("Consumed {0}", item);
                }
                Thread.Sleep(50);
            }
        }
    }
}
