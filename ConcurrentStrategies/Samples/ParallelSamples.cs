using System.Linq;
using System.Threading.Tasks;

namespace ConcurrentStrategies.Samples
{
    internal class ParallelSamples
    {
        internal async Task DoWorkAsync()
        {
            var numbers = Enumerable.Range(1, 10);

            //numbers.AsParallel().
        }
    }
}
