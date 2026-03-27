using System.Security.Claims;
using Domain.Services.Applications;
using Domain.Services.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjASP.Net.Controllers;
[ApiController]
[Route("api/v1/applications")]
public class ApplicationController : ControllerBase
{
    private readonly ApplicationService _service;

    public ApplicationController(ApplicationService service)
    {
        _service = service;
    }
    
    
    [HttpPost("{jobId}/users/{userId}")]
    public async Task<IActionResult> Apply(int jobId, int userId, CreateApplicationRequest request)
    {
        var result = await _service.Apply(jobId, userId, request);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserApps(int userId)
    {
        var apps = await _service.GetUserApplications(userId);
        return Ok(apps);
    }

    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetByJob(int jobId)
    {
        var apps = await _service.GetJobApplications(jobId);
        return Ok(apps);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateApplicationStatusRequest request)
    {
        var app = await _service.UpdateStatus(id, request);
        return Ok(app);
    }
}