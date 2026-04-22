# RedDune

Martian Robots simulation system - deterministic robot navigation on a bounded grid.

## Purpose

Simulate robotic movement on a bounded grid, producing deterministic outcomes from declarative input. Robots may become LOST if they traverse beyond the grid boundary, leaving a persistent "scent" to prevent future robots from falling off at the same position.

## Problem

Process a sequence of robots navigating a rectangular grid:

- Robots move sequentially
- Maintain shared state (scent positions)
- Produce deterministic final states

## Architecture

```text
RedDune
├── src
│   ├── Core                        # C# domain & application logic
│   │   ├── Domain                  # Coordinates, Orientation, Command, Robot, Grid, Scent
│   │   ├── Application             # SimulationEngine, DTOs
│   │   └── RedDune.Core.csproj
│   │
│   ├── Api                         # C# Minimal API host
│   │   ├── Endpoints
│   │   ├── Requests
│   │   ├── Responses
│   │   └── RedDune.Api.csproj
│   │
│   └── Console                     # TypeScript CLI
│       ├── src                     # cli.ts, apiClient.ts, inputParser.ts
│       └── package.json
│
├── docs
│   ├── architecture                     # Architecture Decision Records (ADRs)
│   │   └── decisions.md                 # Complete decision log (ADR-001 to ADR-020)
│   ├── BRD.md                          # Business Requirements Document
│   └── sample-data
│       ├── inputs.txt                  # Sample input files
│       └── outputs.txt                 # Expected outputs
│
└── tests
    ├── RedDune.Tests               # C# unit tests
    └── console/
        └── typescript.tests        # TypeScript tests
```

## Key Decisions

See [docs/architecture/decisions.md](docs/architecture/decisions.md) for the complete Architecture Decision Log (ADRs 001–020).

| Decision | Reason |
| --- | --- |
| ✅ Monolith first | Minimize accidental complexity |
| ✅ Minimal API | Realistic consumption boundary without distributed systems |
| ✅ TypeScript CLI | Consumer-side thinking without UI noise |
| ❌ No Database | Execution-scoped lifecycle, persistence deferred |
| ❌ No Frontend UI | Presentation complexity deferred |

## Documentation

| Document | Description |
| --- | --- |
| [Business Requirements Document](docs/BRD.md) | Project purpose, problem statement, non-functional requirements |
| [Architecture Decisions](docs/architecture/decisions.md) | Complete ADR log (20 decisions) with rationale and trade-offs |
| [Future Evaluation](docs/future-evaluation.md) | System evolution scenarios and client implementation guidance |
| [Client Next Steps](docs/client-next-steps.md) | Integration examples, troubleshooting, deployment checklist |

## Quick Start

### Prerequisites

- .NET 10.0.202 SDK
- Node.js 18+

### Build & Run API

```bash
# Restore and build
dotnet build

# Run API (Terminal 1)
dotnet run --project src/Api/RedDune.Api
```

### Run CLI Client

```bash
# Build TypeScript (Terminal 2)
cd src/Console/typescript
npm install
npm run build

# Run with classic input format
echo -e "5 3\n1 1 E\nRFRFFFRF" | node dist/cli.js
# Output: (1, 0) E

# Or with multiple robots
echo -e "5 3\n1 1 E\nRFRFFFRF\n3 2 N\nFLLFLLF" | node dist/cli.js
# Output:
# (1, 0) E
# (3, 3) N LOST
```

## Testing

### C# Tests

```bash
dotnet test
```

### TypeScript Tests

```bash
# Run from the tests directory
cd tests/console/typescript.tests
npm install
npm test
```

### Lint

```bash
cd src/Console/typescript
npm run lint
```

## API Usage

### POST /api/simulation

```bash
curl -X POST http://localhost:5000/api/simulation \
  -H "Content-Type: application/json" \
  -d '{
    "gridWidth": 5,
    "gridHeight": 3,
    "robots": [
      { "x": 1, "y": 1, "orientation": "E", "commands": "RFRFFFRF" }
    ]
  }'
```

Response:

```json
{
  "robots": [
    { "x": 1, "y": 0, "orientation": "E", "isLost": false }
  ]
}
```

## Sample Input/Output

**Input:**

```text
5 3
1 1 E
RFRFFFRF
3 2 N
FLLFLLF
```

**Output:**

```text
(1, 0) E
(3, 3) N LOST
```

## Project Structure

- `src/Core/RedDune.Core/` - Domain entities and application logic
- `src/Api/RedDune.Api/` - Minimal API host
- `src/Console/typescript/` - TypeScript CLI consumer
- `tests/RedDune.Tests/` - C# unit tests
- `tests/console/typescript.tests/` - TypeScript tests

## Non-Functional Requirements

- Runtime determinism: Required
- External dependencies: None (runtime)
- Persistence: In-memory only
- UI: Explicitly deferred

## Constraints

- Grid dimensions: Max 50 x 50
- Command string length: Max 100 characters
- Valid coordinate values: 0 to (dimension - 1)

## Sample Data

Sample inputs are provided in `docs/sample-data/inputs.txt`. Each blank line separates different simulations, but you must run them one at a time:

```bash
cd src/Console/typescript

# Run first simulation only (extract lines 1-6 from inputs.txt):
$ head -n 6 docs/sample-data/inputs.txt | node dist/cli.js
(1, 1) E
(3, 2) N LOST
(0, 2) W

# Or run with echo for custom input:
echo "5 3" | node dist/cli.js
echo -e "5 3\n1 1 E\nRFRFFFRF" | node dist/cli.js

# Run with a file:
node dist/cli.js < my-simulation.txt
```

Example with custom input:
```bash
echo "5 3" | node dist/cli.js     # Grid 5x3
echo "1 1 E" | node dist/cli.js   # Robot at (1,1) facing East
echo "RFRFRFRF" | node dist/cli.js  # Commands
```

Expected outputs are in `docs/sample-data/outputs.txt` (one-to-one with each simulation in inputs.txt).
