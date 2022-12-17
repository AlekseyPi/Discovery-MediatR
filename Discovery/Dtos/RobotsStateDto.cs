namespace Discovery.Dtos;

public record RobotsStateDto(RobotPositionDto[] RobotsInsideTheyAreas, int[] RobotsOutsideTheyAreas);