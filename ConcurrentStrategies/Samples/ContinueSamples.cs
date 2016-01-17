using System;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentStrategies.Things;

namespace ConcurrentStrategies.Samples
{
    public class ContinueSamples
    {
        public async Task DoWorkAsync()
        {
            Task<Thing> somethingValuable = GetValueAsync();

            Console.WriteLine("I have the Task<Thing>");

            var result = await somethingValuable;

            Console.WriteLine("I've got the result.");

            Console.WriteLine(" Ref Value: {0}", result.Ref);
        }

        private async Task<Thing> GetValueAsync()
        {
            Func<Thing> anonFunc = () =>
            {
                Console.WriteLine("Sleep for 500");
                Thread.Sleep(500);
                Console.WriteLine("Slept for 500");
                return new Thing { Ref = "Init" };
            };

            var result = 
                Task
                    .Run(anonFunc)
                    .ContinueWith((prev) =>
                    {
                        var prevResult = prev.Result;

                        return new Thing {Ref = prevResult.Ref + ", Plus Something Else"};
                    }
                    )
                    .ContinueWith(prev =>
                    {
                        var prevResult = prev.Result;

                        return new Thing { Ref = prevResult.Ref + ", But Also This" };
                    });


            return await result;
        }
    }
}
