using Discovery.Domain;
using Discovery.Dtos;
using Discovery.Queries;
using MediatR;

namespace Discovery.Handlers;

public class GetRobotHandler : IRequestHandler<GetRobotQuery, RobotDto?>
{
    private readonly RobotRepository _robotRepository;

    public GetRobotHandler(RobotRepository robotRepository)
    {
        _robotRepository = robotRepository;
    }

    public async Task<RobotDto?> Handle(GetRobotQuery request, CancellationToken cancellationToken)
    {
        return (await _robotRepository.Get(request.Id))?.ToDto();
    }
}