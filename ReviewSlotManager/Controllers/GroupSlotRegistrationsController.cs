using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

[Route("api/[controller]")]
public class GroupSlotRegistrationsController : ControllerBase
{
    private readonly IGroupSlotRegistrationService _service;

    public GroupSlotRegistrationsController(IGroupSlotRegistrationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        var result = await _service.Read(pageSize, pageNumber);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetById(id);
        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _service.Count();
        return Ok(new { count });
    }

    [HttpGet("slot/{slotId:int}")]
    public async Task<IActionResult> GetBySlot(int slotId)
    {
        var result = await _service.GetBySlot(slotId);
        return Ok(result);
    }

    [HttpGet("group/{groupId:int}")]
    public async Task<IActionResult> GetByGroup(int groupId)
    {
        var result = await _service.GetByGroup(groupId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateGroupSlotRegistrationDto dto)
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
