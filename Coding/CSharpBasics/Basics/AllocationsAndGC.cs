using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Basics
{
    internal sealed class AllocationsAndGC
    {
        private static long memoryDiff = 0;
        public static void RunGenerations()
        {
            memoryDiff = GC.GetTotalMemory(true);

            var mocks = new List<Mock>(100);
            mocks.AddRange(Enumerable.Range(0, 10).Select(x => new Mock((char) ((byte)'A' + x))));

            HeapStatistics.CollectAndReport(mocks, () =>
            {
                mocks.RemoveAt(1);
                mocks.RemoveAt(5);
                mocks.RemoveAt(6);
            });

            mocks.Add(new Mock('X'));
            mocks.Add(new Mock('Y'));
            mocks.Add(new Mock('Z'));

            HeapStatistics.CollectAndReport(mocks, () =>
            {
                mocks.RemoveAt(2);
                mocks.RemoveAt(4);
                mocks.RemoveAt(1);
            });

            mocks.Add(new Mock('W'));

            HeapStatistics.CollectAndReport(mocks, () =>
            {
                mocks.RemoveAt(0);
            });
        }

        internal class HeapStatistics
        {
            public static void CollectAndReport(IList<Mock> objs, Action cleanUpCallback)
            {
                Console.WriteLine(new String('=', 20));
                Console.WriteLine("Gen0: " + GC.CollectionCount(0));
                Console.WriteLine("Gen1: " + GC.CollectionCount(1));
                Console.WriteLine("Gen2: " + GC.CollectionCount(2));
                var memory = GC.GetTotalMemory(true);
                Console.WriteLine("MemoryDiff: " + (memory - memoryDiff));
                memoryDiff = memory;
                Console.WriteLine();

                Console.WriteLine("Before collecting:");
                var mocks = objs.Select(x => new MockWrapper(x)).ToList();
                PrintGenerations(mocks);

                cleanUpCallback();
                GC.Collect();
                GC.WaitForFullGCComplete();

                Console.WriteLine();
                Console.WriteLine("After collecting:");
                PrintGenerations(mocks);

                Console.WriteLine();
            }

            private static void PrintGenerations(List<MockWrapper> mocks)
            {
                var currentConsoleColor = Console.ForegroundColor;
                foreach (var gen in mocks.GroupBy(x => x.Generation).OrderByDescending(x => x.Key))
                {
                    Console.ForegroundColor = Char.IsDigit(gen.Key) ? ConsoleColor.White : ConsoleColor.Blue;
                    Console.Write($"{gen.Key}:[");
                    var currentColor = Console.ForegroundColor;
                    var chars = String.Join(' ', gen).TrimEnd(' ');

                    Console.Write(chars);
                    Console.ForegroundColor = currentColor;
                    Console.Write($"] ");
                }
                Console.ForegroundColor = currentConsoleColor;
            }

            private class MockWrapper
            {
                public char Name { get; }
                public bool IsAlive => Target.IsAlive;
                public WeakReference Target { get; }
                public char Generation => IsAlive ? GC.GetGeneration(Target.Target).ToString()[0] : '-';

                public MockWrapper(Mock target)
                {
                    if (target == null) throw new NullReferenceException();

                    Name = target.Name;
                    Target = new WeakReference(target);
                }

                public override string ToString()
                {
                    return $"{Name}";
                }
            }
        }

        internal class Mock
        {
            public Char Name { get; }

            public Mock(char name)
            {
                Name = name;
            }
        }
    }
}