using System;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Binary operations;\r\n2. Memory Tests");
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
            }

        }

    }
}