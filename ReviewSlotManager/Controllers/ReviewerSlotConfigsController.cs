using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class ReviewerSlotConfigsController : BaseController<IReviewerSlotConfigService, ReviewerSlotConfigDto>
{
    private readonly IReviewerSlotConfigService _service;

    public ReviewerSlotConfigsController(IReviewerSlotConfigService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("round/{roundId:int}")]
    public async Task<IActionResult> GetByRound(int roundId)
    {
        var result = await _service.GetByRound(roundId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewerSlotConfigDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.ConfigId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewerSlotConfigDto dto)
    {
        var result = await _service.Update(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}
