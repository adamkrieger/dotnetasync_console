using System;
using System.Threading;
using ConcurrentStrategies.Things;

namespace ConcurrentStrategies.Samples
{
    public class OldThreadSamples
    {
        public void DoWorkAsync()
        {
            Thing result = null;
            var getThread = new Thread(() => { result = GetValue(); });

            Console.WriteLine("I have the Thread.");

            getThread.Start();
            Thread.Sleep(1);

            Console.WriteLine("And I can do other things here while I'm waiting.");

            Console.WriteLine("But now I have to wait for the Thread in a complicated way.");

            while (getThread.IsAlive)
            {
                Console.WriteLine("...");
                Thread.SpinWait(50000000);
                if (!getThread.IsAlive)
                {
                    break;
                }
                Console.WriteLine("......");
                Thread.Sleep(100);
            }

            Console.WriteLine("Thread has completed.");

            Console.WriteLine(" Ref Value: {0}", result.Ref);
        }

        private Thing GetValue()
        {
            Console.WriteLine("Sleep for 500");
            Thread.Sleep(500);
            Console.WriteLine("Slept for 500");
            return new Thing { Ref = "Init" };
        }
    }
}
