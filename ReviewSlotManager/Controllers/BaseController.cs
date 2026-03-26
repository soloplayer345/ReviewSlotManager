using ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace ReviewSlotManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<TService, TDto> : ControllerBase
    where TService : IBaseService<TDto>
    where TDto : class
{
    protected readonly TService Service;

    public BaseController(TService service)
    {
        Service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get([FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        var result = await Service.Read(pageSize, pageNumber);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        var result = await Service.Read(id);
        return Ok(result);
    }
}
