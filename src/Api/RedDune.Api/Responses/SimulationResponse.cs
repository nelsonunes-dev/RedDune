namespace RedDune.Api.Responses;

public sealed record SimulationResponse(
    IReadOnlyList<RobotResponse> Robots
);

public sealed record RobotResponse(
    int X,
    int Y,
    char Orientation,
    bool IsLost
);