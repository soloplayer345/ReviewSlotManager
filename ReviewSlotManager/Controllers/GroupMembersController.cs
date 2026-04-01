using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

public class GroupMembersController : BaseController<IGroupMemberService, GroupMemberDto>
{
    private readonly IGroupMemberService _service;

    public GroupMembersController(IGroupMemberService service) : base(service)
    {
        _service = service;
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _service.Count();
        return Ok(new { count });
    }

    [HttpGet("group/{groupId:int}")]
    public async Task<IActionResult> GetByGroup(int groupId)
    {
        var result = await _service.GetByGroup(groupId);
        return Ok(result);
    }

    /// <summary>
    /// Tat ca membership cua mot sinh vien (co the nhieu dong neu du lieu lich su / nhieu ky).
    /// </summary>
    [HttpGet("student/{studentId:int}")]
    [ProducesResponseType(typeof(List<GroupMemberDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(int studentId)
    {
        var result = await _service.GetByStudent(studentId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupMemberDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.MemberId }, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}
