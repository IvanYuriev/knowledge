using System.Threading.Tasks;

namespace AsyncAwait.Services
{
    public interface IWorldTimeService
    {
        Task<TimeDTO> GetTimeAsync(string location);
    }
}