using System.ComponentModel;
using System.Net.Mime;
using System.Reflection;
using Discovery.Commands;
using Discovery.Domain;
using Discovery.Dtos;
using Discovery.Extensions;
using Discovery.Queries;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
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


app.MapGet("/robot/{id}", 
    [SwaggerOperation(
        Summary = "Returns single robot",
        Description = "Returns full information about single robot")
    ]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RobotDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    async (int id, IMediator mediator) =>
{
    var robot = await mediator.Send(new GetRobotQuery(id));
    if (robot is null)
    {
        return Results.NotFound($"Robot with id {id} not found");
    }
    return Results.Ok(robot);
});

app.MapPost("/robot", 
    [SwaggerOperation(
        Summary = "Creates single robot",
        Description = "Returns full information about created robot")
    ]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RobotDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "AreaX1/Y1 must be smaller then AreaX2/Y2")]
    async (CreateRobotDto robot, IMediator mediator) => 
    Results.Ok(await mediator.Send(new CreateRobotCommand(robot))));

app.MapDelete("/robot/{id}", 
    [SwaggerOperation(
        Summary = "Deletes single robot")
    ]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    async (int id, IMediator mediator) =>
{
    var deletedId = await mediator.Send(new DeleteRobotCommand(id));
    return deletedId == id ? Results.Ok() : Results.NotFound($"Robot with id {id} not found");
});

app.MapPost("/commands/{commands}", 
    [SwaggerOperation(
        Summary = "Sends list of commands to all robots",
        Description = "Every robot in simulation executes list of provided commands")
    ]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RobotsStateDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Commands string should contain valid comma separated values")]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    async ([SwaggerParameter($"Comma separated list of values: {nameof(Command.Advance)}, {nameof(Command.Left)}, {nameof(Command.Right)}")] 
            string commands, IMediator mediator) => 
    Results.Ok(await mediator.Send(new ExecuteCommandsCommand(commands))));

app.Run();