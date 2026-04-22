# Business Requirements Document

## Purpose

The purpose of RedDune is to simulate robotic movement on a bounded grid, producing deterministic outcomes from declarative input, while explicitly demonstrating how such a system could evolve in a real client environment without premature complexity.

The project prioritises:

- Correctness
- Clarity
- Explicit trade‑offs
- Evolvability

## Problem Statement

The system must process a sequence of robots navigating a rectangular grid, following simple movement commands. Robots may become lost if they traverse beyond the grid boundary, leaving a persistent “scent” to prevent future robots from being lost at the same coordinate.

The system must:

- Process robots sequentially
- Maintain shared state (scent positions)
- Produce deterministic final states
- Be easily executable and testable

## Non-Functional Requirements

| Requirement | Decision |
| --- | --- |
| Runtime determinism | Required |
| External Dependencies | None (runtime) |
| Persistence | In-memory only |
| UI | Explicitly deferred |
| Test coverage | Unit + Behavioural |
| Reproduction execution | Required |
| Extensibility | Designed, not over built |

## Architecture Overview

### Core Principles

- Monolith first
- Single source of truth
- Thin boundaries
- Explicit seams

### Logical Components

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
│   └── sample-data
│       ├── inputs.txt            # Sample input files (one simulation per blank line)
│       └── outputs.txt
│
└── tests
    ├── RedDune.Tests               # C# unit tests
    └── console/
        └── typescript.tests        # TypeScript tests
```

## Key Architecture Decisions (Intentional)

| Decision | Reason |
| --- | --- |
| ✅ Monolith first | Minimize accidental complexity |
| ✅ Minimal API | Realistic consumption boundary without distributed systems |
| ✅ TypeScript CLI | Consumer-side thinking without UI noise |
| ❌ No Database | Execution-scoped lifecycle, persistence deferred |
| ❌ No Frontend UI | Presentation complexity deferred |

## Success Criteria

- Correct processing of all sample inputs
- Robots fall off grid correctly and leave scent
- Subsequent robots respect scent
- Clear reasoning documented
- Reviewer can run the system in minutes
- Reviewer can explain why choices were made

## Sprint Planning

| Sprint | User Story | Tasks |
| --- | --- | --- |
| Sprint 1 | Foundations | Add BRD and problem analysis |
| Sprint 2 | Solution | Scaffold .NET solution and project structure |
| Sprint 3 | Core Domain | |
| | Domain Layer | Model coordinates, orientation, and commands |
| | | Implement robot movement rules |
| | | Implement grid boundaries and scent tracking |
| | | Cover robot movement and boundary loss scenarios |
| | Application Layer | Implement simulation engine |
| Sprint 4 | Api | |
| | Api | Add minimal Api endpoint for simulation |
| Sprint 5 | Tests | |
| | Tests | Scenario-based simulation tests |
| | Tests | Api-level integration test |
| Sprint 6 | Typescript Consumer | |
| | Client | Scaffold Typescript CLI |
| | Client | Consume simulation Api |
| | Tests | Client contract and happy-path tests |

## Testing Strategy

### C#

- Unit Tests for:
  - Turns
  - Forward movement
  - Boundary crossing
  - Scent behaviour
- Scenario tests for full robot sequences.
- Minimal Api integration test

### Typescript (tests/console/typescript.tests/)

- Contract tests against known input/output
- CLI parsing tests
- ApiClient tests
- Error handling (invalid input)

### Testing Principles

- Behaviour over coverage
- Deterministic inputs
- Readable assertions
