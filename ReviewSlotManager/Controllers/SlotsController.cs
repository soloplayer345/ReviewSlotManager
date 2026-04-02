using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class SlotsController : BaseController<ISlotService, SlotDto>
{
    private readonly ISlotService _service;

    public SlotsController(ISlotService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("round/{roundId:int}")]
    public async Task<IActionResult> GetByRound(int roundId)
    {
        var result = await _service.GetAvailableSlotsByRound(roundId);
        return Ok(result);
    }

    [HttpGet("round/{roundId:int}/details")]
    public async Task<IActionResult> GetDetailsByRound(int roundId)
    {
        var result = await _service.GetDetailsByRound(roundId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSlotDto dto)
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(new { error = "Invalid token: missing user ID." });

        dto.CreatedBy = userId;
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.SlotId }, result);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSlotDto dto)
    {
        var result = await _service.Update(id, dto);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok(new { message = "Slot deleted successfully." });
    }
}
