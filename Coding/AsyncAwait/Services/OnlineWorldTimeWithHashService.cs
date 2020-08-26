using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Services
{
    internal partial class OnlineWorldTimeWithHashService : IWorldTimeService
    {
        private static HttpClient _httpClient = new HttpClient();
        private static Hasher _hasher = new Hasher();
        private const string BaseAddress = "http://worldtimeapi.org/api";

        public async Task<TimeDTO> GetTimeAsync(string location)
        {
            // async PREFIX - running in Caller Thread
            var requestString = $"{BaseAddress}/{location}";
            var initialThreadId = Thread.CurrentThread.ManagedThreadId;

            // async request - NON of the threads is processing it
            var bytes = await _httpClient.GetByteArrayAsync(requestString);

            // AFTER async operation - some thread from the pool continue this method
            // CPU bound task - some thread is running it
            var beforeHashThread = Thread.CurrentThread.ManagedThreadId;
            var hash = await Task.Factory.StartNew(() => _hasher.GetBase64Hash(bytes));

            // AFTER 2nd async operation - some thread from the pool continue this method
            var timeDTO = JsonSerializer.Deserialize<TimeDTO>(bytes); //synchronous
            timeDTO.MiddleThreadId = beforeHashThread;
            timeDTO.Code = hash;
            timeDTO.Location = location;
            timeDTO.InitialThreadId = initialThreadId;
            timeDTO.CompletionThreadId = Thread.CurrentThread.ManagedThreadId;
            return timeDTO;
        }
    }
}