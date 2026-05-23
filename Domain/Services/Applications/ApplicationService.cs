using Amazon.DynamoDBv2.DataModel;
using Domain.Abstraction;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Applications;

public class ApplicationService
{
    private readonly IApplicationRepository _repo;
    private readonly DynamoDBContext _dynamo; 
    private readonly ILogger _logger;

    public ApplicationService(DynamoDBContext dynamo, IApplicationRepository repo, ILogger<ApplicationService> logger)
    {
        _dynamo = dynamo;
        _repo = repo;
        _logger = logger;
    }

    public async Task<ApplicationResponse> Apply(int jobId, int userId, CreateApplicationRequest request)
    {
        var app = new Application()
        {
            JobPostId = jobId,
            UserId = userId,
            CoverLetter = request.CoverLetter,
            AppliedDate = DateTime.UtcNow,
            Status = "Pending"
        };

        app = await _repo.Add(app);
        //DYNAMODB
        await _dynamo.SaveAsync(new ApplicationDynamo
        {
            Id = app.Id.ToString(),
            JobPostId = jobId.ToString(),
            UserId = userId.ToString(),
            Status = "Pending",
            CoverLetter = request.CoverLetter,
            AppliedDate = DateTime.UtcNow
        });
        
        _logger.LogInformation($"User {userId} applied to job {jobId}");
        
        return Map(app);
    }
    //DYNAMODB
    public async Task<ApplicationDynamo?> GetById(string id)
    {
        return await _dynamo.LoadAsync<ApplicationDynamo>(id);
    }
    //DYNAMODB
    public async Task<List<ApplicationDynamo>> GetUserApplications(string userId)
    {
        var scan = _dynamo.ScanAsync<ApplicationDynamo>(new List<ScanCondition>
        {
            new ScanCondition("UserId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, userId)
        });

        return await scan.GetRemainingAsync();
    }
    public async Task<ApplicationResponse[]> GetUserApplications(int userId)
    {
        var apps = await _repo.GetByUserId(userId);

        return apps.Select(Map).ToArray();
    }

    public async Task<ApplicationResponse[]> GetJobApplications(int jobId)
    {
        var apps = await _repo.GetByJobId(jobId);

        return apps.Select(Map).ToArray();
    }

    public async Task<ApplicationResponse> UpdateStatus(int id, UpdateApplicationStatusRequest request)
    {
        var app = await _repo.Get(id);

        if (app == null)
            throw new Exception("Application not found");

        app.Status = request.Status;

        app = await _repo.Update(app);

        return Map(app);
    }

    private ApplicationResponse Map(Application app) => new()
    {
        Id = app.Id,
        Status = app.Status,
    };

}