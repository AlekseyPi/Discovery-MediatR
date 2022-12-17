using System.Net.Mime;
using System.Reflection;
using Discovery.Commands;
using Discovery.Domain;
using Discovery.Dtos;
using Discovery.Queries;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<RobotRepository>();

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

app.MapGet("/robot/{id}", async (int id, IMediator mediator) =>
{
    var robot = await mediator.Send(new GetRobotQuery(id));
    if (robot is null)
    {
        return Results.NotFound($"Robot with id {id} not found");
    }
    return Results.Ok(robot);
});

app.MapPost("/robot", async (CreateRobotDto robot, IMediator mediator) => 
    Results.Ok(await mediator.Send(new CreateRobotCommand(robot))));

app.MapDelete("/robot/{id}", async (int id, IMediator mediator) =>
{
    var deletedId = await mediator.Send(new DeleteRobotCommand(id));
    return deletedId == id ? Results.Ok() : Results.NotFound($"Robot with id {id} not found");
});

app.MapPost("/commands/{commands}", async (string commands, IMediator mediator) => 
    Results.Ok(await mediator.Send(new ExecuteCommandsCommand(commands))));

app.Run();