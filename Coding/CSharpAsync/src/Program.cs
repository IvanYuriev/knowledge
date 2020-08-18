using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Tpl;
using Microsoft.Diagnostics.Tracing.Session;

namespace CSharpAsync
{
    partial class Program
    {
        struct MyValue
        {
            public int Val { get; }
        }

        struct MyValue2
        {
            public int Val { get; }
            public override int GetHashCode()
            {
                return Val.GetHashCode();
            }

            public override string ToString()
            {
                return Val.ToString();
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
            var a = Console.ReadKey();
            //var tcs = new System.Threading.CancellationTokenSource();
            // using (var session = new TraceEventSession("TplCaptureSession"))
            // {
            //     var t = Task.Run(() =>
            //     {
            //         session.EnableProvider(TplEtwProviderTraceEventParser.ProviderGuid, TraceEventLevel.Informational);

            //         var parser = new TplEtwProviderTraceEventParser(session.Source);
            //         parser.AddCallbackForEvent<Microsoft.Diagnostics.Tracing.Parsers.Tpl.AwaitTaskContinuationScheduledArgs>(
            //          null,
            //          @event =>
            //          {
            //              Console.WriteLine($"Incomplete async: Task {@event.Task} started from {@event.ContinuationId}: {@event.OpcodeName}");
            //          });

            //         parser.AddCallbackForEvent<Microsoft.Diagnostics.Tracing.Parsers.Tpl.TraceSynchronousWorkStopArgs>(
            //          null,
            //          @event =>
            //          {
            //              Console.WriteLine($"Sync: Task {@event.Task} started from {@event.ID}: {@event.OpcodeName}");
            //          });

                    //parser.AddCallbackForEvent<Microsoft.Diagnostics.Tracing.Parsers.Tpl.TraceOperationStartArgs>(
                    // null,
                    // @event =>
                    // {
                    //     Console.WriteLine($"OpStart {@event.Task} started from {@event.ID}: {@event.OpcodeName}");
                    // });
                    //parser.AddCallbackForEvent<Microsoft.Diagnostics.Tracing.Parsers.Tpl.TaskWaitContinuationCompleteArgs>(
                    // null,
                    // @event =>
                    // {
                    //     Console.WriteLine($"Task {@event.Task} continuation complete {@event.FormattedMessage} ");
                    // });
                    //parser.AddCallbackForEvent<TaskCompletedArgs>(
                    //     null,
                    //     @event =>
                    //     {
                    //         Console.WriteLine($"Task {@event.TaskID} completed");
                    //     });
                    //parser.AddCallbackForProviderEvents((eventName, smth) => EventFilterResponse.AcceptEvent,
                    //    null,
                    //    @event =>
                    //    {
                    //        Console.WriteLine($"{@event.EventName}: {@event.FormattedMessage}");
                    //    });

                //     session.Source.Process();

                // });
                //Thread.Sleep(3000);
                CountRecursivelyAsync(30000).Wait();
            //}
            //Async1.Run(3);
            //var taskExercises = new TasksExercises();
            //taskExercises.TaskContinuation();
            //taskExercises.TaskChainOfContinuation();
            //taskExercises.TaskChainOfContinuationRecursively(100);
            //taskExercises.TaskChainOfContinuationRecursivelyReverse(100);
            //Async1.Run(2);
            //source?.StopProcessing();
            //tcs.Cancel();
            //Task.WhenAll(task1, task2).Wait();
            //a = Console.ReadKey();
            Console.WriteLine("Press smth to finish");
            Console.ReadLine(); // press key to start
        }

        static async Task<int> CountRecursivelyAsync(int count)
        {
            await Task.Yield();
            if (count <= 0) return count;
            var result = 1 + await CountRecursivelyAsync(count - 1);
            await Task.Yield();
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            return result;
        }
    }
}