namespace Discovery.Domain;

public class RobotRepository
{
    private static readonly List<Robot> Robots = new();

    public Robot? Get(int id)
    {
        return Robots.FirstOrDefault(r => r.Id == id);
    }

    public Robot Add(Robot robot)
    {
        var maxRobotId = Robots.Any() ? Robots.Max(r => r.Id) : 0;
        var newRobot = robot with {Id = maxRobotId + 1};
        Robots.Add(newRobot);
        return newRobot;
    }

    public Robot Update(Robot robot)
    {
        Robots.RemoveAll(r => r.Id == robot.Id);
        Robots.Add(robot);
        return robot;
    }

    public int Remove(int id)
    {
        return Robots.RemoveAll(r => r.Id == id);
    }
}