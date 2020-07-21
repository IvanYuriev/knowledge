using System;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            //2 Complement experiments:
            var binaryOps = new BinaryOperations();
            binaryOps.SubtractionIsReplacedWithAddition();
            binaryOps.PrintAllNumbers(-6, 6);
        }

        
    }
}
