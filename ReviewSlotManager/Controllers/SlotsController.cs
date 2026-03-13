using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class SlotsController : BaseController<SlotService, SlotDto>
{
    private readonly SlotService _service;

    public SlotsController(SlotService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("round/{roundId:int}")]
    public async Task<IActionResult> GetByRound(int roundId)
    {
        var result = await _service.GetAvailableSlotsByRound(roundId);
        return Ok(result);
    }
}
