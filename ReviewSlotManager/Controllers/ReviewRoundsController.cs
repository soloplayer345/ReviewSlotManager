using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class ReviewRoundsController : BaseController<ReviewRoundService, ReviewRoundDto>
{
    private readonly ReviewRoundService _service;

    public ReviewRoundsController(ReviewRoundService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("open")]
    public async Task<IActionResult> GetOpenRounds()
    {
        var result = await _service.GetOpenRounds();
        return Ok(result);
    }
}
