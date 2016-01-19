using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    class MutexSamples
    {
        private static readonly ConcurrentQueue<int> ConcurrentQueue = new ConcurrentQueue<int>();
        private static int _concurrentSequence = 1;
        private static Mutex _sequenceMutex = new Mutex(false);

        public async Task DoWorkAsync()
        {
            await Task.Run(() =>
            {
                var cancelSource = new CancellationTokenSource();
                var cancelToken = cancelSource.Token;

                var publisher = Task.Run(() => PublisherWithMutex(cancelToken), cancelToken);
                var publisher2 = Task.Run(() => PublisherWithMutex(cancelToken), cancelToken);
                var consumer = Task.Run(() => Consumer(cancelToken), cancelToken);

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
                Thread.Sleep(100);
            }
        }

        private static void PublisherWithMutex(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var owned = _sequenceMutex.WaitOne(50);

                if (owned)
                {
                    var next = _concurrentSequence++;
                    ConcurrentQueue.Enqueue(next);
                    Console.WriteLine("Published {0}", next);
                    Thread.Sleep(100);
                    _sequenceMutex.ReleaseMutex();
                }
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
