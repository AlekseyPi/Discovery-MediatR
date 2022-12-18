using Discovery.Commands;
using Discovery.Domain;
using Discovery.Dtos;
using Discovery.Extensions;
using MediatR;

namespace Discovery.Handlers;

public class ExecuteCommandsHandler : IRequestHandler<ExecuteCommandsCommand, RobotsStateDto>
{
    private readonly RobotRepository _robotRepository;

    public ExecuteCommandsHandler(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public async Task<RobotsStateDto> Handle(ExecuteCommandsCommand request, CancellationToken cancellationToken)
    {
        var commandsArray = request.Commands.Split(",").Select(ParseCommand).ToArray();

        var robots = await ExecuteCommandsOnAllRobots(commandsArray);

        var robotsInsideTheyArea = robots.Where(r => r.IsInArea).Select(r => r.ToPositionDto()).ToArray();
        var robotsOutsideTheyArea = robots.Where(r => !r.IsInArea).Select(r => r.Id).ToArray();

        return new RobotsStateDto(robotsInsideTheyArea, robotsOutsideTheyArea);

        Command ParseCommand(string s)
        {
            return Enum.TryParse(s, true, out Command command) && Enum.IsDefined(typeof(Command), command)
                ? command
                : throw new Exception(
                    $"Unknown command: '{s}'. Supported commands: {EnumExtensions.ValuesAsString<Command>()}.");
        }
    }

    private async Task<Robot[]> ExecuteCommandsOnAllRobots(Command[] commands)
    {
        var robots = await _robotRepository.GetAll();

        foreach (var robot in robots)
        {
            foreach (var command in commands) robot.Execute(command);

            await _robotRepository.Update(robot);
        }

        return robots;
    }
}