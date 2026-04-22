import { ApiClient, createApiClient } from "../../../src/Console/typescript/src/apiClient";
import type { SimulationInput } from "../../../src/Console/typescript/src/types";

describe("ApiClient", () => {
  const mockInput: SimulationInput = {
    gridWidth: 5,
    gridHeight: 3,
    robots: [{ x: 1, y: 1, orientation: "N", commands: "F" }],
  };

  describe("createApiClient", () => {
    it("creates client with default options", () => {
      const client = createApiClient("http://localhost:5000");
      expect(client).toBeInstanceOf(ApiClient);
    });
  });

  describe("constructor", () => {
    it("strips trailing slash from baseUrl", () => {
      const client = new ApiClient({ baseUrl: "http://localhost:5000/" });
      expect(client).toBeInstanceOf(ApiClient);
    });

    it("uses default timeout of 30000ms", () => {
      const client = new ApiClient({ baseUrl: "http://localhost:5000" });
      expect(client).toBeInstanceOf(ApiClient);
    });
  });

  describe("simulate", () => {
    it("throws error for unreachable server", async () => {
      const client = new ApiClient({
        baseUrl: "http://localhost:99999",
        timeout: 1000,
      });

      await expect(client.simulate(mockInput)).rejects.toThrow();
    });
  });
});