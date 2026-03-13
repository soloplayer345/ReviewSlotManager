using Microsoft.AspNetCore.Mvc;

namespace ReviewSlotManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<TService, TDto> : ControllerBase
{
    protected readonly TService Service;

    public BaseController(TService service)
    {
        Service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get([FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
    {
        var method = typeof(TService).GetMethod("Read", [typeof(int), typeof(int)]);
        if (method is null)
        {
            return BadRequest("Read method not found in service.");
        }

        var result = await (Task<List<TDto>>)method.Invoke(Service, [pageSize, pageNumber])!;
        return Ok(result);
    }
}
