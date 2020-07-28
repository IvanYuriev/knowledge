using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Basics
{

    public class MemoryBasics
    {
        private const int Count = 1000000;

        struct MyValueType
        {
            public int counter;
            public long ticks;
        }

        struct MyRefType
        {
            public int i;
            public long j;
        }

        public static void Run()
        {
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
            Console.WriteLine("Press smth to run");
            Console.ReadLine();

            RunWithBenchmark($"WarmUp", () => {}, 1000);

            int stackAllocationCount = 1024 * 1024 / (4 + 8);
            stackAllocationCount += 87243; //it is the max number possible to insert in stack
            RunWithBenchmark($"Stack allocation {stackAllocationCount}", () => 
            {
                RecursiveAction(new MyValueType{ counter = stackAllocationCount});
            });

            RunWithBenchmark($"Array of {Count} ValueTypes", () =>
            {
                var valueArray = new MyValueType[Count];
                for (int i = 0; i < Count; i++)
                {
                    var value = new MyValueType { counter = i, ticks = DateTime.Now.Ticks };
                    valueArray[i] = value;
                }
            });

            RunWithBenchmark($"Array of {Count} ReferenceTypes", () =>
            {
                var refArray = new MyRefType[Count];
                for (int i = 0; i < Count; i++)
                {
                    var value = new MyRefType { i = i, j = DateTime.Now.Ticks };
                    refArray[i] = value;
                }
            });

            RunWithBenchmark($"List of {Count} ReferenceTypes", () =>
            {
                var refList = new List<MyRefType>(Count); //w/o Count it allocates twice more (array grow)
                for (int i = 0; i < Count; i++)
                {
                    var value = new MyRefType { i = i, j = DateTime.Now.Ticks };
                    refList.Add(value);
                }
            });

            // ===============================================
            // THREADS
            // ===============================================
            var threadsCount = 10000;
            var threads = new Thread[threadsCount];
            RunWithBenchmark($"Creating threads ({threadsCount})", () =>
            {
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i] = new Thread(CpuIntensiveJob);
                }
            });
            RunWithBenchmark("Running threads", () =>
            {
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i].Start(i);
                }
            });

            // ===============================================
            // TASKS
            // ===============================================
            var tasksCount = 10000;
            var tasks = new Task<double>[tasksCount];
            RunWithBenchmark($"Creating tasks ({tasksCount})", () =>
            {
                for (int i = 0; i < tasksCount; i++)
                {
                    tasks[i] = new Task<double>(() => CpuIntensiveJob(i));
                }
            });
            RunWithBenchmark("Running tasks:", () =>
            {
                for (int i = 0; i < tasksCount; i++)
                {
                    tasks[i].Start();
                }
                var taskResults = Task.WhenAll(tasks).GetAwaiter().GetResult();
            });

            Console.WriteLine("Press smth to finish");
            Console.ReadLine(); // press key to start
        }

        static double CpuIntensiveJob(int seed)
        {
            double sin = 0;
            double tan = 0;

            for (int i = 0; i < 1000; i++)
            {
                sin = Math.Sin(i);
                tan = Math.Tan(i ^ 2);
            }
            return seed + 1000 + sin + tan;
        }

        static void CpuIntensiveJob(object seed)
        {
            CpuIntensiveJob((int) seed);
        }

        static void RecursiveAction(MyValueType i)
        {
            //Console.WriteLine(i.counter);
            if (i.counter <= 0) return;
            i.counter--;
            RecursiveAction(i);
        }


        static void RunWithBenchmark(string message, Action action, int idleTimeoutMs = 5000)
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine();
            Console.Write(message);
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured:");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                watch.Stop();
                Console.WriteLine($"... Done in {watch.ElapsedMilliseconds}ms");
                Console.WriteLine("\tWorking Set MB:" + Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
                Console.WriteLine("\tGC Thread Allocated KB:" + GC.GetAllocatedBytesForCurrentThread() / 1024);
                Console.WriteLine("\tGC GetTotalAllocated MB:" + GC.GetTotalAllocatedBytes(true) / 1024 / 1024);
            }
            Thread.Sleep(idleTimeoutMs);
        }
    }

}