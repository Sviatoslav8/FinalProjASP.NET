using Domain.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Storage.Repositories;

public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
{
    public ApplicationRepository(DataContext context) : base(context)
    {
    }

    public Task<List<Application>> GetByUserId(int userId)
    {
        return _context.Applications
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public Task<List<Application>> GetByJobId(int jobId)
    {
        return _context.Applications
            .AsNoTracking()
            .Where(a => a.JobPostId == jobId)
            .ToListAsync();
    }

    public async Task<Application> Update(Application application)
    {
        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
        return application;
    }
}