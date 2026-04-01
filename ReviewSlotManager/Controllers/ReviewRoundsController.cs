using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class ReviewRoundsController : BaseController<IReviewRoundService, ReviewRoundDto>
{
    private readonly IReviewRoundService _service;

    public ReviewRoundsController(IReviewRoundService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("open")]
    public async Task<IActionResult> GetOpenRounds()
    {
        var result = await _service.GetOpenRounds();
        return Ok(result);
    }

    /// <summary>
    /// Danh sach review round theo hoc ky (semesterId).
    /// </summary>
    [HttpGet("semester/{semesterId:int}")]
    [ProducesResponseType(typeof(List<ReviewRoundDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySemester(int semesterId)
    {
        var result = await _service.GetBySemester(semesterId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewRoundDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.RoundId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewRoundDto dto)
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
