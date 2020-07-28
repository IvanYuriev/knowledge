using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpAsync
{
    partial class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
            Console.WriteLine("Press smth to run");
            Console.ReadLine();
            
            

            Console.WriteLine("Press smth to finish");
            Console.ReadLine(); // press key to start
        }


        static async Task<int> CountRecursivelyAsync(int count)
        {
            await Task.Yield();
            if (count <= 0)
                return count;
            var result = 1 + await CountRecursivelyAsync(count - 1);
            await Task.Yield();
            return result;
        }
    }
}