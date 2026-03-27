using Domain.Services.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjASP.Net.Controllers;
[ApiController]
[Route("api/v1/jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _service;

    public JobsController(JobService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var jobs = await _service.GetJobs();
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobRequest request)
    {
        var job = await _service.Add(request);
        return Ok(job);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateJobRequest request)
    {
        var job = await _service.Update(id, request);
        return Ok(job);
    }

    [HttpPut("{id}/close")]
    public async Task<IActionResult> Close(int id)
    {
        await _service.Close(id);
        return Ok(new { Message = "Job closed successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok(new { Message = "Job deleted successfully" });
    }
}