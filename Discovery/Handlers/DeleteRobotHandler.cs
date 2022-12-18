using Discovery.Commands;
using Discovery.Domain;
using MediatR;

namespace Discovery.Handlers;

public class DeleteRobotHandler : IRequestHandler<DeleteRobotCommand, int?>
{
    private readonly RobotRepository _robotRepository;

    public DeleteRobotHandler(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public async Task<int?> Handle(DeleteRobotCommand request, CancellationToken cancellationToken)
    {
        return await _robotRepository.Remove(request.Id);
    }
}