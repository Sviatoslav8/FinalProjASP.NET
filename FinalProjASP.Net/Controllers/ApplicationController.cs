using Domain.Services.Applications;
using Domain.Models;
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
    
    // 1. Подати заявку на вакансію (в URL передаємо тільки jobId)
    [HttpPost("{jobId}")]
    public async Task<IActionResult> Apply(
        [FromRoute] string jobId, 
        [FromBody] CreateApplicationRequest request)
    {
        if (request == null)
            return BadRequest(new { Message = "Request body cannot be null" });

        var result = await _service.Apply(jobId, request);
        return Ok(result);
    }

    // 2. Отримати конкретну заявку за її ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var app = await _service.GetById(id);
        
        if (app == null)
            return NotFound(new { Message = $"Application with ID {id} not found" });

        return Ok(app);
    }

    // 3. Отримати всі заявки, які прийшли на конкретну вакансію
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetByJob([FromRoute] string jobId)
    {
        var apps = await _service.GetJobApplications(jobId);
        return Ok(apps);
    }

    // 4. Змінити статус заявки (прийняти/відхилити)
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] string id, 
        [FromBody] UpdateApplicationStatusRequest request)
    {
        if (request == null)
            return BadRequest(new { Message = "Request body cannot be null" });

        try
        {
            var app = await _service.UpdateStatus(id, request);
            return Ok(app);
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}