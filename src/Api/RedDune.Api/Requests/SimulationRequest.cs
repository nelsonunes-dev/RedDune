namespace RedDune.Api.Requests;

public sealed record SimulationRequest(
    int GridWidth,
    int GridHeight,
    IReadOnlyList<RobotRequest> Robots
);

public sealed record RobotRequest(
    int X,
    int Y,
    char Orientation,
    string Commands
);