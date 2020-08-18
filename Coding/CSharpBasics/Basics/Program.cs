using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Basics
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("1. Binary operations;\r\n2. Memory Tests");
            GCNotification.GCDone += (x) =>
            {
                Console.WriteLine("Gen " + x + "; " + Thread.CurrentThread.ManagedThreadId);
            };

            var tasks = Enumerable.RangeAsync(0, 10)
                .Select(x => Task.Run(() => 
                {
                    Console.WriteLine($"Running task {x} on {Thread.CurrentThread.ManagedThreadId}");
                    var arrayList = new List<int>();
                    foreach (var x in Enumerable.Range(0, 1_000_000))
                    {
                        arrayList.Add(x);
                    }
                    Thread.Sleep(2000);
                }));
            await Task.WhenAll(tasks);
            return;
            switch (Console.ReadLine())
            {
                case "1":
                    //2 Complement experiments:
                    var binaryOps = new BinaryOperations();
                    binaryOps.SubtractionIsReplacedWithAddition();
                    binaryOps.PrintAllNumbers(-6, 6);
                    break;
                case "2":
                    MemoryBasics.Run();
                    break;
                case "3":
                    AllocationsAndGC.RunGenerations();
                    break;
            }

        }

    }
}