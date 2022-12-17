using Discovery.Domain;

namespace Discovery.Dtos;

public record RobotDto(
    int Id, 
    decimal X, 
    decimal Y,
    decimal DirectionAngle,
    decimal AreaX1, 
    decimal AreaY1, 
    decimal AreaX2, 
    decimal AreaY2, 
    bool IsInArea);

public static partial class RobotDtoExtensions
{
    public static RobotDto ToDto(this Robot robot) => 
        new RobotDto(
            robot.Id, 
            robot.X, 
            robot.Y,
            robot.DirectionAngle,
            robot.AreaX1,
            robot.AreaY1,
            robot.AreaX2,
            robot.AreaY2,
            robot.IsInArea);
}