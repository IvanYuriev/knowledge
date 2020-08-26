using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Services
{
    internal class OnlineWorldTimeService : IWorldTimeService
    {
        private static HttpClient _httpClient = new HttpClient();
        private const string BaseAddress = "http://worldtimeapi.org/api";

        public async Task<TimeDTO> GetTimeAsync(string location)
        {
            // async PREFIX - running in Caller Thread
            var requestString = $"{BaseAddress}/{location}";
            var initialThreadId = Thread.CurrentThread.ManagedThreadId;

            // async request - NON of the threads is processing it
            var stream = await _httpClient.GetStreamAsync(requestString);

            // AFTER async operation - some thread from the pool continue this method
            // CPU bound task - some thread is running it
            var deserializeThreadId = Thread.CurrentThread.ManagedThreadId;
            var timeDTO = await JsonSerializer.DeserializeAsync<TimeDTO>(stream);

            // AFTER 2nd async operation - some thread from the pool continue this method
            timeDTO.Location = location;
            timeDTO.InitialThreadId = initialThreadId;
            timeDTO.MiddleThreadId = deserializeThreadId;
            timeDTO.CompletionThreadId = Thread.CurrentThread.ManagedThreadId;
            return timeDTO;
        }
    }
}