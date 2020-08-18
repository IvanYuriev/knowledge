using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    internal class OnlineWorldTimeClockService
    {
        private static HttpClient _httpClient = new HttpClient();
        private const string BaseAddress = "http://worldtimeapi.org/api";

        public async Task<TimeDTO> GetTimeAsync(string location)
        {
            var requestString = $"{BaseAddress}/{location}";
            var initialThreadId = Thread.CurrentThread.ManagedThreadId;
            
            var stream = await _httpClient.GetStreamAsync(requestString);

            var timeDTO = await JsonSerializer.DeserializeAsync<TimeDTO>(stream);

            timeDTO.Location = location;
            timeDTO.InitialThreadId = initialThreadId;
            timeDTO.CompletionThreadId = Thread.CurrentThread.ManagedThreadId;
            return timeDTO;
        }
    }
}