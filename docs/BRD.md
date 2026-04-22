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
├── docs
│   ├── BRD.md
├── src
│   ├── Core                            # C# domain & application logic
│   │   ├── Domain
│   │   │   ├── Coordinates
│   │   │   ├── Orientation
│   │   │   ├── Robot
│   │   │   ├── World
│   │   │   └── Scent
│   │   ├── Application
│   │   │   ├── SimulationEngine
│   │   │   ├── Parsers
│   │   │   └── DTOs
│   │   └── Core.csproj
│   │
│   ├── Api                             # C# Minimal API host
│   │   ├── Endpoints
│   │   ├── Requests
│   │   ├── Responses
│   │   ├── Program.cs
│   │   └── Api.csproj
│   │
│   ├── Console                          # TypeScript CLI (no UI)
│   │   ├── src
│   │   │   ├── cli.ts
│   │   │   ├── apiClient.ts
│   │   │   ├── inputParser.ts
│   │   │   └── index.ts
│   │   ├── tests
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── README.md
└── Tests
│   ├── RedDune.Tests
│   │   ├── Domain.Tests
│   │   ├── Application.Tests
│   │   ├── Api.Tests
│   │   └── RedDune.Tests.csproj
│   │
│   └── typescript.Tests
│       ├── cli.test.ts
│       ├── contract.test.ts
│       └── jest.config.js
│
│
├── .editorconfig
├── .gitignore
├── README.md
└── RedDune.sln

```

## Key Architecture Decisions (Intentional)

✅ Monolith
Chosen to minimise accidental complexity and reflect early‑phase client discovery work.
✅ Minimal API (C#)
Provides a realistic consumption boundary without introducing distributed systems overhead.
✅ TypeScript CLI Client
Demonstrates consumer‑side thinking and contract awareness without UI noise.
❌ Database
Not required due to execution‑scoped lifecycle. Persistence explicitly deferred.
❌ Frontend UI
Adds presentation complexity without improving confidence in correctness.

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
| | Solution | Scaffold .NET solution and project structure |
| Sprint 2 | Core Domain | |
| | Domain | Model coordinates, orientation, and commands |
| | | Implement robot movement rules |
| | | Implement grid boundaries and scent tracking |
| | | Cover robot movement and boundary loss scenarios |
| Sprint 3 | Application Layer | |
| | Application | Implement simulation engine |
| | Tests | Scenario-based simulation tests |
| Sprint 4 | Execution Boundary | |
| | Api | Add minimal Api endpoint for simulation |
| | Tests | Api-level integration test |
| Sprint 5 | Typescript Consumer | |
| | Client | Scaffold Typescript CLI |
| | Client | Consume simulation Api |
| | Tests | Client contract and happy-path tests |
| Sprint 6 | Decision Transparency | |
| | Docs | Add architecture decision log |
| | Docs | Add future evaluation and client next steps |

## Testing Strategy

### C#

- Unit Tests for:
  - Turns
  - Forward movement
  - Boundary crossing
  - Scent behaviour
- Scenario tests for full robot sequences.
- Minimal Api integration test

### Typescript

- Contract tests against known input/output
- CLI parsing tests
- Error handling (invalid input)

### Testing Principles

- Behaviour over coverage
- Deterministic inputs
- Readable assertions
