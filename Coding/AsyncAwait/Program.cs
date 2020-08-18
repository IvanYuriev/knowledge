using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace AsyncAwait
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ProcessId: " + Process.GetCurrentProcess().Id);
            Console.WriteLine("Press ENTER to start");
            Console.ReadLine();

            var limit = args.Length > 0 ? Int32.Parse(args[0]) : Int32.MaxValue;
            
            new PureAsyncWithCPUboundTask().Run(limit);

        }
    }
}