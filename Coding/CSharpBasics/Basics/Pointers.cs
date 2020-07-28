using System;

namespace Basics
{
    public class Pointers
    {
        class PocoTest1
        {
            public int i {get;}
            public DateTime date {get;}
            public byte a {get;}
            public string str {get;}
        }

        public unsafe void Run()
        {
            int i = 1;
            int j = 4;
            long a = 8;
            int* ptr = &i;
            IntPtr addr = (IntPtr)ptr;

            long* ptr2 = &a;
            IntPtr addr2 = (IntPtr)ptr2;

            var poco = new PocoTest1();

            TypedReference typedReference = __makeref(poco);
            int* poco1Address = (int*)((*(int*)(&typedReference)) - 4);

            Console.WriteLine(addr.ToString("x"));
            Console.WriteLine(addr2.ToString("x"));
            Console.WriteLine(addr.ToInt64() - addr2.ToInt64());
            Console.WriteLine(((IntPtr)(poco1Address)).ToString("x"));
        }
    }
}
