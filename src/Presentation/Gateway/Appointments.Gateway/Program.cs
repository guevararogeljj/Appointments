using Appointments.Gateway;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
//Eneable cors
app.UseCors("AllowAll");
app.MapReverseProxy();
app.MapGet("/", () => "Hello World! yarp");
app.UseMiddleware<ApiKeyMiddleware>();

app.Run();