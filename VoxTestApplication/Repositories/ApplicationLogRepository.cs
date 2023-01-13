using VoxTestApplication.Data;
using VoxTestApplication.Models;

namespace VoxTestApplication.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly ApplicationContext _context;
        public ApplicationLogRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task SaveLog(ApplicationLog log)
        {
            await _context.ApplicationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
