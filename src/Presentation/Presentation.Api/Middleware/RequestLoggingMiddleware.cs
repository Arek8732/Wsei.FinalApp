using System.Text;

namespace Presentation.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();
        string bodyText = string.Empty;
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            bodyText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        Console.WriteLine($">>> {context.Request.Method} {context.Request.Path}");
        foreach (var h in context.Request.Headers) Console.WriteLine($"    {h.Key}: {h.Value}");
        if (!string.IsNullOrWhiteSpace(bodyText)) Console.WriteLine($"BODY: {bodyText}");

        await _next(context);
    }
}

public static class RequestLoggingExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();
}
