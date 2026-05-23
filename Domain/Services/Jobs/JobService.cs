using Amazon.DynamoDBv2.DataModel;
using Domain.Abstraction;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Jobs;

public class JobService
{
    private readonly IJobRepository _repo;
    private readonly DynamoDBContext _dynamo;
    private readonly ILogger _logger;

    public JobService(DynamoDBContext dynamo, IJobRepository repo, ILogger<JobService> logger)
    {
        _repo = repo;
        _logger = logger;
        _dynamo = dynamo;
    }

    public async Task<JobResponse[]> GetJobs()
    {
        var jobs = await _repo.GetAllActive();

        return jobs.Select(Map).ToArray();
    }

    public async Task<JobResponse> Add(CreateJobRequest request)
    {
        
        //DYNAMODB
        var job = new JobPostDynamo
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            PostedDate = DateTime.UtcNow,
            IsActive = true
        };
        await _dynamo.SaveAsync(job);
        _logger.LogInformation($"Job {job.Title} created");

        return new JobResponse
        {
            Id = int.Parse(job.Id.GetHashCode().ToString()),
            Title = job.Title
        };
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
    //DYNAMODB
    public async Task<JobPostDynamo?> GetById(string id)
    {
        return await _dynamo.LoadAsync<JobPostDynamo>(id);
    }
    //DYNAMODB
    public async Task<List<JobPostDynamo>> GetActiveJobs()
    {
        var scan = _dynamo.ScanAsync<JobPostDynamo>(new List<ScanCondition>
        {
            new ScanCondition("IsActive", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, true)
        });

        return await scan.GetRemainingAsync();
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