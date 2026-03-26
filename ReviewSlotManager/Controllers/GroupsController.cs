using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class GroupsController : BaseController<IGroupService, GroupDto>
{
    private readonly IGroupService _service;

    public GroupsController(IGroupService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _service.Count();
        return Ok(new { count });
    }

    [HttpGet("semester/{semesterId:int}")]
    public async Task<IActionResult> GetBySemester(int semesterId)
    {
        var result = await _service.GetBySemester(semesterId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.GroupId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGroupDto dto)
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
