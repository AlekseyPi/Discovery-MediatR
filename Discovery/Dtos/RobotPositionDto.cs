using Discovery.Domain;

namespace Discovery.Dtos;

public record RobotPositionDto(int Id, decimal X, decimal Y);

public static partial class RobotDtoExtensions
{
    public static RobotPositionDto ToPositionDto(this Robot robot)
    {
        return new(robot.Id, robot.X, robot.Y);
    }
}