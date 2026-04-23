#!/usr/bin/env node

import * as readline from "readline";
import { createApiClient } from "./apiClient";
import { parseInput, formatRobotOutput } from "./inputParser";
import type { SimulationInput } from "./types";

interface CliOptions {
  url: string;
  verbose: boolean;
}

function printUsage(): void {
  console.log(`
RedDune CLI - Martian Robots Simulator

Usage: reddune [options] < input.txt
       echo "WIDTH HEIGHT" | reddune
       cat input.txt | reddune

Options:
  -u, --url <url>    API base URL (default: http://localhost:5000)
  -v, --verbose     Verbose output
  -h, --help      Show this help

Input Format (one simulation at a time):
  WIDTH HEIGHT
  X Y ORIENTATION
  COMMANDS
  (repeat for additional robots)

Examples:
  # Bash / WSL:
  echo -e "5 3\n1 1 E\nRFRFFFRF" | node dist/cli.js
  node dist/cli.js < my-simulation.txt
  # PowerShell:
  Write-Output "5 3\`n1 1 E\`nRFRFFFRF" | node dist/cli.js
  Get-Content my-simulation.txt | node .\dist\cli.js

  # Multiple simulations - run separately:
  head -n 3 docs/sample-data/inputs.txt | node dist/cli.js
`);
}

function readStdin(): Promise<string[]> {
  return new Promise((resolve) => {
    const lines: string[] = [];
    const rl = readline.createInterface({
      input: process.stdin,
      output: process.stdout,
      terminal: false,
    });

    rl.on("line", (line) => {
      lines.push(line);
    });

    rl.on("close", () => {
      resolve(lines);
    });
  });
}

async function main(argv: string[]): Promise<number> {
  const options: CliOptions = {
    url: "http://localhost:5000",
    verbose: false,
  };

  for (let i = 0; i < argv.length; i++) {
    const arg = argv[i];
    if (arg === "-h" || arg === "--help") {
      printUsage();
      return 0;
    }
    if (arg === "-u" || arg === "--url") {
      options.url = argv[++i] ?? "http://localhost:5000";
    } else if (arg === "-v" || arg === "--verbose") {
      options.verbose = true;
    }
  }

  if (options.verbose) {
    console.error(`RedDune CLI connecting to ${options.url}`);
  }

  const inputLines = await readStdin();
  const trimmedLines = inputLines.map((l) => l.trim());

  if (trimmedLines.filter((l) => l.length > 0).length === 0) {
    console.error("Error: No input provided");
    console.error("Usage: reddune < grid.txt or echo '5 3' | reddune");
    return 1;
  }

  const parseResult = parseInput(trimmedLines);
  if ("message" in parseResult) {
    console.error(`Error on line ${parseResult.line}: ${parseResult.message}`);
    return 1;
  }

  const simulationInput: SimulationInput = {
    gridWidth: parseResult.gridWidth,
    gridHeight: parseResult.gridHeight,
    robots: parseResult.robots,
  };

  try {
    const client = createApiClient(options.url);
    const result = await client.simulate(simulationInput);

    for (const robot of result.robots) {
      console.log(formatRobotOutput(robot));
    }

    return 0;
  } catch (error) {
    console.error(`Error: ${error instanceof Error ? error.message : String(error)}`);
    return 1;
  }
}

main(process.argv.slice(2))
  .then((exitCode) => process.exit(exitCode))
  .catch((error) => {
    console.error(`Error: ${error instanceof Error ? error.message : String(error)}`);
    process.exit(1);
  });