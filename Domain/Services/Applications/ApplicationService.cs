using Amazon.DynamoDBv2.DataModel;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Applications;

public class ApplicationService
{
    private readonly DynamoDBContext _dynamo; 
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(DynamoDBContext dynamo, ILogger<ApplicationService> logger)
    {
        _dynamo = dynamo;
        _logger = logger;
    }


    public async Task<ApplicationResponse> Apply(string jobId, CreateApplicationRequest request)
    {
        var appDynamo = new ApplicationDynamo
        {
            Id = Guid.NewGuid().ToString(), 
            JobPostId = jobId,           
            UserId = "Anonymous",        
            Status = "Pending",
            CoverLetter = request.CoverLetter,
            AppliedDate = DateTime.UtcNow
        };

        await _dynamo.SaveAsync(appDynamo);
        _logger.LogInformation($"Anonymous application {appDynamo.Id} created for job {jobId}");
        
        return new ApplicationResponse 
        { 
            Id = appDynamo.Id, 
            Status = appDynamo.Status 
        };
    }
    
    public async Task<ApplicationDynamo?> GetById(string id)
    {
        return await _dynamo.LoadAsync<ApplicationDynamo>(id);
    }
    
    public async Task<ApplicationDynamo[]> GetJobApplications(string jobId)
    {
        var scan = _dynamo.ScanAsync<ApplicationDynamo>(new List<ScanCondition>
        {
            new ScanCondition("JobPostId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, jobId)
        });

        var results = await scan.GetRemainingAsync();
        return results.ToArray();
    }
    
    public async Task<ApplicationResponse> UpdateStatus(string id, UpdateApplicationStatusRequest request)
    {
        var app = await _dynamo.LoadAsync<ApplicationDynamo>(id);

        if (app == null)
            throw new Exception($"Application with ID {id} not found");

        app.Status = request.Status;

        await _dynamo.SaveAsync(app);

        return new ApplicationResponse
        {
            Id = app.Id,
            Status = app.Status
        };
    }
}