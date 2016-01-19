using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    internal class ExceptionSamples
    {
        internal async Task DoWorkAsync()
        {
            var awaitedTask = Task.Run(() => BrokenCodeAsync());

            try
            {
                await awaitedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsFaulted: {0}", awaitedTask.IsFaulted);
                Console.WriteLine("Type: {0}. Message: {1}\n", ex.GetType().Name, ex.Message);
            }

            var waitedTask = Task.Run(() => BrokenCodeAsync());

            try
            {
                waitedTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsFaulted: {0}", waitedTask.IsFaulted);
                Console.WriteLine("Type: {0}. Message: {1}", ex.GetType().Name, ex.Message);

                if (ex.GetType() == typeof (AggregateException))
                {
                    var asAggregate = ex as AggregateException;
                    if (asAggregate != null)
                    {
                        var firstEx = asAggregate.InnerExceptions.First();
                        Console.WriteLine("Type: {0}. Message: {1}",firstEx.GetType().Name, firstEx.Message);
                    }
                }
            }
        }

        private void BrokenCode()
        {
            throw new NotImplementedException("Hasn't been written correctly.");
        }

        private static async Task BrokenCodeAsync()
        {
            throw new NotImplementedException("Hasn't been written correctly.");

            await Task.Run(() => "");
        }
    }
}
