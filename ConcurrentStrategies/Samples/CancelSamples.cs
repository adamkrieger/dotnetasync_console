using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    public class CancelSamples
    {
        public async Task DoWorkAsync()
        {
            var cancelTokenSource = new CancellationTokenSource();
            var cancelToken = cancelTokenSource.Token;

            var cancellableWork = Task.Run(() => CancelThis(cancelToken), cancelToken);

            Thread.Sleep(3000);
            cancelTokenSource.Cancel();
            
            var statement = await cancellableWork;

            Console.WriteLine(statement);
        }

        public string CancelThis(CancellationToken token)
        {
            var seconds = Enumerable.Range(1, 10).ToArray();

            foreach (var second in seconds)
            {
                Thread.Sleep(1000);

                if (token.IsCancellationRequested)
                {
                    return string.Format("I waited for {0} seconds, before being cancelled.", second);
                }
            }

            return string.Format("I waited for {0} seconds.", seconds.Last());
        }
    }
}
