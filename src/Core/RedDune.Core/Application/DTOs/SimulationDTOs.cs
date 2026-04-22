namespace RedDune.Core.Application.DTOs;

public sealed record RobotInput(
    int X,
    int Y,
    char Orientation,
    string Commands
);

public sealed record SimulationInput(
    int GridWidth,
    int GridHeight,
    IReadOnlyList<RobotInput> Robots
);

public sealed record RobotOutput(
    int X,
    int Y,
    char Orientation,
    bool IsLost
);

public sealed record SimulationOutput(
    IReadOnlyList<RobotOutput> Robots
);