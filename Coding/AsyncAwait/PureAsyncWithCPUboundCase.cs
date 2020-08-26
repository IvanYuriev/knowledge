using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AsyncAwait.Services;

namespace AsyncAwait
{
    internal class PureAsyncWithCPUboundCase
    {
        private readonly IWorldTimeService worldTimeService;

        public PureAsyncWithCPUboundCase(IWorldTimeService worldTimeService)
        {
            this.worldTimeService = worldTimeService;
        }
        public void Run(int limit = Int32.MaxValue)
        {
            var locations = GetLocations().Take(limit).ToArray();
            var results = new ConcurrentBag<TimeDTO>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = 200 };
            // options.TaskScheduler = TaskScheduler.Default;
            // Parallel.ForEach(locations, options, location => {
            //     var timeTask = worldTimeService.GetTimeAsync(location);
            //     var time = timeTask.Result;
            //     PrintTime(time);
            //     results.Add(time);
            // });
            var tasks = locations.Select(location =>
                Task.Run(async () =>
                {
                    var time = await worldTimeService.GetTimeAsync(location);
                    PrintTime(time);
                    results.Add(time);
                }));
            Task.WhenAll(tasks).Wait();
            PrintStatistics(results);
        }
        private void PrintStatistics(ConcurrentBag<TimeDTO> results)
        {
            var asyncOperationSwitchingAmount = results.LongCount(t => t.InitialThreadId != t.MiddleThreadId);
            var cpuOperationSwitchingAmount = results.LongCount(t => t.MiddleThreadId != t.CompletionThreadId);
            var completeInOriginalThreadAmount = results.LongCount(t => t.InitialThreadId == t.CompletionThreadId);

            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Statistics: ");
            Console.WriteLine($"HttpRequest thread switches: {asyncOperationSwitchingAmount}");
            Console.WriteLine($"Deserialize thread switches: {cpuOperationSwitchingAmount}");
            Console.WriteLine($"Initial and Complete threads match: {completeInOriginalThreadAmount}");
        }

        private static void PrintTime(TimeDTO t)
        {
            var threadingInfo = $"{t.InitialThreadId:00}->{t.MiddleThreadId:00}->{t.CompletionThreadId:00}";
            Console.WriteLine($"It is {t.DateTime,-30} now at {t.Location,-30} {threadingInfo} {t.Code}");
        }

        private IEnumerable<string> GetLocations()
        {
            using var jsonStream = new WebClient().OpenRead("http://worldtimeapi.org/api/timezone");
            using var jsonString = new StreamReader(jsonStream);
            return jsonString.ReadToEnd().Trim(new [] { '[', ']' }).Split(',').Select(x => x.Trim('"'));
        }

    }
}