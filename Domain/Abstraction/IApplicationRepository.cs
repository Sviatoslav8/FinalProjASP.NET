using Domain.Models;

namespace Domain.Abstraction;

public interface IApplicationRepository
{
    Task<Application> Add(Application application);
    Task<List<Application>> GetByUserId(int userId);
    Task<List<Application>> GetByJobId(int jobId);
    ValueTask<Application?> Get(int id);
    Task<Application> Update(Application application);
}