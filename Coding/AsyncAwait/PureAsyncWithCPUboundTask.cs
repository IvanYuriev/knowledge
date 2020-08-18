using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AsyncAwait
{
    internal class PureAsyncWithCPUboundTask
    {
        public void Run(int limit = Int32.MaxValue)
        {
            var clock = new OnlineWorldTimeClockService();
            var locations = GetLocations().Take(limit).ToArray();
            
            Parallel.ForEach(locations, (place) => 
            {
                PrintTime(clock.GetTimeAsync(place).GetAwaiter().GetResult());
            });

            // foreach(var place in locations)
            // {
            //     PrintTime(clock.GetTimeAsync(place).GetAwaiter().GetResult());
            // }
        }

        private static void PrintTime(TimeDTO t)
        {
            Console.WriteLine($"It is {t.DateTime,-30} now at {t.Location,-20} ({t.InitialThreadId}->{t.CompletionThreadId})");
        }

        private IEnumerable<string> GetLocations()
        {
            using var jsonStream = new WebClient().OpenRead("http://worldtimeapi.org/api/timezone");
            using var jsonString = new StreamReader(jsonStream);
            return jsonString.ReadToEnd().Trim(new [] { '[', ']' }).Split(',').Select(x => x.Trim('"'));
        }

    }
}