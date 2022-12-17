using Discovery.Dtos;
using MediatR;

namespace Discovery.Commands;

public record CreateRobotCommand(CreateRobotDto Robot): IRequest<RobotDto>;