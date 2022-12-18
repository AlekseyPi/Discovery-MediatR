using Discovery.Dtos;
using MediatR;

namespace Discovery.Queries;

public record GetRobotQuery(int Id) : IRequest<RobotDto?>;