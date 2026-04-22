#!/usr/bin/env node
"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
const readline = __importStar(require("readline"));
const apiClient_1 = require("./apiClient");
const inputParser_1 = require("./inputParser");
function printUsage() {
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
  echo -e "5 3\\n1 1 E\\nRFRFFFRF" | node dist/cli.js
  node dist/cli.js < my-simulation.txt

  # Multiple simulations - run separately:
  head -n 3 docs/sample-data/inputs.txt | node dist/cli.js
`);
}
function readStdin() {
    return new Promise((resolve) => {
        const lines = [];
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
async function main(argv) {
    const options = {
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
        }
        else if (arg === "-v" || arg === "--verbose") {
            options.verbose = true;
        }
    }
    if (options.verbose) {
        console.error(`RedDune CLI connecting to ${options.url}`);
    }
    const inputLines = await readStdin();
    const trimmedLines = inputLines
        .map((l) => l.trim())
        .filter((l) => l.length > 0);
    if (trimmedLines.length === 0) {
        console.error("Error: No input provided");
        console.error("Usage: reddune < grid.txt or echo '5 3' | reddune");
        return 1;
    }
    const parseResult = (0, inputParser_1.parseInput)(trimmedLines);
    if ("message" in parseResult) {
        console.error(`Error on line ${parseResult.line}: ${parseResult.message}`);
        return 1;
    }
    const simulationInput = {
        gridWidth: parseResult.gridWidth,
        gridHeight: parseResult.gridHeight,
        robots: parseResult.robots,
    };
    try {
        const client = (0, apiClient_1.createApiClient)(options.url);
        const result = await client.simulate(simulationInput);
        for (const robot of result.robots) {
            console.log((0, inputParser_1.formatRobotOutput)(robot));
        }
        return 0;
    }
    catch (error) {
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
//# sourceMappingURL=cli.js.map