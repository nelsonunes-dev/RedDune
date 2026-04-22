import { parseInput, formatRobotOutput } from "../../../src/Console/typescript/src/inputParser";

describe("CLI helpers", () => {
  describe("parseInput integration", () => {
    it("parses classic input format correctly", () => {
      const lines = ["5 3", "1 1 E", "RFRFFFRF", "3 2 N", "FLLFLLF"];
      const result = parseInput(lines);

      expect("robots" in result).toBe(true);
      if ("robots" in result) {
        expect(result.robots.length).toBe(2);
        expect(result.gridWidth).toBe(5);
        expect(result.gridHeight).toBe(3);
      }
    });

    it("rejects invalid grid dimensions", () => {
      const lines = ["abc", "1 1 E", "RFRF"];
      const result = parseInput(lines);

      expect("message" in result).toBe(true);
      if ("message" in result) {
        expect(result.message).toContain("grid dimensions");
      }
    });
  });

  describe("formatRobotOutput", () => {
    it("formats output matching expected format", () => {
      const output = formatRobotOutput({
        x: 0,
        y: 0,
        orientation: "W",
        isLost: true,
      });

      expect(output).toBe("(0, 0) W LOST");
    });

    it("formats non-lost robot correctly", () => {
      const output = formatRobotOutput({
        x: 1,
        y: 2,
        orientation: "N",
        isLost: false,
      });

      expect(output).toBe("(1, 2) N");
    });
  });
});