using Domain.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Storage.Repositories;

public class JobRepository : BaseRepository<JobPost>, IJobRepository
{
    public JobRepository(DataContext context) : base(context)
    {
    }

    public async Task<JobPost> Update(JobPost job)
    {
        _context.JobPosts.Update(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public Task<List<JobPost>> GetAllActive()
    {
        return _context.JobPosts
            .AsNoTracking()
            .Where(j => j.IsActive)
            .OrderByDescending(j => j.PostedDate)
            .ToListAsync();
    }

    public async Task Delete(int id)
    {
        var job = await _context.JobPosts.FindAsync(id);

        if (job == null)
            return;

        _context.JobPosts.Remove(job);
        await _context.SaveChangesAsync();
    }
}