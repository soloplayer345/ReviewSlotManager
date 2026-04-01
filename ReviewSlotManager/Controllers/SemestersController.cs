using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class SemestersController : BaseController<ISemesterService, SemesterDto>
{
    private readonly ISemesterService _service;

    public SemestersController(ISemesterService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _service.Count();
        return Ok(new { count });
    }

    /// <summary>
    /// Lay semester dang IsActive = true. Neu khong co semester active thi tra 404 (FE nen bat case nay).
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(SemesterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveSemester();
        if (result is null)
        {
            return NotFound(new { error = "No active semester configured.", status = 404 });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSemesterDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.SemesterId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSemesterDto dto)
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
