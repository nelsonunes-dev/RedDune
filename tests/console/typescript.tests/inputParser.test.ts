import {
  parseInput,
  parseGridDimensions,
  parseRobotLine,
  isValidOrientation,
  formatRobotOutput,
} from "../../../src/Console/typescript/src/inputParser";

describe("inputParser", () => {
  describe("parseGridDimensions", () => {
    it("parses valid grid dimensions", () => {
      const result = parseGridDimensions("5 3");
      expect(result).toEqual({ width: 5, height: 3 });
    });

    it("returns null for invalid format", () => {
      expect(parseGridDimensions("abc")).toBeNull();
      expect(parseGridDimensions("5")).toBeNull();
      expect(parseGridDimensions("")).toBeNull();
    });
  });

  describe("parseRobotLine", () => {
    it("parses valid robot line", () => {
      const result = parseRobotLine("1 2 N");
      expect(result).toEqual({ x: 1, y: 2, orientation: "N" });
    });

    it("handles lowercase orientation", () => {
      const result = parseRobotLine("1 2 n");
      expect(result).toEqual({ x: 1, y: 2, orientation: "N" });
    });

    it("returns null for invalid format", () => {
      expect(parseRobotLine("abc")).toBeNull();
      expect(parseRobotLine("1 2")).toBeNull();
    });
  });

  describe("isValidOrientation", () => {
    it("validates correct orientations", () => {
      expect(isValidOrientation("N")).toBe(true);
      expect(isValidOrientation("E")).toBe(true);
      expect(isValidOrientation("S")).toBe(true);
      expect(isValidOrientation("W")).toBe(true);
    });

    it("rejects invalid orientations", () => {
      expect(isValidOrientation("X")).toBe(false);
      expect(isValidOrientation("")).toBe(false);
    });
  });

  describe("parseInput", () => {
    it("parses single robot", () => {
      const lines = ["5 3", "1 1 E", "RFRFFFRF"];
      const result = parseInput(lines);
      expect(result).toEqual({
        gridWidth: 5,
        gridHeight: 3,
        robots: [{ x: 1, y: 1, orientation: "E", commands: "RFRFFFRF" }],
      });
    });

    it("parses multiple robots", () => {
      const lines = ["5 3", "1 1 E", "RFRF", "2 2 N", "FF"];
      const result = parseInput(lines);
      expect(result).toEqual({
        gridWidth: 5,
        gridHeight: 3,
        robots: [
          { x: 1, y: 1, orientation: "E", commands: "RFRF" },
          { x: 2, y: 2, orientation: "N", commands: "FF" },
        ],
      });
    });

    it("returns error for missing commands", () => {
      const lines = ["5 3", "1 1 E", "RFRF", "2 2 N"];
      const result = parseInput(lines);
      expect(result).toEqual({ line: 5, message: "Robot 2 is missing commands line" });
    });

    it("returns error for invalid command", () => {
      const lines = ["5 3", "1 1 E", "XYZ"];
      const result = parseInput(lines);
      expect(result).toEqual({
        line: 3,
        message: "Invalid command 'X' in robot 1. Use L, R, or F",
      });
    });
  });

  describe("formatRobotOutput", () => {
    it("formats robot not lost", () => {
      const robot = { x: 1, y: 2, orientation: "N", isLost: false };
      expect(formatRobotOutput(robot)).toBe("(1, 2) N");
    });

    it("formats robot lost", () => {
      const robot = { x: 3, y: 3, orientation: "N", isLost: true };
      expect(formatRobotOutput(robot)).toBe("(3, 3) N LOST");
    });
  });
});