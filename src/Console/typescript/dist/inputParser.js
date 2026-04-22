"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.parseInput = parseInput;
exports.parseGridDimensions = parseGridDimensions;
exports.parseRobotLine = parseRobotLine;
exports.isValidOrientation = isValidOrientation;
exports.formatRobotOutput = formatRobotOutput;
const ORIENTATIONS = new Set(["N", "E", "S", "W"]);
const VALID_COMMANDS = new Set(["L", "R", "F"]);
const MAX_GRID_SIZE = 50;
const MAX_COMMAND_LENGTH = 100;
function parseInput(lines) {
    if (lines.length < 3) {
        return { line: 0, message: "Input must have at least 3 lines: grid dimensions and one robot" };
    }
    const gridMatch = lines[0]?.match(/^(\d+)\s+(\d+)$/);
    if (!gridMatch) {
        return { line: 1, message: "First line must be grid dimensions: WIDTH HEIGHT" };
    }
    const gridWidth = parseInt(gridMatch[1], 10);
    const gridHeight = parseInt(gridMatch[2], 10);
    if (gridWidth > MAX_GRID_SIZE || gridHeight > MAX_GRID_SIZE) {
        return {
            line: 1,
            message: `Grid dimensions cannot exceed ${MAX_GRID_SIZE}x${MAX_GRID_SIZE}`,
        };
    }
    const robots = [];
    let lineNum = 1;
    let robotIndex = 0;
    while (lineNum < lines.length) {
        const line = lines[lineNum];
        if (!line || line.trim() === "") {
            lineNum++;
            continue;
        }
        const robotMatch = line.match(/^(\d+)\s+(\d+)\s+([NESW])$/);
        if (!robotMatch) {
            return {
                line: lineNum + 1,
                message: `Robot ${robotIndex + 1} position must be: X Y O`,
            };
        }
        const x = parseInt(robotMatch[1], 10);
        const y = parseInt(robotMatch[2], 10);
        const orientation = robotMatch[3].toUpperCase();
        lineNum++;
        if (lineNum >= lines.length) {
            return {
                line: lineNum + 1,
                message: `Robot ${robotIndex + 1} is missing commands line`,
            };
        }
        const commandsLine = lines[lineNum];
        if (!commandsLine) {
            return {
                line: lineNum + 1,
                message: `Robot ${robotIndex + 1} commands cannot be empty`,
            };
        }
        const commands = commandsLine.trim().toUpperCase();
        if (commands.length > MAX_COMMAND_LENGTH) {
            return {
                line: lineNum + 1,
                message: `Commands cannot exceed ${MAX_COMMAND_LENGTH} characters`,
            };
        }
        for (const cmd of commands) {
            if (!VALID_COMMANDS.has(cmd)) {
                return {
                    line: lineNum + 1,
                    message: `Invalid command '${cmd}' in robot ${robotIndex + 1}. Use L, R, or F`,
                };
            }
        }
        robots.push({ x, y, orientation, commands });
        lineNum++;
        robotIndex++;
    }
    return { gridWidth, gridHeight, robots };
}
function parseGridDimensions(line) {
    const match = line.match(/^(\d+)\s+(\d+)$/);
    if (!match)
        return null;
    return {
        width: parseInt(match[1], 10),
        height: parseInt(match[2], 10),
    };
}
function parseRobotLine(line) {
    const match = line.match(/^(\d+)\s+(\d+)\s+([NESWnesw])$/i);
    if (!match)
        return null;
    return {
        x: parseInt(match[1], 10),
        y: parseInt(match[2], 10),
        orientation: match[3].toUpperCase(),
    };
}
function isValidOrientation(o) {
    return ORIENTATIONS.has(o.toUpperCase());
}
function formatRobotOutput(robot) {
    const lost = robot.isLost ? " LOST" : "";
    return `(${robot.x}, ${robot.y}) ${robot.orientation}${lost}`;
}
//# sourceMappingURL=inputParser.js.map