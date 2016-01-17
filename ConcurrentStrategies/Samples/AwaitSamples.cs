using System;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentStrategies.Things;

namespace ConcurrentStrategies.Samples
{
    public class AwaitSamples
    {
        //Things of Note
        // "Sleep for 500" is sometimes printed before "I have the Task<Thing>" and sometimes not.
        // Thread.Sleep is used to made it obvious that DoWorkAsync continues while the Task is running.

        public async Task DoWorkAsync()
        {
            Task<Thing> somethingValuable = GetValueAsync();
            //Task<Thing> somethingValuable = GetValueDependentOnInputAsync(1);

            Console.WriteLine("I have the Task<Thing>");

            var result = await somethingValuable;

            Console.WriteLine("I've got the result.");

            Console.WriteLine(" Ref Value: {0}", result.Ref);
        }

        private async Task<Thing> GetValueAsync()
        {
            var result = await Task.Run(() =>
            {
                Console.WriteLine("Sleep for 500");
                Thread.Sleep(500);
                Console.WriteLine("Slept for 500");
                return new Thing { Ref = "Init" };
            });

            //Can do validation, logging, and other work that doesn't require the main thread before returning.
            //Anything done here will also delay a call to .Result

            result.Ref = "Updated";

            return result;
        }

        private async Task<Thing> GetValueDependentOnInputAsync(string index)
        {
            Func<Thing> anonFunc = () =>
            {
                Console.WriteLine("Sleep for 500");
                Thread.Sleep(500);
                Console.WriteLine("Slept for 500");
                return new Thing { Ref = index };
            };

            var result = await Task.Run(anonFunc);

            //Not used, won't affect the ref returned in 'result' because of closures
            index = "Completely unrelated value.";

            return result;
        }
    }
}
