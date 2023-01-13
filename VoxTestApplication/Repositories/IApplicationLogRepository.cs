using VoxTestApplication.Models;

namespace VoxTestApplication.Repositories
{
    public interface IApplicationLogRepository
    {
        Task SaveLog(ApplicationLog log);
    }
}
