namespace Discovery.Domain;

public class RobotsService
{
    private readonly RobotRepository _robotRepository;

    public RobotsService(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public Robot Execute(int robotId, RobotCommand command)
    {
        var robot = _robotRepository.Get(robotId) ?? throw new Exception($"Robot with id {robotId} not found");
        var position = robot.Position;
        switch (command)
        {
            case RobotCommand.Advance:
                switch (position.DirectionAngle)
                {
                    case 0:
                        position = position with {Y = position.Y + 1};
                        break;
                    case 90:
                        position = position with {X = position.X + 1};
                        break;
                    case 180:
                        position = position with {Y = position.Y - 1};
                        break;
                    case 270:
                        position = position with {Y = position.X - 1};
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid robot angle {position.DirectionAngle}");
                };
                break;
                
            case RobotCommand.Left:
                var turnLeftAngle = position.DirectionAngle == 0 ? 270 : position.DirectionAngle - 90; 
                position = position with {DirectionAngle = turnLeftAngle};
                break;
            case RobotCommand.Right:
                var turnRightAngle = position.DirectionAngle == 270 ? 0 : position.DirectionAngle + 90; 
                position = position with {DirectionAngle = turnRightAngle};
                break;
            default:
                throw new NotImplementedException($"Command {command} is not implemented");
        }

        return _robotRepository.Update(robot with {Position = position});
    }
}