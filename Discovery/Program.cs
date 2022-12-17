using System;
using Discovery.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RobotRepository>();
builder.Services.AddScoped<RobotsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/robot/{id}", (int id, RobotRepository repository) =>
{
    var robot = repository.Get(id);
    if (robot is null)
    {
        return Results.NotFound($"Robot with id {id} not found");
    }
    return Results.Ok(robot);
});

app.MapPost("/robot", (Robot robot, RobotsService robotService) =>
{
    try
    {
        return Results.Ok(robotService.Add(robot));
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapDelete("/robot", (int id, RobotRepository repository) =>
    repository.Remove(id) == 0 ? Results.NotFound($"Robot with id {id} not found") : Results.Ok());

app.MapPost("/commands/{commands}", (string commands, RobotsService robotService) =>
{
    try
    {
        var commandsArray = commands.Split(",").Select(ParseCommand).ToArray();
        return Results.Ok(robotService.Execute(commandsArray));
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
    
    Command ParseCommand(string s)
    {
        var allSupportedCommands = String.Join(", ", Enum.GetNames<Command>());
        return Enum.TryParse(s, true, out Command command)
            ? command
            : throw new Exception($"Unknown command: '{s}'. Supported commands: {allSupportedCommands}");
    }
});


app.Run();