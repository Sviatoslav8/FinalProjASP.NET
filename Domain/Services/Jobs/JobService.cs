using Amazon.DynamoDBv2.DataModel;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Jobs;

public class JobService
{
    private readonly DynamoDBContext _dynamo;
    private readonly ILogger<JobService> _logger;
    
    public JobService(DynamoDBContext dynamo, ILogger<JobService> logger)
    {
        _logger = logger;
        _dynamo = dynamo;
    }
    
    public async Task<JobResponse[]> GetJobs()
    {
        var activeJobs = await GetActiveJobs();
        return activeJobs.Select(job => new JobResponse
        {
            Id = int.Parse(job.Id.GetHashCode().ToString()),
            Title = job.Title
        }).ToArray();
    }

    
    public async Task<JobResponse> Add(CreateJobRequest request)
    {
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
        _logger.LogInformation($"Job {job.Title} created in DynamoDB");

        return new JobResponse
        {
            Id = int.Parse(job.Id.GetHashCode().ToString()),
            Title = job.Title
        };
    }

   
    public async Task<JobResponse> Update(string id, CreateJobRequest request)
    {
        
        var job = await _dynamo.LoadAsync<JobPostDynamo>(id);

        if (job == null)
            throw new Exception("Job not found");

        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirements = request.Requirements;

        await _dynamo.SaveAsync(job);

        return new JobResponse
        {
            Id = int.Parse(job.Id.GetHashCode().ToString()),
            Title = job.Title
        };
    }

    
    public async Task<JobPostDynamo?> GetById(string id)
    {
        return await _dynamo.LoadAsync<JobPostDynamo>(id);
    }

    
    public async Task<List<JobPostDynamo>> GetActiveJobs()
    {
        var scan = _dynamo.ScanAsync<JobPostDynamo>(new List<ScanCondition>
        {
            new ScanCondition("IsActive", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, true)
        });

        return await scan.GetRemainingAsync();
    }

    
    public async Task Close(string id)
    {
        var job = await _dynamo.LoadAsync<JobPostDynamo>(id);

        if (job == null)
            throw new Exception("Job not found");

        job.IsActive = false;

        await _dynamo.SaveAsync(job);
    }

   
    public async Task Delete(string id)
    {
        var job = await _dynamo.LoadAsync<JobPostDynamo>(id);
        if (job != null)
        {
            await _dynamo.DeleteAsync(job);
        }
    }
}