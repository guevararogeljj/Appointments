namespace Appointments.Gateway;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY = "XApiKey";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided ");
            return;
        }

        // Reemplaza o agrega el encabezado Authorization
        //context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjY3NjljYzdiLWE4NjYtNDI4NC1hMWQ3LTMwNGFiNjI3MjQwNyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImNsaWVudEBsb2NhbGhvc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImNsaWVudCIsImlkVXNlciI6IjY3NjljYzdiLWE4NjYtNDI4NC1hMWQ3LTMwNGFiNjI3MjQwNyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJleHAiOjE3NTQ0MzIyNDIsImlzcyI6IkFwcG9pbnRtZW50c0FQSSIsImF1ZCI6IkFwcG9pbnRtZW50c0NsaWVudCJ9.I2UGfHNUh_glLdPS6i5jHRC90ySdDwln7ktnoDKnnOQ";

        //get header Bearer token
        var bearerToken = context.Request.Headers["Bearer"].ToString();
        //add to Authorization header
        if (!string.IsNullOrEmpty(bearerToken))
        {
            context.Request.Headers["Authorization"] = $"Bearer {bearerToken}";
        }
  

        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>(APIKEY);
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        await _next(context);
    }
}