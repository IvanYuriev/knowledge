using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpAsync
{
    public class TasksExercises
    {
        public void DelegatesActionsAndTasks()
        {

        }

        public void TaskContinuation()
        {
            int totalTasksSucceeded = 0;
            int threadWasChangedAmount = 0;
            int threadWasChangedAmountUnsafe = 0;
            var watch = Stopwatch.StartNew();
            var rnd = new Random();
            var count = 8_000_000;
            var tasks = new Task<TaskInfo>[count];
            Console.WriteLine($"Running {count} tasks...");
            for (int i = 0; i < count; i++)
            {
                tasks[i] = new Task<TaskInfo>(() =>
                {
                    MakeSomeCpuLoad(rnd, i);
                    return new TaskInfo(i, Thread.CurrentThread.ManagedThreadId);
                });
                tasks[i].ContinueWith((t) =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        Interlocked.Increment(ref totalTasksSucceeded);
                    }
                    var taskInfo = t.Result;
                    if (taskInfo.ThreadId != Thread.CurrentThread.ManagedThreadId)
                    {
                        threadWasChangedAmountUnsafe++;
                        Interlocked.Increment(ref threadWasChangedAmount);
                        //Console.WriteLine($"Task {taskInfo.Id} doesn't match thread {taskInfo.ThreadId} != {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            };
            Parallel.ForEach(tasks, task => task.Start());

            Console.WriteLine($"Waiting for {count} tasks to finish...");
            Task.WhenAll(tasks);
            watch.Stop();
            Console.WriteLine($"Finished in {watch.ElapsedMilliseconds}ms with {totalTasksSucceeded} successful tasks");
            Console.WriteLine($"Total amount of threads switching: {threadWasChangedAmount}, unsafe: {threadWasChangedAmountUnsafe}");
        }

        public void TaskChainOfContinuation()
        {
            var rootTask = new Task(() => { Console.WriteLine($"Root task is running on {Thread.CurrentThread.ManagedThreadId}"); });
            var task = rootTask;
            for(int i = 0; i < 100; i++)
            {
                task = task.ContinueWith((t, state) => 
                { 
                    Console.WriteLine($"Continuation {state} is running on {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(20);
                }, i);
            }
            rootTask.Start();
            rootTask.Wait();
            Console.WriteLine("Finished");
        }

        public void TaskChainOfContinuationRecursively(int count, Task task = null)
        {
            if (task == null)
            {
                var rootTask = new Task(() => { Console.WriteLine($"Root task is running on {Thread.CurrentThread.ManagedThreadId}"); });
                TaskChainOfContinuationRecursively(count, rootTask);
                rootTask.Start();
                rootTask.Wait();
                return;
            }
            if (count > 0) 
            {
                TaskChainOfContinuationRecursively(count - 1, task.ContinueWith((t, state) => 
                { 
                    Console.WriteLine($"Continuation {state} is running on {Thread.CurrentThread.ManagedThreadId}");
                    //Thread.Sleep(20);
                }, count));
                return;
            }
        }

        public void TaskChainOfContinuationRecursivelyReverse(int count)
        {
            TaskChainOfContinuationRecursivelyReverse(count, out var rootTask);
            rootTask.Start();
            rootTask.Wait();
        }

        private Task TaskChainOfContinuationRecursivelyReverse(int count, out Task initialTask)
        {
            var continuation = new Action<object>(state => 
                { 
                    Console.WriteLine($"Continuation {state} is running on {Thread.CurrentThread.ManagedThreadId}"); 
                });

            if (count > 0) 
            {
                var task = TaskChainOfContinuationRecursivelyReverse(count - 1, out initialTask);
                return task.ContinueWith((t, state) => continuation(state), count);
            }
            else
            {
                initialTask = new Task(() => { Console.WriteLine($"Root task is running on {Thread.CurrentThread.ManagedThreadId}"); });
                return initialTask.ContinueWith((t, state) => continuation(state), count);
            }
        }

        private static void MakeSomeCpuLoad(Random rnd, int i)
        {
            var d = double.MinValue;
            for (int j = 0; j < rnd.Next(10000); j++)
                d = Math.Pow(Math.Atan2(j, i), Math.Exp(j * i));
        }

        private struct TaskInfo
        {
            public TaskInfo(int id, int threadId)
            {
                Id = id;
                ThreadId = threadId;
            }
            public readonly long Id;
            public readonly int ThreadId;
        }
    }
}