using Discovery.Dtos;
using MediatR;

namespace Discovery.Commands;

public record ExecuteCommandsCommand(string Commands) : IRequest<RobotsStateDto>;