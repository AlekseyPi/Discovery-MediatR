namespace Discovery.Domain;

public class RobotRepository
{
    private static readonly List<Robot> Robots = new();

    public async Task<Robot?> Get(int id)
    {
        return await Task.FromResult(Robots.FirstOrDefault(r => r.Id == id));
    }
    
    public async Task<Robot[]> GetAll()
    {
        return await Task.FromResult(Robots.ToArray());
    }

    public async Task<Robot> Add(Robot robot)
    {
        var maxRobotId = Robots.Any() ? Robots.Max(r => r.Id) : 0;
        robot.SetId(maxRobotId + 1);
        Robots.Add(robot);
        return await Task.FromResult(robot);
    }

    public async Task<Robot> Update(Robot robot)
    {
        Robots.RemoveAll(r => r.Id == robot.Id);
        Robots.Add(robot);
        return await Task.FromResult(robot);
    }

    public async Task<int?> Remove(int id)
    {
        var robot = Robots.SingleOrDefault(r => r.Id == id);
        if (robot is null)
        {
            return await Task.FromResult<int?>(null);
        }
        Robots.Remove(robot);
        return await Task.FromResult(robot.Id);
    }
}