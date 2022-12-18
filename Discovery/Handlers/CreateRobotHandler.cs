using Discovery.Commands;
using Discovery.Domain;
using Discovery.Dtos;
using MediatR;

namespace Discovery.Handlers;

public class CreateRobotHandler : IRequestHandler<CreateRobotCommand, RobotDto>
{
    private readonly RobotRepository _robotRepository;

    public CreateRobotHandler(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public async Task<RobotDto> Handle(CreateRobotCommand request, CancellationToken cancellationToken)
    {
        var robot = new Robot(
            0,
            request.Robot.X,
            request.Robot.Y,
            0,
            request.Robot.AreaX1,
            request.Robot.AreaY1,
            request.Robot.AreaX2,
            request.Robot.AreaY2
        );
        return (await _robotRepository.Add(robot)).ToDto();
    }
}