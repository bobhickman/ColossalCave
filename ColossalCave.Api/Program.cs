using ColossalCave.Service;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ICommandParser, CommandParser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapPost("/api/command", async (HttpContext context, IGameService gameService) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var command = await reader.ReadToEndAsync();
    var response = gameService.ProcessCommand(command);
    await context.Response.WriteAsync(response);
});

app.MapGet("/api/heartbeat", async (HttpContext context, IGameService gameService) =>
{
    await context.Response.WriteAsync("Alive");
});

app.Run();
