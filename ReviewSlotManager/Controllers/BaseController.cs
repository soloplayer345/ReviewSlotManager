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
}
