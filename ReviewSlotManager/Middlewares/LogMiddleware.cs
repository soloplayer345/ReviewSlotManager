namespace ReviewSlotManager.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogMiddleware> _logger;
    private readonly string _message;

    public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger, string message)
    {
        _next = next;
        _logger = logger;
        _message = message;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation("{Message} - {Method} {Path}", _message, context.Request.Method, context.Request.Path);
        await _next(context);
    }
}
