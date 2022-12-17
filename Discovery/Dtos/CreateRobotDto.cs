namespace Discovery.Dtos;

public record CreateRobotDto(
    decimal X, 
    decimal Y, 
    decimal AreaX1, 
    decimal AreaY1,
    decimal AreaX2,
    decimal AreaY2);