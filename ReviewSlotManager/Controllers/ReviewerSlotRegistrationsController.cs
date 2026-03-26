using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

[Route("api/[controller]")]
public class ReviewerSlotRegistrationsController : ControllerBase
{
    private readonly IReviewerSlotRegistrationService _service;

    public ReviewerSlotRegistrationsController(IReviewerSlotRegistrationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        var result = await _service.Read(pageSize, pageNumber);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateReviewerSlotRegistrationDto dto)
    {
        var result = await _service.Register(dto);
        return Ok(result);
    }

    [HttpPut("{registrationId:int}/cancel")]
    public async Task<IActionResult> Cancel(int registrationId)
    {
        await _service.Cancel(registrationId);
        return NoContent();
    }
}
