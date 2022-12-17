using MediatR;

namespace Discovery.Commands;

public record DeleteRobotCommand(int Id): IRequest<int?>;