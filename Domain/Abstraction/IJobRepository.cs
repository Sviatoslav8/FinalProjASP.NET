using Domain.Models;

namespace Domain.Abstraction;

public interface IJobRepository
{
    Task<JobPost> Add(JobPost job);
    Task<JobPost> Update(JobPost job);
    Task<List<JobPost>> GetAllActive();
    ValueTask<JobPost?> Get(int id);
    Task Delete(int id);
}