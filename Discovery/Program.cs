using Discovery.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AreaRepository>();
builder.Services.AddSingleton<RobotRepository>();
builder.Services.AddScoped<RobotsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/area", (Area area, AreaRepository repository) => 
    Results.Ok(repository.Add(area)));

app.MapGet("/robot/{id}", (int id, RobotRepository repository) =>
{
    var robot = repository.Get(id);
    if (robot is null)
    {
        return Results.NotFound($"Robot with id {id} not found");
    }
    return Results.Ok(robot);
});

app.MapPost("/robot", (Robot robot, RobotRepository repository) => 
    Results.Ok(repository.Add(robot)));

app.MapDelete("/robot", (int id, RobotRepository repository) =>
    repository.Remove(id) == 0 ? Results.NotFound($"Robot with id {id} not found") : Results.Ok());

app.MapPost("/robot/{id}/command/{command}", (int id, RobotCommand command, RobotsService robotService) =>
{
    try
    {
        return Results.Ok(robotService.Execute(id, command).Position);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});


app.Run();