import type { SimulationOutput, ParseError, ParsedInput } from "./types.js";
export declare function parseInput(lines: string[]): ParsedInput | ParseError;
export declare function parseGridDimensions(line: string): {
    width: number;
    height: number;
} | null;
export declare function parseRobotLine(line: string): {
    x: number;
    y: number;
    orientation: string;
} | null;
export declare function isValidOrientation(o: string): boolean;
export declare function formatRobotOutput(robot: SimulationOutput["robots"][number]): string;
//# sourceMappingURL=inputParser.d.ts.map