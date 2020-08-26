using System;
using System.Threading.Tasks;

namespace CSharpAsyncInternals
{
    public class CountRecursivelyExample
    {
        public static void Run()
        {
            Console.WriteLine("Counting recursively async: ");
            DoRecursivelyAsync(1_000_000).Wait();
            
            Console.WriteLine("Counting recursively sync: ");
            DoRecursively(1_000_000); //this will cause StackOverflowException

            // Counting recursively async: 
            // Counting recursively sync: 
            // Stack overflow.
            // Repeat 174650 times:
            // --------------------------------
            //    at CSharpAsyncInternals.Program.<Main>g__DoRecursively|0_0(Int32)
            // --------------------------------
            //    at CSharpAsyncInternals.Program.Main(System.String[])

            int DoRecursively(int count)
            {
                if (count < 2) return 1;
                return 1 + DoRecursively(count - 1);
            }

            async Task<int> DoRecursivelyAsync(int count)
            {
                if (count < 2) return 1;
                await Task.Yield();
                var result = 1 + await DoRecursivelyAsync(count - 1);
                await Task.Yield();
                return result;
            }
        }
    }
}