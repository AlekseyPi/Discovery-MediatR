namespace Discovery.Domain;

public class RobotsService
{
    private readonly RobotRepository _robotRepository;

    public RobotsService(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public Robot Add(Robot robot)
    {
        return _robotRepository.Add(robot);
    }

    public Robot[] Execute(Command[] commands)
    {
        var robots = _robotRepository.GetAll();

        foreach (var robot in robots)
        {
            foreach (var command in commands)
            {
                robot.Execute(command);
            }

            _robotRepository.Update(robot);
        }

        return robots;
    }
}