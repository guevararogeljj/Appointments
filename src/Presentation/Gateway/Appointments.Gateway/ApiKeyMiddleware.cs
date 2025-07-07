namespace Appointments.Gateway;

public class ApiKeyMiddleware {
    private readonly RequestDelegate _next;
    private
        const string APIKEY = "XApiKey";
    public ApiKeyMiddleware(RequestDelegate next) {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(APIKEY, out
                var extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided ");
            return;
        }

        context.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImRmN2I5MGY0LWI5MzEtNGFmOS1iZTk5LTE0MThjNjg2NDhhZiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGxvY2FsaG9zdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzUxNTY1OTU5LCJpc3MiOiJBcHBvaW50bWVudHNBUEkiLCJhdWQiOiJBcHBvaW50bWVudHNDbGllbnQifQ.4Jr5_e5ROpj6c2rrw3W9-qWfNEtsU9VMAai3R6omn7o");
        
        var appSettings = context.RequestServices.GetRequiredService < IConfiguration > ();
        var apiKey = appSettings.GetValue < string > (APIKEY);
        if (!apiKey.Equals(extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        await _next(context);
    }
}