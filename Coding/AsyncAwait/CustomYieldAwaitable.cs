using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.YieldAwaitable;

namespace AsyncAwait
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct CustomYieldAwaitable
    {
        [StructLayout(LayoutKind.Sequential, Size = 2)]
        public readonly struct CustomYieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            private static readonly YieldAwaiter _awaiter = new YieldAwaiter();

            public bool IsCompleted => false;

            public void GetResult(){ Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\tNoTask\t\t\t\tYieldAwaiter GetResult"); }

            public void OnCompleted(Action continuation)
            {
                _awaiter.OnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\tNoTask\t\t\t\tYieldAwaiter before OnCompleted");
                Thread.Sleep(100);
                _awaiter.UnsafeOnCompleted(continuation);
                //Task.Factory.StartNew(continuation, TaskCreationOptions.RunContinuationsAsynchronously);
                Console.WriteLine($"{DateTime.Now.Ticks}\t{Environment.CurrentManagedThreadId}\tNoTask\t\t\t\tYieldAwaiter after OnCompleted");
            }
        }

        public CustomYieldAwaiter GetAwaiter()
        {
            return new CustomYieldAwaiter();
        }
    }
}