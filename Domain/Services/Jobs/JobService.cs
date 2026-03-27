using Domain.Abstraction;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Jobs;

public class JobService
{
    private readonly IJobRepository _repo;
    private readonly ILogger _logger;

    public JobService(IJobRepository repo, ILogger<JobService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<JobResponse[]> GetJobs()
    {
        var jobs = await _repo.GetAllActive();

        return jobs.Select(Map).ToArray();
    }

    public async Task<JobResponse> Add(CreateJobRequest request)
    {
        var job = new JobPost
        {
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            PostedDate = DateTime.UtcNow,
            IsActive = true
        };

        job = await _repo.Add(job);

        _logger.LogInformation($"Job {job.Title} created");

        return Map(job);
    }

    public async Task<JobResponse> Update(int id, CreateJobRequest request)
    {
        var job = await _repo.Get(id);

        if (job == null)
            throw new Exception("Job not found");

        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirements = request.Requirements;

        job = await _repo.Update(job);

        return Map(job);
    }

    public async Task Close(int id)
    {
        var job = await _repo.Get(id);

        if (job == null)
            throw new Exception("Job not found");

        job.IsActive = false;

        await _repo.Update(job);
    }

    public Task Delete(int id)
        => _repo.Delete(id);

    private JobResponse Map(JobPost job) => new()
    {
        Id = job.Id,
        Title = job.Title
    };
}