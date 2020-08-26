using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Services;

namespace AsyncAwait
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ProcessId: " + Process.GetCurrentProcess().Id);
            Console.WriteLine("Press ENTER to start");
            Console.ReadLine();

            var obj = new Program();
            var task = obj.DoWork(4);
            Console.WriteLine($"Running  task: {task.Id}\t{task.Status}");
            task.Wait();
            Console.WriteLine($"Finished task: {task.Id}\t{task.Status}");
            // var watch = Stopwatch.StartNew();
            // var limit = args.GetInt32OrDefault(0, 10);
            // var worldTimeService = GetService(args);
            // var testCase = new PureAsyncWithCPUboundCase(worldTimeService);
            // testCase.Run(limit);
            // Console.WriteLine($"Finished in {watch.ElapsedMillisåeconds}ms");
            Console.Read();
        }

        private async Task<int> DoWork(int count)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\tNoTask\t\t\t\tDoWork({count})");
            if (count == 1) 
            {
                return 1;
            }
            await new CustomYieldAwaitable();

            var task = DoWork(count - 1);
            //task.ContinueWith(t => Console.WriteLine($"Task {t.Id} finished in {Environment.CurrentManagedThreadId}"));
            Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\t{task.Id}\t{task.Status,-24}DoWork({count}) before await");
            var result = await task + 1;
            Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\t{task.Id}\t{task.Status,-24}DoWork({count}) after await");
            return result;
        }

        private static IWorldTimeService GetService(string[] args)
        {
            if (args.Contains("WithHashing"))
                return new OnlineWorldTimeWithHashService();
            else
                return new OnlineWorldTimeService();
        }
    }

    public class TaskCompletionSourceDeadlock
    {
        public async Task Case1()
        {
            Console.WriteLine("Starting");
            var tcs = new TaskCompletionSource<int>();

            var task = Task.Run(() => 
            {
                Thread.Sleep(100);
                tcs.SetResult(0);
            });

            await tcs.Task;

            task.Wait();
            Console.WriteLine("Finished");
        }
    }

    public class UIContextDeadlock
    {
        public async Task Case()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            Console.WriteLine("Starting");
            DoAsync().Wait();
            Console.WriteLine("Finished");
        }

        private async Task DoAsync()
        {
            await Task.Delay(100);
        }
    }

    public class ExceptionHiding
    {
        public async Task Case()
        {
            Console.WriteLine("Starting");
            try
            {
                Run(async () => throw new NotImplementedException());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Finished");
        }

        private void Run(Action work)
        {
            work();
        }
    }

    public class ExceptionUnhandled
    {
        public async Task Case()
        {
            Console.WriteLine("Starting");
            try
            {
                Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Finished");
            await Task.Delay(1000);
        }

        private async void Run()
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }
    }

    public class ExceptionUnhandledIncompleteTask
    {
        public void Case()
        {
            Console.WriteLine("Starting");
            try
            {
                Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Finished");
            GC.Collect();
        }

        private Task Run()
        {
            Thread.Sleep(100);
            throw new NotImplementedException();
        }
    }
}