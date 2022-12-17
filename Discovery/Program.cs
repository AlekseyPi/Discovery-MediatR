using System;
using System.Net.Mime;
using Discovery.Domain;
using Microsoft.AspNetCore.Diagnostics;

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

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = MediaTypeNames.Text.Plain;
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature is not null)
        {
            await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
        }
    });
});

app.MapGet("/robot/{id}", (int id, RobotRepository repository) =>
{
    var robot = repository.Get(id);
    if (robot is null)
    {
        return Results.NotFound($"Robot with id {id} not found");
    }
    return Results.Ok(robot);
});

app.MapPost("/robot", (Robot robot, RobotsService robotService) => Results.Ok(robotService.Add(robot)));

app.MapDelete("/robot", (int id, RobotRepository repository) =>
    repository.Remove(id) == 0 ? Results.NotFound($"Robot with id {id} not found") : Results.Ok());

app.MapPost("/commands/{commands}", (string commands, RobotsService robotService) =>
{
    var commandsArray = commands.Split(",").Select(ParseCommand).ToArray();
    return Results.Ok(robotService.Execute(commandsArray));
    
    Command ParseCommand(string s)
    {
        var allSupportedCommands = String.Join(", ", Enum.GetNames<Command>());
        return Enum.TryParse(s, true, out Command command) && Enum.IsDefined(typeof(Command), command)
            ? command
            : throw new Exception($"Unknown command: '{s}'. Supported commands: {allSupportedCommands}");
    }
});


app.Run();